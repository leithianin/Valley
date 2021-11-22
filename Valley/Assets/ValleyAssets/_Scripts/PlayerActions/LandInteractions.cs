using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LandInteractions : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnEndPath;

    public GameObject marker;
    public GameObject markersList;

    private bool isNewPath = true;

    private GameObject firstMarker;                                            //First path's marker
    private GameObject selectedMarker;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                Debug.Log("UI");
            }
            else
            {
                UIManager.Hidebuttons();
                if (ToolManager._selectedTool != SelectedTools.None)
                {
                    ToolManager.GetEventSystemKeepSelected().KeepSelected();

                    if (ToolManager._selectedTool == SelectedTools.PathTool)
                    {
                        if (GetHitGameObject().GetComponent<PathPoint>())
                        {
                            if (!isNewPath) { CreatePathWithoutMarker(selectedMarker); }
                            else            { CreateOrModifyPath(selectedMarker);}
                        }
                        else
                        {
                            PlaceMarker();
                        }
                    }
                }

                else
                {
                    Debug.LogError("No tool selected");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            CompletePath();
            ToolManager.ActivePathTool();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            DeletePreviousMarker();
        }
    }

    private Vector3 GetHitPoint()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100000.0f))
        {
            return hit.point;
        }

        Debug.Log("Raycast Error no hit Vector");
        return Vector3.zero;
    }

    private GameObject GetHitGameObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100000.0f))
        {
            selectedMarker = hit.transform.gameObject;
            return hit.transform.gameObject;
        }

        Debug.Log("Raycast Error no hit GameObject");
        return null;
    }

    private void PlaceMarker()
    {
        GameObject _marker = Instantiate(marker, GetHitPoint(), Quaternion.identity, markersList.transform);

        if (isNewPath)
        {
            firstMarker = _marker;
            Valley_PathManager.CreatePath();
            Valley_PathManager.GetCurrentPath().pathPoints.Add(_marker.GetComponent<PathPoint>());
            ToolManager.CreateLink(_marker);
            isNewPath = false;
        }
        else
        {
            ToolManager.AddLink(_marker, firstMarker);
            Valley_PathManager.GetCurrentPath().pathPoints.Add(_marker.GetComponent<PathPoint>());
        }
    }

    //Click on a marker
    public void CreatePathWithoutMarker(GameObject markerAlreadyPlace)
    {
        if (isNewPath)
        {
            firstMarker = markerAlreadyPlace;
            Valley_PathManager.CreatePath();
            Valley_PathManager.GetCurrentPath().pathPoints.Add(markerAlreadyPlace.GetComponent<PathPoint>());
            ToolManager.CreateLink(markerAlreadyPlace);
            isNewPath = false;
        }
        else
        {
            Valley_PathManager.GetCurrentPath().pathPoints.Add(markerAlreadyPlace.GetComponent<PathPoint>());
            ToolManager.AddLink(markerAlreadyPlace, firstMarker);
        }
    }

    public void ModifyPath(Valley_PathData _pathData)
    {
        isNewPath = false;
        firstMarker = _pathData.pathPoints[0].gameObject;
        firstMarker.GetComponent<VisibleLink>().SetLine(firstMarker.GetComponent<LineRenderer>());
        Valley_PathManager.SetCurrentPath(_pathData);
        //ToolManager.AddLink(_pathData.pathPoints[_pathData.pathPoints.Count-1].gameObject, firstMarker);

        //Get Link in first Marker
        //Add Link Last to Mouse Position
    }

    private void CompletePath()
    {
        
        Valley_PathManager.EndPath();
        ToolManager.EndLink(firstMarker);
        isNewPath = true;
    }

    private void DeletePreviousMarker()
    {
        ToolManager.ResetLink(firstMarker);
        Valley_PathManager.GetCurrentPath().pathPoints.Remove(Valley_PathManager.GetCurrentPath().pathPoints[Valley_PathManager.GetCurrentPath().pathPoints.Count-1]);
        Destroy(markersList.transform.GetChild(markersList.transform.childCount-1).gameObject);
    }

    private void CreateOrModifyPath(GameObject selectedMarker)
    {
        CheckHowManyPathToModify(selectedMarker.GetComponent<PathPoint>());
        //Check in Existing point if we find the Path Point in several paths
        UIManager.ShowButtonsUI(selectedMarker);
    }

    private void CheckHowManyPathToModify(PathPoint pathPoint)
    {
        UIManager.ModifyPathCount(Valley_PathManager.GetNumberOfPathPoints(pathPoint));
    }
}
