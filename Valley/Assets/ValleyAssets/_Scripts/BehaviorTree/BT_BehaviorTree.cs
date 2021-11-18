using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_BehaviorTree : MonoBehaviour
{
    [SerializeField] private List<BT_TreeAction> actions;

    private void OnEnable()
    {
        for(int i = 0; i < actions.Count; i++)
        {
            if(actions[i].IsUsable())
            {
                actions[i].PlayAction(EndAction);
                break;
            }
        }
    }

    private void EndAction()
    {
        Debug.Log("End");
    }
}
