using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DFB_DisplayText : MonoBehaviour
{
    [SerializeField] private string toDisplay;
    [SerializeField] private TextMeshProUGUI displayer;
    [SerializeField, Tooltip("If equal or inferior to 0, will be displayed until it is ask to not be display.")] private float displayTime;

    public void Play()
    {
        StopAllCoroutines();

        displayer.text = toDisplay;

        gameObject.SetActive(true);

        if(displayTime > 0)
        {
            StartCoroutine(DisplayTimer());
        }
    }

    public void Stop()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DisplayTimer()
    {
        yield return new WaitForSeconds(displayTime);
        Stop();
    }
}
