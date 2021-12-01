using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_NumerousTree : AreaDisplay
{
    [Serializable]
    private class ADData_StageTree
    {
        [Tooltip("ça correspond à telle valeur")]
        public float stageValue;
        public List<GameObject> trees;
    }

    [SerializeField] List<ADData_StageTree> stagesValues;
    public override void SetNatureLevel(float soundLevel)
    {
        natureLevel = soundLevel;
        for(int i = 0; i < stagesValues.Count; i++)
        {
            if(soundLevel < stagesValues[i].stageValue)
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
