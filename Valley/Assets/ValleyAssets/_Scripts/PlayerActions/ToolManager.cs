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
    public GameObject lineRendererObject;

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

    public static void CreateLink(PathPoint firstObjectToLink)
    {
        GameObject lineRendererChild = Instantiate(instance.lineRendererObject, firstObjectToLink.transform.position, Quaternion.identity, firstObjectToLink.transform);

        LineRenderer ln = lineRendererChild.AddComponent<LineRenderer>();
        Valley_PathManager.GetCurrentPath().lineRenderer = ln;
        firstObjectToLink.GetLink.line = ln;
        firstObjectToLink.GetLink.FirstPoint();
    }

    public static void AddLink(PathPoint nextObjectToLink, PathPoint firstObjectToLink)
    {
        firstObjectToLink.GetLink.AddPoint(nextObjectToLink.gameObject);
    }

    public static void EndLink(PathPoint firstObjectToLink)
    {
        firstObjectToLink.GetLink.EndPoint(firstObjectToLink.gameObject);
    }

    public static void ResetLink(PathPoint firstObjectToLink)
    {
        firstObjectToLink.GetLink.ResetPoint();
    }
}
