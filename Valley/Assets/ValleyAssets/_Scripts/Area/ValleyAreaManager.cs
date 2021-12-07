using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class ValleyAreaManager : MonoBehaviour
{
    private static ValleyAreaManager instance;
    [SerializeField] private List<ValleyArea> areas;

    private List<VisitorAgentBehave> visitors = new List<VisitorAgentBehave>();

    [SerializeField] private int visitorByFrame = 20;

    private int visitorChecked = 0;

    private List<ValleyArea> updatableArea = new List<ValleyArea>();

    private static List<ValleyArea> pathAreas = new List<ValleyArea>();                      //List de ValleyArea dans lequel un chemin passe.

    private static float pointsByPath = 4;
    private static float pointByPathValue;

    public static List<ValleyArea> GetAreas => instance.areas;

    private List<LineRenderer> usedLineRenderer;
    [SerializeField] private Transform zoneDelimitationParent;
    [SerializeField] private LineRenderer zoneDelimitationPrefab;

    [ContextMenu("Set positions")]
    private void SetPositions()
    {
        for(int i = 0; i < usedLineRenderer.Count; i++)
        {
            Destroy(usedLineRenderer[i].gameObject);
        }

        for (int i = 0; i < areas.Count; i++)
        {
            ValleyArea area = areas[i];
            area.borders = new List<Vector2>();
            LineRenderer line = Instantiate(zoneDelimitationPrefab, zoneDelimitationParent);
            line.positionCount = area.bordersTransform.Count;
            for (int j = 0; j < area.bordersTransform.Count; j++)
            {
                area.borders.Add(new Vector2(area.bordersTransform[j].position.x, area.bordersTransform[j].position.z));

                
                
                line.SetPosition(j, area.bordersTransform[j].position);
            }
            usedLineRenderer.Add(line);
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pointByPathValue = 1 / pointsByPath;
        /*if(visitorByFrame > areas.Count)
        {
            visitorByFrame = areas.Count;
        }*/

        //gameObject.SetActive(false);
    }

    private void Update()
    {
        visitors = VisitorManager.GetVisitors;

        if (visitors.Count > 0)
        {
            for (int i = 0; i < visitorByFrame; i++)
            {
                int visitorIndex = (visitorChecked + i) % visitors.Count;
                if (visitors[visitorIndex].gameObject.activeSelf)
                {
                    ValleyArea toAdd = GetZoneFromPosition(visitors[visitorIndex].GetPosition);

                    if (toAdd != visitors[visitorIndex].currentArea && toAdd != null)
                    {
                        visitors[visitorIndex].currentArea.visitorInZone.Remove(visitors[visitorIndex]);
                        visitors[visitorIndex].currentArea = toAdd;
                        toAdd.visitorInZone.Add(visitors[visitorIndex]);
                    }

                    if (toAdd != null && !updatableArea.Contains(toAdd))
                    {
                        updatableArea.Add(toAdd);
                    }
                }
            }

            visitorChecked = (visitorChecked + visitorByFrame) % visitors.Count;
        }
    }

    private void LateUpdate()
    {
        if (updatableArea.Count > 0)
        {
            ValleyArea area = updatableArea[0];
            float areaSoundLevel = 0;
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

    public static void GetZoneFromLineRenderer(LineRenderer ln)
    {
        for(int i = 0; i < ln.positionCount-1; i++)
        {
            //Take two points by point ( a-b / b-c / c/d / ...)
            //Take positions of those 2 points
            Vector3 point1 = ln.GetPosition(i);
            Vector3 point2 = ln.GetPosition(i+1);

            //Check PointsByPath points on a path
            for(int j = 1; j <= pointsByPath; j++)
            {
                ValleyArea zone = instance.GetZoneFromPosition(GetVectorPoint(point1, point2, pointByPathValue * j));
                Debug.Log(zone);
                if(zone != null)
                {
                    CheckIfZoneAlreadySaved(zone);
                }
            }  
        }

        foreach(ValleyArea va in pathAreas)
        {
            //Save pathAreas in PathData
            Valley_PathManager.GetCurrentPath.valleyAreaList.Add(va);

            //Debug.Log("Zone : " + va.nom);
        }
        
        pathAreas.Clear();
    }

    public static Vector2 GetVectorPoint(Vector3 point1, Vector3 point2, float t)
    {
        float pointx = point1.x * (1 - t) + point2.x * t;
        float pointz = point1.z * (1 - t) + point2.z * t;

        Vector2 pointPlaced = new Vector2(pointx, pointz);

        return pointPlaced;
    }

    public static void CheckIfZoneAlreadySaved(ValleyArea zone)
    {
        foreach(ValleyArea va in pathAreas)
        {
            if(va == zone)
            {
                return;
            }
        }

        pathAreas.Add(zone);
    }

    //Remake a new list when delete a marker
    //Code Review : Just update the list don't create a new one
    public static void UpdatePathArea(Valley_PathData path)
    {
        if (Valley_PathManager.GetCurrentPath != null)
        {
            Valley_PathManager.GetCurrentPath.valleyAreaList.Clear();
            foreach (PathFragmentData pfd in path.pathFragment)
            {
                GetZoneFromLineRenderer(pfd.line);
            }
        }
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
