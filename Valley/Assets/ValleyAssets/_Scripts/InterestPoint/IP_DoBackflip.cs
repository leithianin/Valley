using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IP_DoBackflip : InterestPoint
{
    /// <summary>
    /// The type of InterestPoint it is.
    /// </summary>
    public override InterestPointType PointType => InterestPointType.ForHiker;

    [SerializeField] private VisitorAgentBehave currentVisitor;

    /// <summary>
    /// Check if a visitor is already using the InterestPoint.
    /// </summary>
    /// <returns>Return true if no visitor is using the InterestPoint.</returns>
    public override bool IsUsable()
    {
        return currentVisitor == null;
    }

    /// <summary>
    /// When the interaction end, we remove the current visitor.
    /// </summary>
    /// <param name="visitor">The visitor that end the interaction.</param>
    protected override void OnEndInteraction(VisitorAgentBehave visitor)
    {
        currentVisitor = null;
    }

    /// <summary>
    /// When a visitor interact, we store it and start a coroutine to make an action.
    /// </summary>
    /// <param name="visitor"></param>
    protected override void OnVisitorInteract(VisitorAgentBehave visitor)
    {
        currentVisitor = visitor;
        StartCoroutine(Backflip());
    }

    IEnumerator Backflip()
    {
        Transform visitorObject = currentVisitor.gameObject.transform;
        for (int i = 0; i < 100; i++)
        {
            if(i < 50)
            {
                visitorObject.position = new Vector3(visitorObject.position.x, visitorObject.position.y + .1f, visitorObject.position.z);
            }
            else
            {
                visitorObject.position = new Vector3(visitorObject.position.x, visitorObject.position.y - .1f, visitorObject.position.z);
            }
            yield return new WaitForSeconds(.2f);
        }
        EndInteraction(currentVisitor);
    }
}
