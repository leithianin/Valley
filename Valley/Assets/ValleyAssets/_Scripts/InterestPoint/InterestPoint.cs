using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterestPoint : MonoBehaviour
{
    public abstract bool IsUsable();

    protected abstract void OnVisitorInteract(VisitorAgentBehave visitor);

    public void MakeVisitorInteract(VisitorAgentBehave visitor)
    {
        OnVisitorInteract(visitor);
    }
}
