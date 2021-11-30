using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PathFragmentData
{
    public PathPoint lastPoint;
    public PathPoint startPoint;
    public List<Vector3> path;

    public PathFragmentData(PathPoint nLastPoint, PathPoint nStartPoint, List<Vector3> nPath)
    {
        lastPoint = nLastPoint;
        startPoint = nStartPoint;

        path = new List<Vector3>(nPath);

    }

    public List<Vector3> GetReversePath()
    {
        List<Vector3> toReturn = new List<Vector3>();

        for (int i = 0; i < path.Count; i++)
        {
            toReturn.Add(path[path.Count - (i + 1)]);
        }
        return toReturn;
    }
}
