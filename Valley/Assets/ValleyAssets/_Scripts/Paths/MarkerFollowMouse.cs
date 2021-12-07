using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerFollowMouse : MonoBehaviour
{
    private ConstructionPreview currentConstruction;

    /// <summary>
    /// Set la Construction et utilise les Spawn/Despawn
    /// </summary>
    /// <param name="construction"></param>
    public void SetSelectedTool(ConstructionPreview construction)
    {
        if(construction != null)
        {
            gameObject.SetActive(true);

            if (construction != currentConstruction)
            {
                if (currentConstruction != null)
                {
                    currentConstruction.DespawnObject();
                }

                currentConstruction = Instantiate(construction, LandInteractions.GetHitPoint(), Quaternion.identity, transform);

                currentConstruction.transform.localPosition = Vector3.zero;

                currentConstruction.SpawnObject(LandInteractions.GetHitPoint());
            }
        }
        else
        {
            gameObject.SetActive(false);
        }

        ConstructionManager.ChooseConstruction(currentConstruction);
    }


    private void Update()
    {
        transform.position = LandInteractions.GetPreviewPosition();
        currentConstruction.CheckAvailability();
    }
}
