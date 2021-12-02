using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPointPreview : ConstructionPreview
{
    [SerializeField] private float maxDistanceFromLastPoint;

    protected override bool OnCanPlaceObject(Vector3 position)
    {
        if (VisibleLinkManager.GetLineLength() <= maxDistanceFromLastPoint)
        {
            return true;
        }
        return false;
    }
}
