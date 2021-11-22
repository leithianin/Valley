using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerFollowMouse : MonoBehaviour
{
    private void Update()
    {
        if(ToolManager._selectedTool == SelectedTools.PathTool)
        {
            RaycastHit();
        }
    }

    public void RaycastHit()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100000.0f))
        {
            gameObject.transform.position = hit.point;

            if(hit.transform.gameObject.GetComponent<PathPoint>())
            {
                gameObject.transform.position = hit.transform.gameObject.transform.position;
            }
        }
    }
}
