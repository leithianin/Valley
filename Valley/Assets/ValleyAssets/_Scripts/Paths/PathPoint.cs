using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    public Vector3 Position => transform.position;

    [SerializeField]
    private List<PathPoint> linkedPoints;

    public PathPoint GetRandomPoint()
    {
        return linkedPoints[Random.Range(0, linkedPoints.Count)];
    }

    public PathPoint GetNextPathPoint(PathPoint lastPoint, Valley_PathData path)
    {
        PathPoint toReturn = lastPoint;

        List<PathPoint> usablePoints = new List<PathPoint>();
        for(int i = 0; i < linkedPoints.Count; i++)
        {
            if(path.ContainsPoint(linkedPoints[i]))
            {
                usablePoints.Add(linkedPoints[i]);
            }
        }

        if (usablePoints.Count > 1 || lastPoint == null)
        {
            while (toReturn == lastPoint)
            {
                PathPoint newPoint = GetRandomPoint();
                if (path.ContainsPoint(newPoint))
                {
                    toReturn = newPoint;
                }
            }
        }
        return toReturn;
    }
}
