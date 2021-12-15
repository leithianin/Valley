using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class VisitorAgentBehave : MonoBehaviour
{
    [Header("Dev datas")]
    // The datas of the visitor.
    [SerializeField] private VisitorData datas = new VisitorData();

    // The PathPoint where the visitor spawned.
    private PathPoint spawnPoint;
    // Is the visitor walking ?
    private bool isWalking;
    // The area in which the visitor is.
    [HideInInspector] public ValleyArea currentArea;
    // Is the visitor interacting with something ?
    private bool doesAlreadyInteract;

    private int currentPathIndex;
    // The scriptable of the visitor.
    private VisitorScriptable visitorType;

    // The animation handler. Handle the display of the visitor.
    [SerializeField] private AnimationHandler currentDisplay;

    [Header("Feedbacks")]
    [Tooltip("Feedback to play when the visitor gains satisfaction.")] public UnityEvent PlayOnSatisfactionAdd;
    [Tooltip("Feedback to play when the visitor loses satisfaction.")] public UnityEvent PlayOnSatisfactionSubstract;

    /// <summary>
    /// The position of the visitor in 2D (X and Z position).
    /// </summary>
    public Vector2 GetPosition => new Vector2(transform.position.x, transform.position.z);
    /// <summary>
    /// The Navmesh Agent of the visitor.
    /// </summary>
    public NavMeshAgent Agent => datas.agent;
    /// <summary>
    /// The datas of the visitor.
    /// </summary>
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

    /// <summary>
    /// Search a path for the visitor and initialize every datas according to the chosen path. If a path is found, activate the GameObject.
    /// </summary>
    /// <param name="nSpawnPoint">The PathPoint where the visitor spawn.</param>
    /// <param name="spawnPosition">The position of the visitor at the spawn.</param>
    /// <param name="nType">The type of visitor it is.</param>
    public void SetVisitor(PathPoint nSpawnPoint, Vector3 spawnPosition, VisitorScriptable nType)
    {
        Valley_PathData wantedPath = VisitorManager.ChoosePath(nSpawnPoint, nType.Objectives, nType.InterestedActivities, out datas.objective);

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

            transform.position = spawnPosition;

            gameObject.SetActive(true);

            if (currentDisplay != null)
            {
                Destroy(currentDisplay.gameObject);
            }

            currentDisplay = Instantiate(visitorType.Display, transform);

            AskToWalk();

            SetNextDestination(currentPathIndex);

            datas.currentPoint.OnPointDestroyed += AskDeleteVisitor;
        }
    }

    /// <summary>
    /// Remove a visitor from the valley.
    /// </summary>
    public void AskDeleteVisitor()
    {
        VisitorManager.DeleteVisitor(this);
    }
    /// <summary>
    /// Disable the visitor.
    /// </summary>
    public void UnsetVisitor()
    {
        datas.currentPoint = null;

        currentArea.RemoveVisitor(this);

        gameObject.SetActive(false);
    }

    #region Deplacement
    /// <summary>
    /// Try to find the next destination and move to it.
    /// </summary>
    private void AskToWalk()
    {
        if (enabled)
        {
            isWalking = VisitorManager.ChooseNextDestination(datas);
            if (isWalking)
            {
                currentDisplay.PlayBodyAnim(BodyAnimationType.Walk);
                currentPathIndex = 0;

                datas.lastPoint.OnPointDestroyed -= AskDeleteVisitor;
                datas.currentPoint.OnPointDestroyed += AskDeleteVisitor;
            }
        }
    }

    /// <summary>
    /// Set the next destination of the visitor.
    /// </summary>
    /// <param name="pathIndex">The index of the wanted destination on the current path.</param>
    private void SetNextDestination(int pathIndex)
    {
        Vector3 targetPosition = datas.wantedTargets[pathIndex];

        if (Vector3.Distance(transform.position, targetPosition) <= 2f)
        {
            ReachDestination();
        }
        else
        {
            Vector3 randomPosition = datas.wantedTargets[pathIndex] + UnityEngine.Random.insideUnitSphere * 2f;

            datas.agent.destination = randomPosition;
        }
    }

    /// <summary>
    /// Called when the visitor reach a destination. Set a new destination or make the visitor quit the valley if it reach its SpawnPoint.
    /// </summary>
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
                VisitorManager.DeleteVisitor(this);
            }
            else
            {
                currentDisplay.PlayBodyAnim(BodyAnimationType.Idle);
                VisitorManager.MakeVisitorWait(Time.deltaTime, AskToWalk);
            }
        }
    }
    #endregion

    #region Interactions
    /// <summary>
    /// Can the visitor interact with something ?
    /// </summary>
    public bool CanInteract => !doesAlreadyInteract;
    /// <summary>
    /// Check if the visitor can interact with a specified InterestPoint. If so, the visitor interact with it.
    /// </summary>
    /// <param name="point">The InterestPoint we want the visitor to interact with.</param>
    public void AskInteraction(InterestPoint point)
    {
        if(point.IsUsable() && CanInteract && ((visitorType.InterestedActivities.Contains(point.PointType) || (datas.objective == point.LandmarkType && point.LandmarkType != LandMarkType.None))))
        {
            StartInteraction();
            point.MakeVisitorInteract(this);
        }
    }
    /// <summary>
    /// Start a visitor's interaction.
    /// </summary>
    public void StartInteraction()
    {
        if(!doesAlreadyInteract)
        {
            doesAlreadyInteract = true;
            //datas.agent.isStopped = true;
            //datas.agent.enabled = false;
        }
    }
    /// <summary>
    /// End the interaction of the visitor.
    /// </summary>
    public void EndInteraction()
    {
        doesAlreadyInteract = false;
        //datas.agent.enabled = true;
        //datas.agent.isStopped = false;
    }
    #endregion
    /// <summary>
    /// Called when the visitor see an other visitor.
    /// </summary>
    public void CrossVisitor()
    {
        AddSatisfaction(visitorType.SatisfactionNextToOther, true);
    }
    /// <summary>
    /// Give a specified amount of satisfaction to the visitor and play a feedback.
    /// </summary>
    /// <param name="toAdd">The mount of satisfaction to give (or remove if negative).</param>
    /// <param name="playFeedback">If true, will play the Satisfaction feedback of the visitor.</param>
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
            datas.currentPoint.OnPointDestroyed -= AskDeleteVisitor;
        }
    }

    private void OnDestroy()
    {
        if(datas.currentPoint != null)
        {
            datas.currentPoint.OnPointDestroyed -= AskDeleteVisitor;
        }
    }

    public void SetObstacle(bool isObstacle)
    {
        datas.agent.enabled = !isObstacle;
        datas.obstacle.enabled = isObstacle;
    }
}
