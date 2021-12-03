using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Visitor", menuName = "Visitor/Create Visitor")]
public class VisitorScriptable : ScriptableObject
{
    [SerializeField] private float speed;
    [SerializeField] private float soundProduced;
    [SerializeField] private List<LandMarkType> objectives;
    [SerializeField] private List<InterestPointType> interestedActivities;

    public float Speed => speed;
    public float SoundProduced => soundProduced;
    public List<LandMarkType> Objectives => objectives;
    public List<InterestPointType> InterestedActivities => interestedActivities;
}
