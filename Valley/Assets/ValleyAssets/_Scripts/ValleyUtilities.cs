using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleyUtilities
{
    public static Vector2 GetVectorPoint2D(Vector3 point1, Vector3 point2, float t)
    {
        float pointx = point1.x * (1 - t) + point2.x * t;
        float pointz = point1.z * (1 - t) + point2.z * t;

        Vector2 pointPlaced = new Vector2(pointx, pointz);

        return pointPlaced;
    }

    public static Vector3 GetVectorPoint3D(Vector3 point1, Vector3 point2, float t)
    {
        float pointx = point1.x * (1 - t) + point2.x * t;
        float pointy = point1.y * (1 - t) + point2.y * t;
        float pointz = point1.z * (1 - t) + point2.z * t;

        Vector3 pointPlaced = new Vector3(pointx, pointy, pointz);

        return pointPlaced;
    }
}
