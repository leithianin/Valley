using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Sequence : BT_TreeAction
{
    [SerializeField] private List<BT_TreeAction> treeActions;

    private int currentActionIndex = -1;

    public override bool IsUsable()
    {
        return treeActions.Count > 0 && treeActions[0].IsUsable();
    }

    public override void OnPlayAction()
    {
        currentActionIndex = -1;
        PlayNextAction();
    }

    public override void OnEndAction()
    {
        
    }

    private void PlayNextAction()
    {
        currentActionIndex++;
        if (currentActionIndex >= treeActions.Count)
        {
            EndAction();
        }
        else
        {
            if(treeActions[currentActionIndex].IsUsable())
            {
                treeActions[currentActionIndex].PlayAction(PlayNextAction);
            }
            else
            {
                EndAction();
            }
        }
    }
}
