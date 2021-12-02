using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    private static ConstructionManager instance;

    private static int Test = 0;    //A delete si j'oublis

    private ConstructionPreview inConstruction;

    public static ConstructionPreview GetCurrentConstruction => instance.inConstruction;

    private void Awake()
    {
        instance = this;
    }

    public static void ChooseConstruction(ConstructionPreview constructionChosen)
    {
        instance.inConstruction = constructionChosen;
    }

    public static bool PlaceConstruction(ConstructionPreview toPlace, Vector3 positionToPlace)
    {
        Debug.Log("Can place : " + toPlace.CanPlaceObject(positionToPlace));
        if(toPlace.CanPlaceObject(positionToPlace))
        {
            Construction placedObject = Instantiate(toPlace.RealConstruction, positionToPlace, Quaternion.identity);
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
                toDelete.RemoveObject();
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
