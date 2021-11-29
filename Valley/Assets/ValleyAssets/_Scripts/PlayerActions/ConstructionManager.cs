using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    private static ConstructionManager instance;

    [SerializeField] private Transform constructionHandler;

    private static int Test = 0;    //A delete si j'oublis

    private void Awake()
    {
        instance = this;
    }

    public static bool PlaceConstruction(Construction toPlace, Vector3 positionToPlace)
    {
        if(toPlace.CanPlaceObject(positionToPlace))
        {
            Construction placedObject = Instantiate(toPlace, positionToPlace, Quaternion.identity);
            placedObject.gameObject.name = "Marker_" + Test;                                            //A delete si j'oublis
            Test++;
            placedObject.PlaceObject(positionToPlace);
            return true;
        }

        return false;
    }

    public static void PlaceOnExistingConstruction(Construction existingConstruction, SelectedTools toolType)
    {
        switch(toolType)
        {
            case SelectedTools.PathTool:
                Valley_PathManager.PlaceOnPoint(existingConstruction as PathPoint);
                break;
        }
    }

    public static void ModifyConstruction(Construction toModify)
    {
        switch (toModify.LinkedTool())
        {
            case SelectedTools.PathTool:
                //Valley_PathManager.ModifyPath()
                break;
        }
    }

    public static void DeleteConstruction(Construction toDelete)
    {
        switch (toDelete.LinkedTool())
        {
            case SelectedTools.PathTool:
                Valley_PathManager.DeleteLastPoint();
                break;
        }
    }

    public static void DeleteConstructionFromTool(SelectedTools tool)
    {
        switch (tool)
        {
            case SelectedTools.PathTool:
                Valley_PathManager.DeleteLastPoint();
                break;
        }
    }

    public static void CompleteConstruction(SelectedTools selectedTool)
    {
        switch (selectedTool)
        {
            case SelectedTools.PathTool:
                Valley_PathManager.CompletePath();
                break;
        }
    }
}
