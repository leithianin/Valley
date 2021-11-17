using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valley_PathData
{
    public List<PathPoint> pathPoints = new List<PathPoint>();

    public bool ContainsPoint(PathPoint toCheck)
    {
        return pathPoints.Contains(toCheck);
    }

}
