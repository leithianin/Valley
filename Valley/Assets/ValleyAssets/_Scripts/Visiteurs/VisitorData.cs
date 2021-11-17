using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorData
{
    public NavMeshAgent agent;
    public PathPoint lastPoint;
    public PathPoint currentPoint;
    public Valley_PathData path;

    public void SetDestination(PathPoint newPoint)
    {
        lastPoint = currentPoint;
        currentPoint = newPoint;
        agent.destination = newPoint.Position;
    }
}
