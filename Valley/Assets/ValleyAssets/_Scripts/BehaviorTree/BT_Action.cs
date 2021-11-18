using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BT_Action
{
    protected Action endCallback;

    public abstract void DoAction(Action callback);

    public abstract void EndAction(bool endState);

    public abstract bool CanDoAction();
}
