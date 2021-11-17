using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorAgentBehave : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    private bool isWalking;

    private Action StartPath;
    private Action EndPath;

    // Start is called before the first frame update
    void Start()
    {
        AskToWalk();
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && isWalking)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    ReachDestination();
                }
            }
        }
    }

    private void AskToWalk()
    {
        isWalking = VisitorManager.ChooseNextDestination(agent);
    }

    private void ReachDestination()
    {
        isWalking = false;
        VisitorManager.MakeVisitorWait(5f, AskToWalk);
    }
}
