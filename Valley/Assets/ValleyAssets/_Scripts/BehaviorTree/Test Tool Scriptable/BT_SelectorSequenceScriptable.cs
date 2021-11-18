using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BT_ActionType
{
    Selector,
    Sequence,
    ActionTest,
}

[CreateAssetMenu(fileName = "Tree Selector", menuName = "Create Selector")]
public class BT_SelectorSequenceScriptable : ScriptableObject
{
    [SerializeField] private List<BT_ActionType> actions;
}
