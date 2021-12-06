using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonsFeedbacks : MonoBehaviour
{
    public UnityEvent PlayOnPointerEnter;
    public UnityEvent PlayOnPointerExit;
    public UnityEvent PlayOnSelected;
    public UnityEvent PlayOnDeselected;

    private bool isSelected = false;

    public void PointerEnter()
    {
        if (!isSelected) { PlayOnPointerEnter?.Invoke(); }
    }

    public void PointerExit()
    {
        if (!isSelected) { PlayOnPointerExit?.Invoke(); }
    }

    public void OnSelected()
    {
        if (!isSelected)
        {
            isSelected = true;
            PlayOnSelected?.Invoke();
        }
        else
        {
            isSelected = false;
            PlayOnDeselected?.Invoke();
        }
    }
}
