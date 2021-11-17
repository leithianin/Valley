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

    public static void ChoosePath()
    {

    }

    public static bool ChooseNextDestination(NavMeshAgent visitor)
    {
        visitor.destination = instance.transTest[UnityEngine.Random.Range(0, instance.transTest.Count)].position;
        return true;
    }

    public static void MakeVisitorWait(float waitTime, Action callback)
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
