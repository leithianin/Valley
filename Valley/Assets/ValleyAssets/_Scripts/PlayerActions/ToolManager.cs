using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AI;

public enum SelectedTools { None, PathTool }
public class ToolManager : MonoBehaviour
{
    private static ToolManager instance;
    public static SelectedTools _selectedTool;
    public static EventSystemKeepSelected eventSystemKeepSelectedScript;

    public MarkerFollowMouse constructionPrevisualisation;
    public LineRenderer lineRendererObject;

    public Material matReference;      
    private Material savedMaterial;

    [Header("Constructions Prefabs")]
    [SerializeField] private PathPoint pathpointPrefab;

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

    public void OnSelectPathTool(Button button)
    {
        if(_selectedTool == SelectedTools.PathTool)
        {
            constructionPrevisualisation.SetSelectedTool(null);
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
            ln.material = instance.savedMaterial;
        }

        firstObjectToLink.GetLink.line = ln;
        firstObjectToLink.GetLink.FirstPoint();
    }

    //Call at each new Marker
    public static void EndPreviousLink(PathPoint nextObjectToLink, PathPoint previousMarker, out List<Vector3> vectorPath, out LineRenderer lineToReturn)
    {
        previousMarker.GetLink.AddPoint(nextObjectToLink.gameObject, out vectorPath, out lineToReturn);
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
