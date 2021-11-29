using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_NumerousTree : AreaDisplay
{
    [Serializable]
    private class ADData_StageTree
    {
        public float stageValue;
        public List<GameObject> trees;
    }

    [SerializeField] List<ADData_StageTree> stagesValues;
    public override void SetNatureLevel(int level)
    {
        natureLevel = level;
        for(int i = 0; i < stagesValues.Count; i++)
        {
            if(stagesValues[i].stageValue > level)
            {
                for(int j = 0; j < stagesValues[i].trees.Count; j++)
                {
                    stagesValues[i].trees[j].gameObject.SetActive(true);
                }
            }
            else
            {
                for (int j = 0; j < stagesValues[i].trees.Count; j++)
                {
                    stagesValues[i].trees[j].gameObject.SetActive(false);
                }
            }
        }
    }


}
