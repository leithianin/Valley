using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tree Scriptable", menuName = "Create Tree")]
public class BT_BehaviorScriptable : ScriptableObject
{
    public List<BT_SelectorSequenceScriptable> datas;
}
