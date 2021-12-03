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
    public float satisfactionScore;

    public List<Vector3> wantedTargets = new List<Vector3>();

    public void SetDestination(PathFragmentData newPath)
    {
        lastPoint = newPath.startPoint;
        currentPoint = newPath.endPoint;
        wantedTargets = newPath.path;
    }

    public void AddSatisfaction(float toAdd)
    {
        satisfactionScore += toAdd;
        satisfactionScore = Mathf.Clamp(satisfactionScore, 0, 1);
    }
}
