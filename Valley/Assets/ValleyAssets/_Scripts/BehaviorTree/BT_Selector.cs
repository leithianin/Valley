using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Selector : BT_TreeAction
{
    [SerializeField] private List<BT_TreeAction> treeActions;

    public override bool IsUsable()
    {
        for(int i = 0; i < treeActions.Count; i++)
        {
            if(GetUsableAction() != null)
            {
                return true;
            }
        }
        return false;
    }

    public override void OnPlayAction()
    {
        GetUsableAction().PlayAction(EndAction);
    }

    public override void OnEndAction()
    {

    }

    private BT_TreeAction GetUsableAction()
    {
        for (int i = 0; i < treeActions.Count; i++)
        {
            if (treeActions[i].IsUsable())
            {
                return treeActions[i];
            }
        }
        return null;
    }
}
