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
            Debug.Log("Je selectionne");
            isSelected = true;
            PlayOnSelected?.Invoke();
        }
        else
        {
            Debug.Log("Je Deselectionne");
            isSelected = false;
            PlayOnDeselected?.Invoke();
        }
    }

    public void OnReset()
    {
        isSelected = false;
        PlayOnDeselected?.Invoke();
    }
}
