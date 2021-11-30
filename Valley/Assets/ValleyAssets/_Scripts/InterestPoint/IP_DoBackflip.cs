using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IP_DoBackflip : InterestPoint
{
    private VisitorAgentBehave currentVisitor;

    public override bool IsUsable()
    {
        return currentVisitor == null;
    }

    protected override void OnEndInteraction(VisitorAgentBehave visitor)
    {
        currentVisitor = null;
    }

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
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        EndInteraction(currentVisitor);
    }
}
