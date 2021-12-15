using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class VisitorManager : MonoBehaviour
{
    private static VisitorManager instance;

    // The Spawn of the visitors.
    [SerializeField] private PathPoint visitorSpawnPoint;
    // A list of all available visitors types.
    [SerializeField] private List<VisitorScriptable> visitorTypes;
    // The time between the arriving of 2 bus of visitor.
    [SerializeField] private float spawnRate = .2f;
    // The max number of visitor in the valley.
    [SerializeField] private int maxSpawn = 100;
    // The pool of visitor.
    [SerializeField] private List<VisitorAgentBehave> visitorPool;

    [SerializeField, Tooltip("Feedback to play when a bus of visitor spawn.")] private UnityEvent PlayOnVisitorSpawn;

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

    /// <summary>
    /// Spawn a visitor in the valley.
    /// </summary>
    /// <returns>Return false if the visitor couldn't be spawn.</returns>
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

    /// <summary>
    /// Remove a visitor from the valley and disable it.
    /// </summary>
    /// <param name="vab">The visitor to remove.</param>
    public static void DeleteVisitor(VisitorAgentBehave vab)
    {
        vab.UnsetVisitor();
        OnVisitorsUpdate?.Invoke(instance.UsedVisitorNumber());
    }

    /// <summary>
    /// Choose a path depending of the visitor datas.
    /// </summary>
    /// <param name="spawnPoint">The PathPoint where the visitor spawn.</param>
    /// <param name="visitorObjectives">A list of all the objectives of the visitor.</param>
    /// <param name="visitorInterest">A list a all the interest of the visitor.</param>
    /// <param name="newObjective">The new objective for the visitor.</param>
    /// <returns>The choosen path.</returns>
    public static Valley_PathData ChoosePath(PathPoint spawnPoint, List<LandMarkType> visitorObjectives, List<InterestPointType> visitorInterest, out LandMarkType newObjective)
    {
        List<Valley_PathData> possiblesPath = Valley_PathManager.GetAllPossiblePath(spawnPoint);

        return instance.GetMostInterestingPath(visitorObjectives, visitorInterest, possiblesPath, out newObjective);
    }
    /// <summary>
    /// Choose the next destination on the path.
    /// </summary>
    /// <param name="visitor">The visitor that ask for the next way.</param>
    /// <returns>Return false if the visitor isn't trying to reach a PathPoint.</returns>
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
    /// <summary>
    /// Called when the visitor have to wait while doing nothing.
    /// </summary>
    /// <param name="waitTime">The amount of time to wait.</param>
    /// <param name="callback">The action to do after the visitor ends waiting.</param>
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

    /// <summary>
    /// Coroutine that handle the spawnrate of visitors.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnVisitorContinue() //CODE REVIEW : Voir comment on peut g�rer le spawn des visiteurs. Commencer � mettre des datas (Spawn rate, delay between spawn, ...)
    {
        int toSpawn = UnityEngine.Random.Range(ValleyManager.AttractivityLevel * 2, (ValleyManager.AttractivityLevel * 2) + 3);

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

    /// <summary>
    /// Calculate the most interesting path for a visitor.
    /// </summary>
    /// <param name="visitorObjectives">A list of all the objectives of the visitor.</param>
    /// <param name="visitorInterest">A list a all the interest of the visitor.</param>
    /// <param name="possiblesPath">A list of all the path the visitor can take.</param>
    /// <param name="newObjective">The new objective for the visitor.</param>
    /// <returns></returns>
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
    /// <summary>
    /// Get a list of all the visitor currently in the valley.
    /// </summary>
    /// <returns>A list of used visitor.</returns>
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
    /// <summary>
    /// Get the next visitor of the pool that is not used.
    /// </summary>
    /// <returns>A disabled visitor.</returns>
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

    /// <summary>
    /// Get the amount of visitor in the valley.
    /// </summary>
    /// <returns>The amount of visitor in the valley.</returns>
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
