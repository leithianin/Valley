using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerFollowMouse : MonoBehaviour
{
    private Construction currentConstruction;

    /// <summary>
    /// Set la Construction et utilise les Spawn/Despawn
    /// </summary>
    /// <param name="construction"></param>
    public void SetSelectedTool(Construction construction)
    {
        if(construction != null)
        {
            enabled = true;

            if (construction != currentConstruction)
            {
                currentConstruction.DespawnObject();

                Instantiate(construction, LandInteractions.GetHitPoint(), Quaternion.identity, transform);

                construction.SpawnObject(LandInteractions.GetHitPoint());
            }
        }
        else
        {
            enabled = false;
        }
    }


    private void Update()
    {
        transform.position = LandInteractions.GetHitPoint();
    }
}
