using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IP_TreeLandmark : InterestPoint
{
    public override LandMarkType LandmarkType => LandMarkType.Tree;

    public override bool IsUsable()
    {
        return true;
    }

    protected override void OnEndInteraction(VisitorAgentBehave visitor)
    {
        
    }

    protected override void OnVisitorInteract(VisitorAgentBehave visitor)
    {
        EndInteraction(visitor);
    }
}
    
