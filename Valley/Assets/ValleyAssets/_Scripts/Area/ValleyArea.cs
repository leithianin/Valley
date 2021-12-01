using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ValleyArea
{
    public string nom;
    public List<Transform> bordersTransform;
    public List<Vector2> borders;

    public List<VisitorAgentBehave> visitorInZone;

    public List<Vector2> GetBorders => borders;

    [SerializeField] private List<AreaDisplay> displays;

    [SerializeField] private List<InterestPoint> interestPoints;

    public List<AreaDisplay> Displays => displays;

    public List<InterestPoint> InterestPoints => interestPoints;

    public bool ContainsInterestType(InterestPointType interestType)
    {
        for(int i = 0; i < interestPoints.Count; i++)
        {
            if(interestPoints[i].PointType == interestType)
            {
                return true;
            }
        }
        return false;
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
