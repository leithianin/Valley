using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAttractivity : MonoBehaviour
{
    private List<Image> starsList = new List<Image>();

    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            starsList.Add(transform.GetChild(i).GetComponent<Image>());
        }

        ValleyManager.OnActravityUpdate += UpdateStarsImage;
    }

    public void UpdateStarsImage(float f)
    {
        for(int i = 0; i < starsList.Count; i++)
        {
            if (f > 1f)
            {
                starsList[i].fillAmount = 1f;
                f -= 1f;
            }
            else if(f <= 0f)
            {
                starsList[i].fillAmount = 0f;
            }
            else
            {
                starsList[i].fillAmount = f;
                f -= f;
            }
        }
    }

    public void OnDisable()
    {
        ValleyManager.OnActravityUpdate -= UpdateStarsImage;
    }
}
