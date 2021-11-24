using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_ActionTestUn : BT_TreeAction
{
    [SerializeField] private int isUsable;

    public override bool IsUsable()
    {
        return isUsable < 3;
    }

    public override void OnPlayAction()
    {
        isUsable++;
        Debug.Log(gameObject);
        EndAction();
    }

    public override void OnEndAction()
    {

    }
}
