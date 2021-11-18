using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BT_TreeAction : MonoBehaviour
{
    protected Action endCallback;

    public abstract bool IsUsable();

    public abstract void OnPlayAction();

    public abstract void OnEndAction();

    public void PlayAction(Action callback)
    {
        endCallback = callback;
        OnPlayAction();
    }

    public void EndAction()
    {
        OnEndAction();
        endCallback?.Invoke();
    }
}
