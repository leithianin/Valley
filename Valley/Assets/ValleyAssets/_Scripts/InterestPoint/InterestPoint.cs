using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterestPoint : MonoBehaviour
{
    public virtual InterestPointType PointType()
    {
        Debug.Log("Base");
        return InterestPointType.Rest;
    }
    public virtual LandMarkType LandmarkType => LandMarkType.None;

    public abstract bool IsUsable();

    protected abstract void OnVisitorInteract(VisitorAgentBehave visitor);

    protected abstract void OnEndInteraction(VisitorAgentBehave visitor);

    public void MakeVisitorInteract(VisitorAgentBehave visitor)
    {
        OnVisitorInteract(visitor);
    }

    public void EndInteraction(VisitorAgentBehave visitor)
    {
        OnEndInteraction(visitor);
        visitor.EndInteraction();
    }
}
