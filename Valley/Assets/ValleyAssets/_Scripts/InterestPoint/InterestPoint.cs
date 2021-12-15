using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterestPoint : MonoBehaviour
{
    // To see how to create an InterestPoint, check the IP_DoBackflip script.

    // Satisfaction given when a visitor interact with the interest point.
    [SerializeField] private float satisfactionGiven;

    /// <summary>
    /// The type of InterestPoint it is. Must be override for every Interest Point.
    /// </summary>
    public virtual InterestPointType PointType => InterestPointType.None;
    
    /// <summary>
    /// The type of Landmark it is. Must be override for every Landmark.
    /// </summary>
    public virtual LandMarkType LandmarkType => LandMarkType.None;

    /// <summary>
    /// Chec if the Interest Point can be used by a visitor.
    /// </summary>
    /// <returns>Return true if the visitor can interact with it.</returns>
    public abstract bool IsUsable();

    /// <summary>
    /// Do specific action when the visitor interact with the InterestPoint.
    /// </summary>
    /// <param name="visitor">The visitor thay interacted with the InterestPoint.</param>
    protected abstract void OnVisitorInteract(VisitorAgentBehave visitor);

    /// <summary>
    /// Do specific actions when the visitor interaction end.
    /// </summary>
    /// <param name="visitor">The visitor that end the interaction.</param>
    protected abstract void OnEndInteraction(VisitorAgentBehave visitor);

    /// <summary>
    /// Make a visitor interact with the InterestPoint.
    /// </summary>
    /// <param name="visitor">The visitor that is interacting.</param>
    public void MakeVisitorInteract(VisitorAgentBehave visitor)
    {
        visitor.AddSatisfaction(satisfactionGiven, true);
        OnVisitorInteract(visitor);
    }

    /// <summary>
    /// End the interaction with a visitor.
    /// </summary>
    /// <param name="visitor">The visitor that end the interaction.</param>
    public void EndInteraction(VisitorAgentBehave visitor)
    {
        OnEndInteraction(visitor);
        visitor.EndInteraction();
    }
}
