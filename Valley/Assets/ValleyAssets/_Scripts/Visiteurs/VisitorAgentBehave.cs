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

    public Vector2 GetPosition => new Vector2(transform.position.x, transform.position.z);

    public NavMeshAgent Agent => datas.agent;

    public VisitorData Data => datas;

    public ValleyArea currentArea;

    private bool doesAlreadyInteract;

    private int currentPathIndex;

    public VisitorScriptable visitorType;

    // Update is called once per frame
    void Update()
    {
        if (datas.agent.enabled && !datas.agent.pathPending && isWalking)
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

    public void SetVisitor(PathPoint nSpawnPoint, VisitorScriptable nType)
    {
        Valley_PathData wantedPath = VisitorManager.ChoosePath(nSpawnPoint);

        if (wantedPath != null)
        {
            visitorType = nType;

            datas.agent.speed = visitorType.Speed;
            datas.noiseMade = visitorType.SoundProduced;

            spawnPoint = nSpawnPoint;

            datas.path = wantedPath;
            datas.lastPoint = null;
            datas.currentPoint = nSpawnPoint;

            transform.position = nSpawnPoint.Position;

            gameObject.SetActive(true);


            AskToWalk();

            if (datas.currentPoint == nSpawnPoint)
            {
                datas.currentPoint = nSpawnPoint.GetNextPathPoint(datas.lastPoint, datas.path);
                AskToWalk();
            }

            SetNextDestination(currentPathIndex);

            datas.currentPoint.OnPointDestroyed += UnsetVisitor;
        }
    }

    public void UnsetVisitor()
    {
        datas.currentPoint = null;

        gameObject.SetActive(false);
    }

    #region Deplacement
    private void AskToWalk()
    {
        if (enabled)
        {
            isWalking = VisitorManager.ChooseNextDestination(datas);
            if (isWalking)
            {
                currentPathIndex = 0;

                datas.lastPoint.OnPointDestroyed -= UnsetVisitor;
                datas.currentPoint.OnPointDestroyed += UnsetVisitor;
            }
        }
    }

    private void SetNextDestination(int pathIndex)
    {
        Debug.Log(pathIndex);
        Debug.Log(datas.wantedTargets.Count);

        if(pathIndex == datas.wantedTargets.Count-1)
        {
            datas.agent.autoBraking = true;
        }
        else if(datas.agent.autoBraking)
        {
            datas.agent.autoBraking = false;
        }
        datas.agent.destination = datas.wantedTargets[pathIndex];
    }

    private void ReachDestination()
    {
        Debug.Log("Reach destination");

        if (currentPathIndex < datas.wantedTargets.Count)
        {
            currentPathIndex++;
            SetNextDestination(currentPathIndex);
        }
        else
        {
            isWalking = false;
            if (datas.currentPoint == spawnPoint)
            {
                VisitorManager.RemoveVisitor(this);
            }
            else
            {
                VisitorManager.MakeVisitorWait(Time.deltaTime, AskToWalk);
            }
        }
    }
    #endregion

    #region Interactions
    public bool CanInteract => !doesAlreadyInteract;

    public void AskInteraction(InterestPoint point)
    {
        if(point.IsUsable() && CanInteract && visitorType.InterestedActivities.Contains(point.PointType))
        {
            StartInteraction();
            point.MakeVisitorInteract(this);
        }
    }

    public void StartInteraction()
    {
        if(!doesAlreadyInteract)
        {
            doesAlreadyInteract = true;
            datas.agent.isStopped = true;
            datas.agent.enabled = false;
        }
    }

    public void EndInteraction()
    {
        doesAlreadyInteract = false;
        datas.agent.enabled = true;
        datas.agent.isStopped = false;
    }
    #endregion

    private void OnDisable()
    {
        if (datas.currentPoint != null)
        {
            datas.currentPoint.OnPointDestroyed -= UnsetVisitor;
        }
    }

    private void OnDestroy()
    {
        if(datas.currentPoint != null)
        {
            datas.currentPoint.OnPointDestroyed -= UnsetVisitor;
        }
    }

    public void SetObstacle(bool isObstacle)
    {
        datas.agent.enabled = !isObstacle;
        datas.obstacle.enabled = isObstacle;
    }
}
