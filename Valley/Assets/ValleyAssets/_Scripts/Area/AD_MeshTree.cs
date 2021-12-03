using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AD_MeshTree : AreaDisplay
{
    [Serializable]
    private class ADData_StageTree
    {
        public float stageValue;
        public List<GameObject> trees;
    }

    [Tooltip("Mesh des arbres vivants dans l'odre LOD0, LOD1, LOD2")]
    public List<Mesh> meshAliveTrees = new List<Mesh>();
    public List<Material> materialAliveTrees = new List<Material>();
    [Tooltip("Mesh des arbres morts dans l'odre LOD0, LOD1, LOD2")]
    public List<Mesh> meshDeadTrees = new List<Mesh>();
    public List<Material> materialDeadTrees = new List<Material>();

    [SerializeField] List<ADData_StageTree> stagesValues;
    public override void SetNatureLevel(float soundLevel)
    {
        natureLevel = soundLevel;

        for (int i = 0; i < stagesValues.Count; i++)
        {
            if (soundLevel < stagesValues[i].stageValue)
            {
                for (int j = 0; j < stagesValues[i].trees.Count; j++)
                {
                    for(int k = 0; k < 3; k++)
                    {
                        stagesValues[i].trees[j].transform.GetChild(k).GetComponent<MeshFilter>().mesh = meshAliveTrees[k];
                        stagesValues[i].trees[j].transform.GetChild(k).GetComponent<MeshRenderer>().material = materialAliveTrees[k];
                    }
                }
            }
            else
            {
                for (int j = 0; j < stagesValues[i].trees.Count; j++)
                {              
                    for (int k = 0; k < 3; k++)
                    {
                        stagesValues[i].trees[j].transform.GetChild(k).GetComponent<MeshFilter>().mesh = meshDeadTrees[k];
                        stagesValues[i].trees[j].transform.GetChild(k).GetComponent<MeshRenderer>().material = materialDeadTrees[k];
                    }
                }
            }
        }
    }
}
