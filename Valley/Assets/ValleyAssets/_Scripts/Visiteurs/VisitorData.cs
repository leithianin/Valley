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
    public float noiseMade = 2;

    public List<Vector3> wantedTargets = new List<Vector3>();

    public void SetDestination(PathFragmentData newPath)
    {
        lastPoint = currentPoint;
        currentPoint = newPath.lastPoint;
        wantedTargets = newPath.path;
    }
}
