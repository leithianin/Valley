using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IP_DoBackflip : InterestPoint
{
    public override InterestPointType PointType()
    {
        return InterestPointType.Photo;
    }

    [SerializeField] private VisitorAgentBehave currentVisitor;

    public override bool IsUsable()
    {
        return true;// currentVisitor == null;
    }

    protected override void OnEndInteraction(VisitorAgentBehave visitor)
    {
        currentVisitor = null;
    }

    protected override void OnVisitorInteract(VisitorAgentBehave visitor)
    {
        currentVisitor = visitor;
        Debug.Log("VisitorInteract");
        EndInteraction(currentVisitor);
        //StartCoroutine(Backflip());
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
