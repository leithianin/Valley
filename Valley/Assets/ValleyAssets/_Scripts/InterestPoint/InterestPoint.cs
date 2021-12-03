using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterestPoint : MonoBehaviour
{
    [SerializeField] private float satisfactionGiven;

    public virtual InterestPointType PointType => InterestPointType.Rest;

    public virtual LandMarkType LandmarkType => LandMarkType.None;

    public abstract bool IsUsable();

    protected abstract void OnVisitorInteract(VisitorAgentBehave visitor);

    protected abstract void OnEndInteraction(VisitorAgentBehave visitor);

    public void MakeVisitorInteract(VisitorAgentBehave visitor)
    {
        visitor.AddSatisfaction(satisfactionGiven);
        OnVisitorInteract(visitor);
    }

    public void EndInteraction(VisitorAgentBehave visitor)
    {
        OnEndInteraction(visitor);
        visitor.EndInteraction();
    }
}
