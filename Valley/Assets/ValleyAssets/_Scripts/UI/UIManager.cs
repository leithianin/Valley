using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public GameObject buttonsList;

    private GameObject markerSelected;

    public LandInteractions _landInteraction;

    public Text woodCounter;

    public Text visitorsCounter;


    public static List<Valley_PathData> pathToModify = new List<Valley_PathData>();

    public List<GameObject> toolsList = new List<GameObject>();
    private bool isToolSelected = false;

    public UnityEvent OnMovingTool;
    public UnityEvent OnMovingToolCompleted;

    public static UnityEvent GetOnMovingTool => instance.OnMovingTool;
    public static UnityEvent GetOnMovingToolCompleted => instance.OnMovingToolCompleted;


    private void Awake()
    {
        instance = this;
    }

    #region Create/Modify Path
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
        Valley_PathManager.CreateNewPath(_landInteraction.selectedConstruction as PathPoint);
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
        Valley_PathManager.ModifyPath(pathLinked.path);
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
    #endregion

    public void OnSelected()
    {
        if(!isToolSelected)
        {
            OnHideTools();
        }
        else
        {
            OnShowTools();
        }
    }

    public void OnShowTools()
    {
        Debug.Log("Show Tool");
        isToolSelected = false;

        foreach (GameObject go in toolsList)
        {
            go.GetComponent<dfb_MoveSign>().OnMove();
        }
    }

    public void OnHideTools()
    {
        Debug.Log("Hide Tool");
        isToolSelected = true;

        foreach (GameObject go in toolsList)
        {
            go.GetComponent<dfb_MoveSign>().OnMove();
        }
    }

    #region Ressources
    public static void UpdateWood(int woodNb)
    {
        instance.woodCounter.text = woodNb.ToString();
    }

    public static void UpdateVisitors(int visitorNb)
    {
        instance.visitorsCounter.text = visitorNb.ToString();
    }
    #endregion
}
