using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorManager : MonoBehaviour
{
    private static VisitorManager instance;

    [SerializeField] private Transform visitorSpawnPosition;
    [SerializeField] private GameObject visitorPrefab;
    [SerializeField] private List<Transform> transTest;

    private void Awake()
    {
        instance = this;
    }

    private void SpawnVisitor()
    {
        Instantiate(visitorPrefab, visitorSpawnPosition, false);
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
}
