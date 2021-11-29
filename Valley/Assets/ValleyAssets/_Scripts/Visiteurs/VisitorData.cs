using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class VisitorData
{
    public NavMeshAgent agent;
    public NavMeshObstacle obstacle;
    public PathPoint lastPoint;
    public PathPoint currentPoint;
    public Valley_PathData path;
    public float noiseMade = 20;

    public void SetDestination(PathPoint newPoint)
    {
        lastPoint = currentPoint;
        currentPoint = newPoint;
        agent.destination = newPoint.Position;
    }
}
