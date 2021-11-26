using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LandInteractions : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnEndPath;

    [SerializeField] private PathPoint pathpointPrefab;

    public Construction marker;
    public Construction markersList;

    private bool isNewPath = true;

    public Construction firstMarker;                                            //First path's marker
    private Construction selectedMarker;
    private Construction currentMarker;

    private int test = 1;

    public Construction selectedConstruction;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.currentSelectedGameObject != null) // CODE REVIEW : A quoi ça sert ?
            {
                Debug.Log("UI");
            }
            else
            {
                OnMouseInput();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ConstructionManager.CompleteConstruction(ToolManager._selectedTool);
            ToolManager.ActivePathTool();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            AskToDeleteConstruction(selectedConstruction);
        }
    }

    private void OnMouseInput()
    {
        UIManager.Hidebuttons();

        selectedConstruction = GetHitConstruction();

        if (ToolManager._selectedTool != SelectedTools.None)
        {
            ToolManager.GetEventSystemKeepSelected().KeepSelected(); // Voir pour le mettre autre part, pour pouvoir supprimer le IF du dessus

            if (GetHitConstruction() != null)
            {
                ConstructionManager.PlaceOnExistingConstruction(GetHitConstruction(), ToolManager._selectedTool);
            }
            else
            {
                AskToPlaceConstruction(ToolManager._selectedTool);
            }
        }
        else
        {
            Debug.Log("No tool selected");
        }
    }

    private bool AskToPlaceConstruction(SelectedTools constructionType)
    {
        bool toReturn = false;

        switch (constructionType)
        {
            case SelectedTools.PathTool:
                toReturn = ConstructionManager.PlaceConstruction(pathpointPrefab, GetHitPoint());
                break;
        }

        return toReturn;
    }

    private void AskToDeleteConstruction(Construction toDelete)
    {
        if (toDelete != null)
        {
            ConstructionManager.DeleteConstruction(toDelete);
        }
        else
        {
            ConstructionManager.DeleteConstructionFromTool(ToolManager._selectedTool);
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

    private Construction GetHitConstruction()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100000.0f))
        {
            selectedMarker = hit.transform.gameObject.GetComponent<Construction>();
            return selectedMarker;
        }

        Debug.Log("Raycast Error no hit GameObject");
        return null;
    }
}
