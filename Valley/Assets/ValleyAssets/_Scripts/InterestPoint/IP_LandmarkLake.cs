using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IP_LandmarkLake : InterestPoint
{
    public override LandMarkType LandmarkType => LandMarkType.Lake;

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
