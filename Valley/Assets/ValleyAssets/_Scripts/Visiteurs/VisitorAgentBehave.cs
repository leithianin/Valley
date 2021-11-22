using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorAgentBehave : MonoBehaviour
{
    [Header("Dev datas")]
    [SerializeField] private VisitorData datas = new VisitorData();

    private PathPoint spawnPoint;
    private bool isWalking;

    private Action StartPath;
    private Action EndPath;

    // Update is called once per frame
    void Update()
    {
        if (!datas.agent.pathPending && isWalking)
        {
            if (datas.agent.remainingDistance <= datas.agent.stoppingDistance)
            {
                if (!datas.agent.hasPath || datas.agent.velocity.sqrMagnitude == 0f) //CODE REVIEW : Voir pour simplifier les "if" et les rassembler
                {
                    ReachDestination();
                }
            }
        }
    }

    public void SetVisitor(PathPoint nSpawnPoint)
    {
        spawnPoint = nSpawnPoint;

        datas.lastPoint = null;
        datas.currentPoint = nSpawnPoint;

        transform.position = nSpawnPoint.Position;

        gameObject.SetActive(true);

        datas.path = VisitorManager.ChoosePath();
        AskToWalk();

        if (datas.currentPoint == nSpawnPoint)
        {
            datas.currentPoint = nSpawnPoint.GetNextPathPoint(datas.lastPoint, datas.path);
            AskToWalk();
        }
    }

    public void UnsetVisitor()
    {
        datas.currentPoint = null;

        gameObject.SetActive(false);
    }

    private void AskToWalk()
    {
        isWalking = VisitorManager.ChooseNextDestination(datas);
    }

    private void ReachDestination()
    {
        isWalking = false;
        if (datas.currentPoint == spawnPoint)
        {
            VisitorManager.RemoveVisitor(this);
        }
        else
        {
            VisitorManager.MakeVisitorWait(UnityEngine.Random.Range(0.5f,1.5f), AskToWalk);
        }
    }
}
