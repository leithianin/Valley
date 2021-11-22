using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BT_TreeAction : MonoBehaviour
{
    protected Action endCallback;

    /// <summary>
    /// Vérifie si l'action est faisable ou non.
    /// </summary>
    /// <returns></returns>
    public abstract bool IsUsable();

    /// <summary>
    /// A override avec le code qui doit être éxécuté au lancement de l'action.
    /// Doit appeler "EndAction".
    /// </summary>
    public abstract void OnPlayAction();

    /// <summary>
    /// A override avec le code qui doit être éxécuté à la fin de l'action.
    /// </summary>
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
