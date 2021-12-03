using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class VisitorAgentBehave : MonoBehaviour
{
    [Header("Dev datas")]
    [SerializeField] private VisitorData datas = new VisitorData();

    private PathPoint spawnPoint;
    private bool isWalking;

    [HideInInspector] public ValleyArea currentArea;

    private bool doesAlreadyInteract;

    private int currentPathIndex;

    private VisitorScriptable visitorType;

    [SerializeField] private AnimationHandler currentDisplay;

    [Header("Feedbacks")]
    public UnityEvent PlayOnSatisfactionAdd;
    public UnityEvent PlayOnSatisfactionSubstract;

    public Vector2 GetPosition => new Vector2(transform.position.x, transform.position.z);

    public NavMeshAgent Agent => datas.agent;

    public VisitorData Data => datas;


    // Update is called once per frame
    void Update()
    {
        AddSatisfaction((visitorType.SatisfactionUpdate / 10) * Time.deltaTime, false);

        if (datas.agent.enabled && !datas.agent.pathPending && isWalking)
        {
            if (datas.agent.remainingDistance <= datas.agent.stoppingDistance)
            {
                //if (!datas.agent.hasPath || datas.agent.velocity.sqrMagnitude == 0f) //CODE REVIEW : Voir pour simplifier les "if" et les rassembler
                {
                    ReachDestination();
                }
            }
        }
    }

    public void SetVisitor(PathPoint nSpawnPoint, VisitorScriptable nType)
    {
        Valley_PathData wantedPath = VisitorManager.ChoosePath(nSpawnPoint, nType.Objectives, nType.InterestedActivities);

        if (wantedPath != null)
        {
            visitorType = nType;

            datas.agent.speed = visitorType.Speed;
            datas.noiseMade = visitorType.SoundProduced;
            datas.satisfactionScore = 0;

            spawnPoint = nSpawnPoint;

            datas.path = wantedPath;
            datas.lastPoint = nSpawnPoint;
            datas.currentPoint = nSpawnPoint;

            transform.position = nSpawnPoint.Position + UnityEngine.Random.insideUnitSphere * 8f;

            gameObject.SetActive(true);

            currentDisplay = Instantiate(visitorType.Display, transform);

            AskToWalk();

            SetNextDestination(currentPathIndex);

            datas.currentPoint.OnPointDestroyed += UnsetVisitor;
        }
    }

    public void UnsetVisitor()
    {
        datas.currentPoint = null;

        currentArea.RemoveVisitor(this);

        Destroy(currentDisplay.gameObject);

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
        Vector3 randomPosition = datas.wantedTargets[pathIndex] + UnityEngine.Random.insideUnitSphere * 2f;
        datas.agent.destination = randomPosition;
    }

    private void ReachDestination()
    {
        currentPathIndex++;
        if (currentPathIndex < datas.wantedTargets.Count)
        {
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
            //datas.agent.isStopped = true;
            //datas.agent.enabled = false;
        }
    }

    public void EndInteraction()
    {
        doesAlreadyInteract = false;
        //datas.agent.enabled = true;
        //datas.agent.isStopped = false;
    }
    #endregion

    public void CrossVisitor()
    {
        AddSatisfaction(visitorType.SatisfactionNextToOther, true);
    }

    public void AddSatisfaction(float toAdd, bool playFeedback)
    {
        datas.AddSatisfaction(toAdd);
        if (playFeedback)
        {
            if (toAdd > 0)
            {
                PlayOnSatisfactionAdd?.Invoke();
            }
            else if (toAdd < 0)
            {
                PlayOnSatisfactionSubstract?.Invoke();
            }
        }
    }

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
