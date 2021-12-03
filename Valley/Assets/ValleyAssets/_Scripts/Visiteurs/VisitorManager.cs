using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorManager : MonoBehaviour
{
    private static VisitorManager instance;

    [SerializeField] private PathPoint visitorSpawnPoint;

    [SerializeField] private List<VisitorScriptable> visitorTypes;
    [SerializeField] private float spawnRate = .2f;
    [SerializeField] private int maxSpawn = 100;
    [SerializeField] private List<VisitorAgentBehave> visitorPool;

    public static List<VisitorAgentBehave> GetVisitors => instance.GetAllUsedVisitor();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnVisitorContinue());
    }

    private void SpawnVisitor()
    {
        if (Valley_PathManager.HasAvailablePath(visitorSpawnPoint))
        {
            VisitorAgentBehave newVisitor = GetAvailableVisitor();

            if (newVisitor != null)
            {
                newVisitor.SetVisitor(visitorSpawnPoint, visitorTypes[UnityEngine.Random.Range(0,visitorTypes.Count)]);
            }
        }
    }

    public static void RemoveVisitor(VisitorAgentBehave toRemove)
    {
        toRemove.UnsetVisitor();
    }

    public static Valley_PathData ChoosePath(PathPoint spawnPoint, List<LandMarkType> visitorObjectives, List<InterestPointType> visitorInterest)
    {
        List<Valley_PathData> possiblesPath = Valley_PathManager.GetAllPossiblePath(spawnPoint);

        return instance.GetMostInterestingPath(visitorObjectives, visitorInterest, possiblesPath);
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
        if(UsedVisitorNumber() < maxSpawn)
        {
            SpawnVisitor();
        }
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(SpawnVisitorContinue());
    }

    private Valley_PathData GetMostInterestingPath(List<LandMarkType> visitorObjectives, List<InterestPointType> visitorInterest, List<Valley_PathData> possiblesPath)
    {
        // REVOIR LES CALCULS DE CHEMINS //
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
                break;
            }
        }

        if (firstPickPhase.Count <= 0)
        {
            firstPickPhase = new List<Valley_PathData>(possiblesPath);
        }

        Valley_PathData toReturn = firstPickPhase[UnityEngine.Random.Range(0,possiblesPath.Count)];
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
