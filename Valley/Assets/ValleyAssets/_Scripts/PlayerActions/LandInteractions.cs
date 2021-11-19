using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LandInteractions : MonoBehaviour
{
    public GameObject marker;
    public GameObject markersList;

    private bool isNewPath = true;

    private GameObject firstMarker;                                            //First path's marker
    private GameObject selectedMarker;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ToolManager._selectedTool != SelectedTools.None)
            {
                ToolManager.GetEventSystemKeepSelected().KeepSelected();

                if (ToolManager._selectedTool == SelectedTools.PathTool)
                {
                    if (GetHitGameObject().GetComponent<PathPoint>())
                    {
                        CreatePathWithoutMarker(selectedMarker);
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

        if(Input.GetKeyDown(KeyCode.Return))
        {
            CompletePath();
            ToolManager._selectedTool = SelectedTools.PathTool;
            ToolManager.ActivePathTool();
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
    private void CreatePathWithoutMarker(GameObject markerAlreadyPlace)
    {
        Valley_PathManager.GetCurrentPath().pathPoints.Add(markerAlreadyPlace.GetComponent<PathPoint>());
        ToolManager.AddLink(markerAlreadyPlace, firstMarker);
    }

    private void CompletePath()
    {
        Valley_PathManager.EndPath();
        ToolManager.EndLink(firstMarker);
        isNewPath = true;
    }
}
