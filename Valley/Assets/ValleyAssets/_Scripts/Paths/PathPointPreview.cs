using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathPointPreview : ConstructionPreview
{
    [SerializeField] private float maxDistanceFromLastPoint;

    [SerializeField] private UnityEvent PlayOnDistanceTooFar;

    protected override void OnAskToPlace(Vector3 position)
    {
        if (!IsInDistance(position))
        {
            PlayOnDistanceTooFar?.Invoke();
        }
    }

    protected override bool OnCanPlaceObject(Vector3 position)
    {
        if (IsInDistance(position))
        {
            return true;
        }
        return false;
    }

    public bool IsInDistance(Vector3 position)
    {
        if (VisibleLinkManager.GetLineLength() <= maxDistanceFromLastPoint)
        {
            return true;
        }
        return false;
    }
}
