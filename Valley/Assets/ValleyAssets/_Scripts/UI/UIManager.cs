using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public GameObject buttonsList;

    private GameObject markerSelected;

    public LandInteractions _landInteraction;
    public static List<Valley_PathData> pathToModify = new List<Valley_PathData>();

    private void Awake()
    {
        instance = this;
    }

    //UI Create or Modify Path
    public static void ShowButtonsUI(GameObject marker)
    {
        instance.markerSelected = marker;
        Vector3 markerPos = marker.transform.position;

        float offsetPosY = markerPos.y + 1.5f;
        float offsetPosX = markerPos.x + 10f;

        Vector3 offsetPos = new Vector3(offsetPosX, offsetPosY, markerPos.z);

        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(instance.buttonsList.transform.parent.parent.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);

        instance.buttonsList.transform.localPosition = canvasPos;

        instance.buttonsList.SetActive(true);
    }

    public void CreateNewPath()
    {
        _landInteraction.CreatePathWithoutMarker(markerSelected);
        markerSelected = null;
        instance.OnHideButtons();
    }

    public static void ModifyPathCount(int nb)
    {
        for(int i=1; i<=6;i++)
        {
            if (i <= nb)
            {
                GameObject button = instance.buttonsList.transform.GetChild(i).gameObject;
                button.GetComponent<PathLinked>().path = pathToModify[i - 1];
                button.SetActive(true);
            }
            else
            {
                GameObject button = instance.buttonsList.transform.GetChild(i).gameObject;
                button.GetComponent<PathLinked>().path = null;
                button.SetActive(false);
            }
        }

        instance.OnHideButtons();
    }

    public void ModifyPath(PathLinked pathLinked)
    {
        _landInteraction.ModifyPath(pathLinked.path);
        instance.OnHideButtons();
    }


    public static void Hidebuttons()
    {
        instance.OnHideButtons();
    }


    private void OnHideButtons()
    {
        buttonsList.SetActive(false);
    }
}
