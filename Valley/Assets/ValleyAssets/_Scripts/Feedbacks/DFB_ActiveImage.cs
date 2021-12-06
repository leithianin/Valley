using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DFB_ActiveImage : MonoBehaviour
{
    private Image imageToActivate;

    private void Start()
    {
        imageToActivate = GetComponent<Image>();
    }

    public void ActivateImage()
    {
        imageToActivate.enabled = true;
    }

    public void DesactivateImage()
    {
            imageToActivate.enabled = false;
    }
}
