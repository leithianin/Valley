using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleLink : MonoBehaviour
{
    public LineRenderer line;
    public Material mat_LineRender;
    private bool isSecondPointIsPlaced = false;
    public int index = 1;

    void Update()
    {
        if (line != null)
        {
            line.SetPosition(index, GetPositionSecondPoint());
        }
    }

    public void FirstPoint()
    {
        //Remet à 1 l'index si jamais il se retrouve à 0 lors de la suppression
        if(index == 0)
        {
            index = 1;
        }

        line.material = mat_LineRender;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);               //Sert à éviter qu'il spawn à 0,0,0 le temps d'une frame, le montrant au joueur par la même occasion
    }

    private Vector3 GetPositionSecondPoint()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100000.0f))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    public void AddPoint(GameObject nextObjectToLink)
    {
        line.SetPosition(index, nextObjectToLink.transform.position);
        line = null;
    }

    public void EndPoint(GameObject previousMarker)
    {
        Destroy(line.gameObject);
    }

    public void ResetPoint()
    {
        index--;
        line.positionCount--;
    }

    public void SetLine(LineRenderer ln)
    {
        line = ln;
        index = ln.positionCount++;
    }

    public void UpdateLine()
    {
        line = transform.GetChild(1).GetComponent<LineRenderer>();
    }

    public void UpdateLineWithLineKnowed(LineRenderer lineRenderer)
    {
        line = lineRenderer;
    }

    public LineRenderer FindLineRenderer(PathPoint objectToCheck)
    {
        if (objectToCheck != null)
        {
            for (int i = 1; i < objectToCheck.transform.childCount; i++)
            {
                if (objectToCheck.transform.GetChild(i).GetComponent<LineRenderer>().GetPosition(1) == gameObject.transform.position)
                {
                    return objectToCheck.transform.GetChild(i).GetComponent<LineRenderer>();
                }
            }
        }

        return null;
    }
}
