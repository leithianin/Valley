using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorAgentBehave : MonoBehaviour
{
    [SerializeField] private VisitorData datas = new VisitorData();

    private bool isWalking;

    private Action StartPath;
    private Action EndPath;

    [Header("Tests")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PathPoint lastPoint;
    [SerializeField] private PathPoint currentPoint;

    private void Awake()
    {
        datas.agent = agent;
        datas.lastPoint = lastPoint;
        datas.currentPoint = currentPoint;
    }

    // Start is called before the first frame update
    void Start()
    {
        datas.path = VisitorManager.ChoosePath();
        AskToWalk();
    }

    // Update is called once per frame
    void Update()
    {
        if (!datas.agent.pathPending && isWalking)
        {
            if (datas.agent.remainingDistance <= datas.agent.stoppingDistance)
            {
                if (!datas.agent.hasPath || datas.agent.velocity.sqrMagnitude == 0f)
                {
                    ReachDestination();
                }
            }
        }
    }

    private void AskToWalk()
    {
        isWalking = VisitorManager.ChooseNextDestination(datas);
    }

    private void ReachDestination()
    {
        isWalking = false;
        VisitorManager.MakeVisitorWait(.5f, AskToWalk);
    }
}
