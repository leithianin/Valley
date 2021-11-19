using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemKeepSelected : MonoBehaviour
{
    private EventSystem eventSystem;
    private GameObject lastSelected = null;

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = GetComponent<EventSystem>();
    }

    public void KeepSelected()
    {
        if (eventSystem != null)
        {
            if (eventSystem.currentSelectedGameObject != null)
            {
                lastSelected = eventSystem.currentSelectedGameObject;
            }
            else
            {
                eventSystem.SetSelectedGameObject(lastSelected);
            }
        }
    }

    public void RemoveLastSelected()
    {
        lastSelected = null;
    }
}
