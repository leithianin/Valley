using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorManager : MonoBehaviour
{
    private static VisitorManager instance;

    [SerializeField] private PathPoint visitorSpawnPoint;
    [SerializeField] private GameObject visitorPrefab;
    [SerializeField] private List<Transform> transTest;

    [SerializeField] private List<VisitorAgentBehave> visitorPool;

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
        VisitorAgentBehave newVisitor = GetAvailableVisitor();

        if (newVisitor != null)
        {
            newVisitor.SetVisitor(visitorSpawnPoint);
        }
    }

    public static void RemoveVisitor(VisitorAgentBehave toRemove)
    {
        toRemove.UnsetVisitor();
    }

    public static Valley_PathData ChoosePath()
    {
        return Valley_PathManager.GetRandomPath();
    }

    public static bool ChooseNextDestination(VisitorData visitor)
    {
        PathPoint newPathpoint = visitor.currentPoint.GetNextPathPoint(visitor.lastPoint, visitor.path);
        visitor.SetDestination(newPathpoint);
        return true;
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

    IEnumerator SpawnVisitorContinue()
    {
        SpawnVisitor();
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnVisitorContinue());
    }

    private VisitorAgentBehave GetAvailableVisitor()
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
}
