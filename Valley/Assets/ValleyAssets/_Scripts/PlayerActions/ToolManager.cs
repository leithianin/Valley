using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;

public enum SelectedTools { None, PathTool }
public class ToolManager : MonoBehaviour
{
    private static ToolManager instance;
    public static SelectedTools _selectedTool;
    public static EventSystemKeepSelected eventSystemKeepSelectedScript;

    [SerializeField] private MarkerFollowMouse constructionPrevisualisation;
    public LineRenderer lineRendererObject;

    public Material matReference;
    private Material savedMaterial;

    [Header("Feedbacks")]
    [SerializeField] private UnityEvent PlayOnPathToolSelected;
    [SerializeField] private UnityEvent PlayOnPathToolUnselected;

    [Header("Constructions Prefabs")]
    [SerializeField] private PathPointPreview pathpointPrefab;

    private Construction selectedConstruction;

    public static Construction SelectedConstruction => instance.selectedConstruction;

    private void Awake()
    {
        instance = this;
        _selectedTool = SelectedTools.None;
    }

    private void Start()
    {
        eventSystemKeepSelectedScript = EventSystem.current.gameObject.GetComponent<EventSystemKeepSelected>();
    }

    public void OnSelectPathTool(RectTransform rt)
    {
        if(_selectedTool == SelectedTools.PathTool)
        {
            PlayOnPathToolUnselected?.Invoke();

            Valley_PathManager.CompletePath();
            constructionPrevisualisation.SetSelectedTool(null);
            eventSystemKeepSelectedScript.RemoveLastSelected();
            _selectedTool = SelectedTools.None;
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            ActivePathTool(rt);
        }
    }

    public void ActivePathTool(RectTransform rt)
    {
        PlayOnPathToolSelected?.Invoke();
        instance.constructionPrevisualisation.SetSelectedTool(pathpointPrefab);
        eventSystemKeepSelectedScript.KeepSelected();
        _selectedTool = SelectedTools.PathTool;
    }

    public static EventSystemKeepSelected GetEventSystemKeepSelected()
    {
        return eventSystemKeepSelectedScript;
    }

    public static void CreateLink(PathPoint firstObjectToLink)
    {
        LineRenderer ln = Instantiate(instance.lineRendererObject, firstObjectToLink.transform.position, Quaternion.identity, firstObjectToLink.transform);

        //SharedMaterial pour le même chemin
        if (Valley_PathManager.GetCurrentPath.pathFragment.Count == 0)
        {
            //Create Material for the new path
            ln.material = Instantiate(instance.matReference);                             //Material Instance
            instance.savedMaterial = ln.material;                                         //Save Material Instance
            Valley_PathManager.GetCurrentPath.colorPath = Random.ColorHSV();              //Random Color
            ln.material.color = Valley_PathManager.GetCurrentPath.colorPath;              //Applicate Random Color 
        }
        else
        {
            //Applicate the savedMaterial 
            ln.material.color = Valley_PathManager.GetCurrentPath.colorPath;
            //ln.material = instance.savedMaterial;
        }

        firstObjectToLink.GetLink.line = ln;
        firstObjectToLink.GetLink.FirstPoint();
    }

    //Call at each new Marker
    public static void EndPreviousLink(PathPoint nextObjectToLink, PathPoint previousMarker, out LineRenderer lineToReturn)
    {
        previousMarker.GetLink.AddPoint(nextObjectToLink.gameObject, out lineToReturn);
    }

    //Call when "Return" key is press
    public static void EndLink(PathPoint currentMarker)
    {
        currentMarker.GetLink.EndPoint(currentMarker.gameObject);
    }

    public static void ResetLink(PathPoint firstObjectToLink)
    {
        firstObjectToLink.GetLink.ResetPoint();
    }
}
