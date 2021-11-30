using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisibleLink : MonoBehaviour
{
    public LineRenderer line;
    private bool isSecondPointIsPlaced = false;
    public int index = 1;
    private Vector3 offsetPathCalcul = new Vector3(0,0,-0.5f);

    private NavMeshPath path;

    private void Awake()
    {
        path = new NavMeshPath();
    }

    void Update()
    {
        if (line != null)
        {
            line.SetPosition(index, GetPositionSecondPoint());
            path = new NavMeshPath();
            if (line.positionCount > 0)
            {
                NavMesh.CalculatePath(Valley_PathManager.GetCurrentMarker.Position +offsetPathCalcul, GetPositionSecondPoint(), NavMesh.AllAreas, path);

                List<Vector3> points = new List<Vector3>();

                int j = 1;

                //Debug.Log(path.corners.Length);
                while (j < path.corners.Length)
                {
                    line.positionCount = path.corners.Length;
                    points = new List<Vector3>(path.corners);
                    for (int k = 0; k < points.Count; k++)
                    {
                        line.SetPosition(k, points[k]);
                    }

                    j++;
                }

                index = line.positionCount - 1;
            }
        }
    }

    public void FirstPoint()
    {
        //Remet � 1 l'index si jamais il se retrouve � 0 lors de la suppression
        if(index == 0)
        {
            index = 1;
        }

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);               //Sert � �viter qu'il spawn � 0,0,0 le temps d'une frame, le montrant au joueur par la m�me occasion
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
