using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class ValleyAreaManager : MonoBehaviour
{
    [SerializeField] private List<ValleyArea> areas;

    private List<VisitorAgentBehave> visitors = new List<VisitorAgentBehave>();

    [SerializeField] private int visitorByFrame = 20;

    private int visitorChecked = 0;

    private List<ValleyArea> updatableArea = new List<ValleyArea>();

    [ContextMenu("Set positions")]
    private void SetPositions()
    {
        for (int i = 0; i < areas.Count; i++)
        {
            ValleyArea area = areas[i];
            area.borders = new List<Vector2>();
            for (int j = 0; j < area.bordersTransform.Count; j++)
            {
                area.borders.Add(new Vector2(area.bordersTransform[j].position.x, area.bordersTransform[j].position.z));
            }
        }
    }

    private void Start()
    {
        /*if(visitorByFrame > areas.Count)
        {
            visitorByFrame = areas.Count;
        }*/

        //gameObject.SetActive(false);
    }

    private void Update()
    {
        visitors = VisitorManager.GetVisitors;

        for (int i = 0; i < visitorByFrame; i++)
        {
            int visitorIndex = (visitorChecked + i) % visitors.Count;
            if (visitors[visitorIndex].gameObject.activeSelf)
            {
                ValleyArea toAdd = GetZoneFromPosition(visitors[visitorIndex].GetPosition);
                
                if(toAdd != visitors[visitorIndex].currentArea)
                {
                    visitors[visitorIndex].currentArea.visitorInZone.Remove(visitors[visitorIndex]);
                    visitors[visitorIndex].currentArea = toAdd;
                    toAdd.visitorInZone.Add(visitors[visitorIndex]);
                }

                if(toAdd != null && !updatableArea.Contains(toAdd))
                {
                    updatableArea.Add(toAdd);
                }
            }
        }

        visitorChecked = (visitorChecked + visitorByFrame) % visitors.Count;
    }

    private void LateUpdate()
    {
        if (updatableArea.Count > 0)
        {
            ValleyArea area = updatableArea[0];
            int areaSoundLevel = 0;
            for (int j = 0; j < area.visitorInZone.Count; j++)
            {
                areaSoundLevel += area.visitorInZone[j].Data.noiseMade;
            }
            area.SetSoundLevel(areaSoundLevel);
            updatableArea.RemoveAt(0);
        }
    }

    private ValleyArea GetZoneFromPosition(Vector2 toCheck)
    {
        ValleyArea toReturn = null;

        for (int i = 0; i < areas.Count; i++)
        {
            if (IsPositionInArea(toCheck, areas[i]))
            {
                toReturn = areas[i];
                break;
            }
        }

        return toReturn;
    }

    private bool IsPositionInArea(Vector2 toCheck, ValleyArea area)
    {
        return IsInsidePolygon(area.GetBorders.ToArray(), toCheck);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for(int i = 0; i < areas.Count; i++)
        {
            ValleyArea area = areas[i];
            for(int j = 0; j < area.bordersTransform.Count; j++)
            {
                Gizmos.DrawLine(area.bordersTransform[j].position, area.bordersTransform[(j + 1) % area.bordersTransform.Count].position);
            }
        }
    }


    #region Check Point in Polygon
    private float DistancePointLine2D(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        return (ProjectPointLine2D(point, lineStart, lineEnd) - point).magnitude;
    }
    private Vector2 ProjectPointLine2D(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        Vector2 rhs = point - lineStart;
        Vector2 vector2 = lineEnd - lineStart;
        float magnitude = vector2.magnitude;
        Vector2 lhs = vector2;
        if (magnitude > 1E-06f)
        {
            lhs = (Vector2)(lhs / magnitude);
        }
        float num2 = Mathf.Clamp(Vector2.Dot(lhs, rhs), 0f, magnitude);
        return (lineStart + ((Vector2)(lhs * num2)));
    }


    private float ClosestDistanceToPolygon(Vector2[] verts, Vector2 point)
    {
        int nvert = verts.Length;
        int i, j = 0;
        float minDistance = Mathf.Infinity;
        for (i = 0, j = nvert - 1; i < nvert; j = i++)
        {
            float distance = DistancePointLine2D(point, verts[i], verts[j]);
            minDistance = Mathf.Min(minDistance, distance);
        }

        return minDistance;
    }

    private bool IsInsidePolygon(Vector2[] vertices, Vector2 checkPoint, float margin = 0.01f)
    {
        if (ClosestDistanceToPolygon(vertices, checkPoint) < margin)
        {
            return true;
        }

        float[] vertX = new float[vertices.Length];
        float[] vertY = new float[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertX[i] = vertices[i].x;
            vertY[i] = vertices[i].y;
        }

        return IsInsidePolygon(vertices.Length, vertX, vertY, checkPoint.x, checkPoint.y);
    }

    private bool IsInsidePolygon(int nvert, float[] vertx, float[] verty, float testx, float testy)
    {
        bool c = false;
        int i, j = 0;
        for (i = 0, j = nvert - 1; i < nvert; j = i++)
        {
            if ((((verty[i] <= testy) && (testy < verty[j])) ||

                 ((verty[j] <= testy) && (testy < verty[i]))) &&

                (testx < (vertx[j] - vertx[i]) * (testy - verty[i]) / (verty[j] - verty[i]) + vertx[i]))
                c = !c;
        }
        return c;
    }
    #endregion
}
