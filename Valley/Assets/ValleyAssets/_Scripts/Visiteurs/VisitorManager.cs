using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class VisitorManager : MonoBehaviour
{
    private static VisitorManager instance;

    [SerializeField] private PathPoint visitorSpawnPoint;

    [SerializeField] private List<VisitorScriptable> visitorTypes;
    [SerializeField] private float spawnRate = .2f;
    [SerializeField] private int maxSpawn = 100;
    [SerializeField] private List<VisitorAgentBehave> visitorPool;

    [SerializeField] private UnityEvent PlayOnVisitorSpawn;

    public static List<VisitorAgentBehave> GetVisitors => instance.GetAllUsedVisitor();

    public static Action<int> OnVisitorsUpdate;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnVisitorContinue());
    }

    private bool SpawnVisitor()
    {
        if (Valley_PathManager.HasAvailablePath(visitorSpawnPoint))
        {
            VisitorAgentBehave newVisitor = GetAvailableVisitor();

            Vector2 rng = UnityEngine.Random.insideUnitCircle * 8f;
            Vector3 spawnPosition = visitorSpawnPoint.Position + new Vector3(rng.x, 0, rng.y);

            NavMeshHit hit;
            if (newVisitor != null && NavMesh.SamplePosition(spawnPosition, out hit, .5f, NavMesh.AllAreas))
            {
                newVisitor.SetVisitor(visitorSpawnPoint, spawnPosition, visitorTypes[UnityEngine.Random.Range(0,visitorTypes.Count)]);
            }

            OnVisitorsUpdate?.Invoke(UsedVisitorNumber());

            return true;
        }
        return false;
    }

    public static void DeleteVisitor(VisitorAgentBehave vab)
    {
        vab.UnsetVisitor();
        OnVisitorsUpdate?.Invoke(instance.UsedVisitorNumber());
    }

    public static Valley_PathData ChoosePath(PathPoint spawnPoint, List<LandMarkType> visitorObjectives, List<InterestPointType> visitorInterest, out LandMarkType newObjective)
    {
        List<Valley_PathData> possiblesPath = Valley_PathManager.GetAllPossiblePath(spawnPoint);

        return instance.GetMostInterestingPath(visitorObjectives, visitorInterest, possiblesPath, out newObjective);
    }

    public static bool ChooseNextDestination(VisitorData visitor)
    {
        if (visitor.currentPoint != null)
        {
            //PathPoint newPathpoint = visitor.currentPoint.GetNextPathPoint(visitor.lastPoint, visitor.path);

            PathFragmentData pathData = visitor.path.GetRandomDestination(visitor.currentPoint, visitor.lastPoint);

            visitor.SetDestination(pathData);
            return true;
        }
        return false;
    }

    public static void MakeVisitorWait(float waitTime, Action callback) //CODE REVIEW : Voir pour eviter de passer par 3 fonctions pour faire un Wait
    {
        instance.StartWaitCoroutine(waitTime, callback);
    }

    private void StartWaitCoroutine(float waitTime, Action callback)
    {
        StartCoroutine(VisitorWaiting(waitTime, callback));
    }

    IEnumerator VisitorWaiting(float waitTime, Action callback)
    {
        yield return new WaitForSeconds(waitTime);
        callback?.Invoke();
    }

    IEnumerator SpawnVisitorContinue() //CODE REVIEW : Voir comment on peut g�rer le spawn des visiteurs. Commencer � mettre des datas (Spawn rate, delay between spawn, ...)
    {
        int toSpawn = UnityEngine.Random.Range(ValleyManager.AttractivityLevel * 3, ValleyManager.AttractivityLevel + 5);

        bool hasPlayFeedback = false;

        for (int i = 0; i < toSpawn; i++)
        {
            if (UsedVisitorNumber() < maxSpawn)
            {
                yield return new WaitForSeconds(.5f);
                bool hasSpawned = SpawnVisitor();
                if(!hasPlayFeedback && hasSpawned)
                {
                    PlayOnVisitorSpawn?.Invoke();
                    hasPlayFeedback = true;
                }
            }
        }
        if (UsedVisitorNumber() > 0)
        {
            yield return new WaitForSeconds(spawnRate);
        }
        else
        {
            yield return new WaitForSeconds(.5f);
        }
        StartCoroutine(SpawnVisitorContinue());
    }

    private Valley_PathData GetMostInterestingPath(List<LandMarkType> visitorObjectives, List<InterestPointType> visitorInterest, List<Valley_PathData> possiblesPath, out LandMarkType newObjective)
    {
        // REVOIR LES CALCULS DE CHEMINS //
        newObjective = LandMarkType.None;

        List<Valley_PathData> firstPickPhase = new List<Valley_PathData>();
        for (int k = 0; k < visitorObjectives.Count; k++)
        {
            for (int i = 0; i < possiblesPath.Count; i++)
            {
                if (possiblesPath[i].ContainsLandmark(visitorObjectives[k]))
                {
                    firstPickPhase.Add(possiblesPath[i]);
                }
            }

            if(firstPickPhase.Count > 0)
            {
                newObjective = visitorObjectives[k];
                break;
            }
        }

        if (firstPickPhase.Count <= 0)
        {
            firstPickPhase = new List<Valley_PathData>(possiblesPath);
        }

        Valley_PathData toReturn = firstPickPhase[UnityEngine.Random.Range(0, firstPickPhase.Count)];
        List<int> scores = new List<int>();
        int maxScore = 0;

        for (int i = 0; i < firstPickPhase.Count; i++)
        {
            int pathScore = 1;
            for (int j = 0; j < visitorInterest.Count; j++)
            {
                pathScore += firstPickPhase[i].GetNumberInterestPoint(visitorInterest[j]);
            }
            scores.Add(pathScore);
            maxScore += pathScore;
        }

        int chosenScore = UnityEngine.Random.Range(0, maxScore+1);

        for(int i = 0; i < scores.Count; i++)
        {
            chosenScore -= scores[i];
            if(chosenScore <= 0)
            {
                toReturn = firstPickPhase[i];
                break;
            }
        }

        return toReturn;
    }

    private List<VisitorAgentBehave> GetAllUsedVisitor()
    {
        List<VisitorAgentBehave> toReturn = new List<VisitorAgentBehave>();
        for (int i = 0; i < visitorPool.Count; i++)
        {
            if (visitorPool[i].gameObject.activeSelf)
            {
                toReturn.Add(visitorPool[i]);
            }
        }
        return toReturn;
    }

    private VisitorAgentBehave GetAvailableVisitor() //CODE REVIEW : N�c�ssit� d'un syst�me de Pool global ?
    {
        for(int i = 0; i < visitorPool.Count; i++)
        {
            if(!visitorPool[i].gameObject.activeSelf)
            {
                return visitorPool[i];
            }
        }
        return null;
    }

    private int UsedVisitorNumber()
    {
        int toReturn = 0;
        for (int i = 0; i < visitorPool.Count; i++)
        {
            if (visitorPool[i].gameObject.activeSelf)
            {
                toReturn++;
            }
        }

        return toReturn;
    }
}
