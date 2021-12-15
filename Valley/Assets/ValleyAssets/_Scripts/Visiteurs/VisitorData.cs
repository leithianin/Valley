using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class VisitorData
{
    // The Navmesh Agent of the visitor.
    public NavMeshAgent agent;
    // The Navmesh Obstacle of the visitor.
    public NavMeshObstacle obstacle;
    // The last point reached by the visitor.
    [HideInInspector] public PathPoint lastPoint;
    // The point the visitor is currently reaching.
    [HideInInspector] public PathPoint currentPoint;
    // The path on which the visitor is.
    [HideInInspector] public Valley_PathData path;
    // The amount of noise made by the visitor.
    [HideInInspector] public float noiseMade = 2;
    // The current satisfaction of the visitor.
    public float satisfactionScore;
    // The current objective of the visitor.
    public LandMarkType objective;

    // A list of all the position the visitor will reach before reaching its destination.
    [HideInInspector] public List<Vector3> wantedTargets = new List<Vector3>();

    /// <summary>
    /// Set a new path to follow for the visitor.
    /// </summary>
    /// <param name="newPath">The path the visitor will follow.</param>
    public void SetDestination(PathFragmentData newPath)
    {
        lastPoint = newPath.startPoint;
        currentPoint = newPath.endPoint;
        wantedTargets = newPath.path;
    }
    /// <summary>
    /// Change the amount of satisfaction of the visitor.
    /// </summary>
    /// <param name="toAdd">The amount to add (or remove if negative).</param>
    public void AddSatisfaction(float toAdd)
    {
        satisfactionScore += toAdd;
        satisfactionScore = Mathf.Clamp(satisfactionScore, 0, 1);
    }
}
