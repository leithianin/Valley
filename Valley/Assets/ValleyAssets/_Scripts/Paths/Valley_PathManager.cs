using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valley_PathManager : MonoBehaviour
{
    private static Valley_PathManager instance;

    [SerializeField] private List<Valley_PathData> existingPaths = new List<Valley_PathData>(); // Liste des points existants.

    private static Valley_PathData currentPathOn;


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
            if(CheckPathUsability(existingPaths[i]) && existingPaths[i].ContainsPoint(spawnPoint))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckPathUsability(Valley_PathData pathToCheck)
    {
        if(pathToCheck.pathPoints.Count > 1)
        {
            return true;
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
        int i = 0;

        while(instance.CheckPathUsability(instance.existingPaths[result]) && i < 100)
        {
            result = Random.Range(0, instance.existingPaths.Count);
            i++;
        }

        return instance.existingPaths[result];
    }

    public static Valley_PathData GetRandomPath(PathPoint startPoint)
    {
        Valley_PathData path = GetRandomPath();
        int i = 0; 

        while(!path.ContainsPoint(startPoint) && i < 100)
        {
            path = GetRandomPath();
            i++;
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

    public static void AddPathPoint(GameObject marker)
    {
        PathPoint currentPathPoint = marker.GetComponent<PathPoint>();
        PathPoint previousPathPoint = instance.GetPreviousPathPoint(currentPathPoint);

        previousPathPoint.AddPoint(currentPathPoint, currentPathOn);
        currentPathPoint.AddPoint(previousPathPoint, currentPathOn);
    }

    public static void AddPathPointWithoutMarker(GameObject targetMarker, GameObject previousMarker)
    {
        PathPoint currentPathPoint = targetMarker.GetComponent<PathPoint>();
        PathPoint previousPathPoint = previousMarker.GetComponent<PathPoint>();

        currentPathPoint.AddPoint(previousPathPoint, currentPathOn);
        previousPathPoint.AddPoint(currentPathPoint, currentPathOn);
    }

    public static void RemovePathPoint(GameObject marker)
    {
        if (GetCurrentPath().pathPoints.Count > 1)
        {
            PathPoint currentPathPoint = marker.GetComponent<PathPoint>();
            PathPoint previousPathPoint = GetCurrentPath().pathPoints[GetCurrentPath().pathPoints.Count - 2];

            previousPathPoint.RemovePoint(currentPathPoint);
            currentPathPoint.RemovePoint(previousPathPoint);
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
}
