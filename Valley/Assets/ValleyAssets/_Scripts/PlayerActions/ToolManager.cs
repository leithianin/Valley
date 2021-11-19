using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SelectedTools { None, PathTool }
public class ToolManager : MonoBehaviour
{
    private static ToolManager instance;
    public static SelectedTools _selectedTool;
    public static EventSystemKeepSelected eventSystemKeepSelectedScript;

    public GameObject markerPlaceHolder;

    private void Awake()
    {
        instance = this;
        _selectedTool = SelectedTools.None;
    }

    private void Start()
    {
        eventSystemKeepSelectedScript = EventSystem.current.gameObject.GetComponent<EventSystemKeepSelected>();
    }

    public void OnSelectPathTool(Button button)
    {
        if(_selectedTool == SelectedTools.PathTool)
        {
            markerPlaceHolder.SetActive(false);
            eventSystemKeepSelectedScript.RemoveLastSelected();
            _selectedTool = SelectedTools.None;
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            ActivePathTool();
        }    
    }

    public static void ActivePathTool()
    {
        instance.markerPlaceHolder.SetActive(true);
        eventSystemKeepSelectedScript.KeepSelected();
        _selectedTool = SelectedTools.PathTool;
    }

    public static EventSystemKeepSelected GetEventSystemKeepSelected()
    {
        return eventSystemKeepSelectedScript;
    }

    public static void CreateLink(GameObject firstObjectToLink)
    {
        LineRenderer ln = firstObjectToLink.AddComponent<LineRenderer>();
        firstObjectToLink.GetComponent<VisibleLink>().line = ln;
        firstObjectToLink.GetComponent<VisibleLink>().FirstPoint();
    }

    public static void AddLink(GameObject nextObjectToLink, GameObject firstObjectToLink)
    {
        firstObjectToLink.GetComponent<VisibleLink>().AddPoint(nextObjectToLink);
    }

    public static void EndLink(GameObject firstObjectToLink)
    {
        firstObjectToLink.GetComponent<VisibleLink>().EndPoint(firstObjectToLink);
    }
}
