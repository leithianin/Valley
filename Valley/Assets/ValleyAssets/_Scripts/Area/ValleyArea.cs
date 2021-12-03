using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ValleyArea
{
    public string nom;
    [Tooltip("List of all the corners of the area.")]
    public List<Transform> bordersTransform;

    [HideInInspector]
    public List<Vector2> borders;

    [HideInInspector]
    public List<VisitorAgentBehave> visitorInZone;

    public List<Vector2> GetBorders => borders;

    [Tooltip("Every elements that will be modified depending on the nature level of the area.")]
    [SerializeField] private List<AreaDisplay> displays;

    [Tooltip("Every interest point in the zone that visitors can interact with.")]
    [SerializeField] private List<InterestPoint> interestPoints;

    public List<AreaDisplay> Displays => displays;

    public List<InterestPoint> InterestPoints => interestPoints;

    public bool ContainsLandmarkType(LandMarkType landmarkType)
    {
        for (int i = 0; i < interestPoints.Count; i++)
        {
            if (interestPoints[i].LandmarkType == landmarkType)
            {
                return true;
            }
        }
        return false;
    }

    public int GetNumberInterestType(InterestPointType interestType)
    {
        int toReturn = 0;
        for(int i = 0; i < interestPoints.Count; i++)
        {
            if(interestPoints[i].PointType() == interestType)
            {
                toReturn++;
            }
        }
        return toReturn;
    }

    public void SetSoundLevel(float soundLevel)
    {
        for(int i = 0; i < displays.Count; i++)
        {
            displays[i].SetNatureLevel(soundLevel);
        }
    }

    public void RemoveVisitor(VisitorAgentBehave toRemove)
    {
        visitorInZone.Remove(toRemove);
    }
}
