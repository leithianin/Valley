using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IP_PlaceholderLandscape : InterestPoint
{
    public override InterestPointType PointType => InterestPointType.ForHiker;

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
