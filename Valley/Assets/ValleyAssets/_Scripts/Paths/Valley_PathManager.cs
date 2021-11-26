using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valley_PathManager : MonoBehaviour
{
    private static Valley_PathManager instance;

    [SerializeField] private List<Valley_PathData> existingPaths = new List<Valley_PathData>(); // Liste des points existants.

    private static Valley_PathData currentPathOn;
    private bool isNewPath = true;
    private PathPoint firstMarker;
    private PathPoint currentMarker;


    /*[Header("Tests")]
    [SerializeField] private List<PathPoint> firstPathPoints;
    [SerializeField] private List<PathPoint> secondPathPoints;*/

    public static bool HasAvailablePath(PathPoint spawnPoint)
    {
        return instance.CheckForAvailablePath(spawnPoint);
    }

    private void Awake()
    {
        instance = this;    
    }

    private bool CheckForAvailablePath(PathPoint spawnPoint)
    {
        for(int i = 0; i < existingPaths.Count; i++)
        {
            if(existingPaths[i].IsUsable(spawnPoint))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Permet de r�cup�rer un chemin choisit al�atoirement.
    /// </summary>
    /// <returns>Le chemin choisit.</returns>
    public static Valley_PathData GetRandomPath()
    {
        int result = Random.Range(0, instance.existingPaths.Count);
        return instance.existingPaths[result];
    }

    public static Valley_PathData GetRandomPath(PathPoint startPoint)
    {
        Valley_PathData path = GetRandomPath();
        int i = 0; 

        while(!path.IsUsable(startPoint) && i < 100)
        {
            path = GetRandomPath();
            i++;
        }

        if(!path.IsUsable(startPoint))
        {
            path = null;
        }

        return path;
    }

    public static void CreatePath()
    {
        Valley_PathData path = new Valley_PathData();
        currentPathOn = path; 
    }

    public static void SetCurrentPath(Valley_PathData path)
    {
        currentPathOn = path;
    }

    public static void EndPath()
    {
        instance.OnEndPath();

    }

    private void OnEndPath()
    {
        if(!existingPaths.Contains(currentPathOn))
        {
            existingPaths.Add(currentPathOn);
        }
    }

    public static Valley_PathData GetCurrentPath()
    {
        return currentPathOn;
    }

    public static void RemovePathData()
    {
        instance.existingPaths.Remove(currentPathOn);
        currentPathOn = null;
    }

    public static void AddPathPoint(PathPoint marker)
    {
        PathPoint previousPathPoint = instance.GetPreviousPathPoint(marker);

        previousPathPoint.AddPoint(marker, currentPathOn);
        marker.AddPoint(previousPathPoint, currentPathOn);
    }

    public static void AddPathPointWithoutMarker(PathPoint targetMarker, PathPoint previousMarker)
    {
        targetMarker.AddPoint(previousMarker, currentPathOn);
        previousMarker.AddPoint(targetMarker, currentPathOn);
    }

    public static void RemovePathPoint(PathPoint marker)
    {
        if (GetCurrentPath().pathPoints.Count > 1)
        {
            PathPoint previousPathPoint = GetCurrentPath().pathPoints[GetCurrentPath().pathPoints.Count - 2];

            previousPathPoint.RemovePoint(marker);
            marker.RemovePoint(previousPathPoint);
        }
    }

    private PathPoint GetPreviousPathPoint(PathPoint currentPathPoint)
    {
        int pathPointIndex = FindIndex(currentPathPoint);

        if(pathPointIndex > 0)
        {
            return currentPathOn.pathPoints[pathPointIndex-1];
        }

        return null;
    }

    private int FindIndex(PathPoint currentPathPoint)
    {
        for (int i = 0; i < currentPathOn.pathPoints.Count; i++)
        {
            if(currentPathOn.pathPoints[i] == currentPathPoint)
            {
                return i;
            }
        }

        return -1;
    }

    public static int GetNumberOfPathPoints(PathPoint pathPoint)
    {
        UIManager.pathToModify.Clear();
        int n = 0;

        for (int i = 0; i < instance.existingPaths.Count; i++)
        {
            if(instance.existingPaths[i].ContainsPoint(pathPoint))
            {
                UIManager.pathToModify.Add(instance.existingPaths[i]);
                n++;
            }                 
        }

        return n;
    }


    // PLACEMENT DES POINTS


    public static void PlacePathPoint(PathPoint toPlace)
    {
        instance.OnPlacePoint(toPlace);
    }

    public void OnPlacePoint(PathPoint toPlace)
    {
        if (isNewPath)
        {
            firstMarker = toPlace;
            CreatePath();
            GetCurrentPath().pathPoints.Add(toPlace);
            ToolManager.CreateLink(toPlace);
            isNewPath = false;
        }
        else
        {
            ToolManager.AddLink(toPlace, firstMarker);
            GetCurrentPath().pathPoints.Add(toPlace);
            AddPathPoint(toPlace);
        }

        currentMarker = toPlace;
    }

    public static void PlaceOnPoint(PathPoint selectedPoint)
    {
        if (!instance.isNewPath)
        {
            instance.CreatePathWithoutMarker(selectedPoint);
        }
        else
        {
            instance.CreateOrModifyPath(selectedPoint);
        }
    }

    public static void ModifyPath(Valley_PathData _pathData)
    {
        instance.OnModifyPath(_pathData);
    }

    public static void DeleteLastPoint()
    {
        instance.DeletePreviousMarker();
    }

    public static void CompletePath()
    {
        instance.OnCompletePath();
    }

    public static void CreateNewPath(PathPoint startPoint)
    {
        instance.CreatePathWithoutMarker(startPoint);
    }

    //Click on a marker
    public void CreatePathWithoutMarker(PathPoint markerAlreadyPlace)
    {
        if (isNewPath)
        {
            firstMarker = markerAlreadyPlace;
            CreatePath();
            GetCurrentPath().pathPoints.Add(markerAlreadyPlace);
            ToolManager.CreateLink(markerAlreadyPlace);
            isNewPath = false;
        }
        else
        {
            GetCurrentPath().pathPoints.Add(markerAlreadyPlace);
            ToolManager.AddLink(markerAlreadyPlace, firstMarker);
            AddPathPointWithoutMarker(markerAlreadyPlace, currentMarker);
        }

        currentMarker = markerAlreadyPlace;
    }

    public void OnModifyPath(Valley_PathData _pathData)
    {
        isNewPath = false;
        firstMarker = _pathData.pathPoints[0];

        firstMarker.GetComponent<VisibleLink>().SetLine(_pathData.lineRenderer);

        SetCurrentPath(_pathData);
        //ToolManager.AddLink(_pathData.pathPoints[_pathData.pathPoints.Count-1].gameObject, firstMarker);

        //Get Link in first Marker
        //Add Link Last to Mouse Position
    }

    private void OnCompletePath()
    {
        EndPath();
        ToolManager.EndLink(firstMarker);
        isNewPath = true;
    }

    private void DeletePreviousMarker()
    {
        if (GetCurrentPath() != null)
        {
            PathPoint localMarker = GetCurrentPath().pathPoints[GetCurrentPath().pathPoints.Count - 1];
            ToolManager.ResetLink(firstMarker);
            RemovePathPoint(GetCurrentPath().pathPoints[GetCurrentPath().pathPoints.Count - 1]);
            GetCurrentPath().pathPoints.RemoveAt(GetCurrentPath().pathPoints.Count - 1);

            if (localMarker.GetComponent<PathPoint>().GetNbLinkedPoint() > 0)
            {
                //Dont deztroy
            }
            else
            {
                Destroy(localMarker.gameObject);
            }

            Debug.Log(GetCurrentPath().pathPoints.Count);
            if (GetCurrentPath().pathPoints.Count > 0)
            {
                currentMarker = GetCurrentPath().pathPoints[GetCurrentPath().pathPoints.Count - 1];
            }
            else
            {
                RemovePathData();
                isNewPath = true;
                currentMarker = null;
            }
        }
    }

    private void CreateOrModifyPath(PathPoint selectedMarker)
    {
        CheckHowManyPathToModify(selectedMarker);

        //Check in Existing point if we find the Path Point in several paths
        UIManager.ShowButtonsUI(selectedMarker.gameObject);
    }

    private void CheckHowManyPathToModify(PathPoint pathPoint)
    {
        UIManager.ModifyPathCount(GetNumberOfPathPoints(pathPoint));
    }
}
