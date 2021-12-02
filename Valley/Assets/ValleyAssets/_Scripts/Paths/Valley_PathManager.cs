using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Valley_PathManager : MonoBehaviour
{
    private static Valley_PathManager instance;

    [SerializeField] private List<Valley_PathData> existingPaths = new List<Valley_PathData>(); // Liste des points existants.

    private static Valley_PathData currentPathOn;
    private bool isNewPath = true;
    private PathPoint firstMarker;   //Maybe Obsolete

    private static PathPoint currentMarker;
    private PathPoint previousMarker = null;

    private LineRenderer lineRendererToSave;

    public static Valley_PathData GetCurrentPath => currentPathOn;
    public static PathPoint GetCurrentMarker => currentMarker;

    public UnityEvent PlayOnCompletePath;

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

    public static List<Valley_PathData> GetAllPossiblePath(PathPoint startPoint)
    {
        return instance.OnGetAllPossiblePath(startPoint);
    }

    private List<Valley_PathData> OnGetAllPossiblePath(PathPoint startPoint)
    {
        List<Valley_PathData> possiblePath = new List<Valley_PathData>();
        for (int i = 0; i < existingPaths.Count; i++)
        {
            if(existingPaths[i].ContainsPoint(startPoint))
            {
                possiblePath.Add(existingPaths[i]);
            }
        }
        return possiblePath;
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

    public static void RemovePathData()
    {
        instance.existingPaths.Remove(currentPathOn);
        currentPathOn = null;
    }

    public static void AddPathPoint(PathPoint marker)
    {
        /***
        PathPoint previousPathPoint = instance.GetPreviousPathPoint(marker);
        Debug.Log(currentMarker);
        ***/

        currentMarker.AddPoint(marker, currentPathOn);
        marker.AddPoint(currentMarker, currentPathOn);
    }

    public static void AddPathPointWithoutMarker(PathPoint targetMarker, PathPoint previousMarker)
    {
        targetMarker.AddPoint(previousMarker, currentPathOn);
        previousMarker.AddPoint(targetMarker, currentPathOn);
    }

    //Remove PathPoint from PreviousMarker and CurrentMarker (Usefull to see if CurrentMarker have still pathPoint or not)
    public static void RemovePathPoint(PathPoint currentMarker, PathPoint previousMarker)
    {
        if (GetCurrentPath.pathFragment.Count > 0)
        {
            previousMarker.RemovePoint(currentMarker);
            currentMarker.RemovePoint(previousMarker);
        }
    }

    /*private PathPoint GetPreviousPathPoint(PathPoint currentPathPoint)
    {
        int pathPointIndex = FindIndex(currentPathPoint);

        if(pathPointIndex > 0)
        {
            return currentPathOn.pathPoints[pathPointIndex-1];
        }

        return null;
    }*/

    /*private int FindIndex(PathPoint currentPathPoint)
    {
        for (int i = 0; i < currentPathOn.pathPoints.Count; i++)
        {
            if(currentPathOn.pathPoints[i] == currentPathPoint)
            {
                return i;
            }
        }

        return -1;
    }*/

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
            CreatePath();
            //GetCurrentPath.pathPoints.Add(toPlace);
            GetCurrentPath.startPoint = toPlace;
            //ToolManager.CreateLink(toPlace);                        //Chaque Marker à le droit à son LineRenderer
            isNewPath = false;
        }
        else
        {
            //Chemin = CurrentMarker LineRenderer
            //Check Zone Where the path pass
            ValleyAreaManager.GetZoneFromLineRenderer(currentMarker.GetLink.line);
            //Close link précedent
            List<Vector3> navPath = new List<Vector3>();
            LineRenderer line = new LineRenderer();
            ToolManager.EndPreviousLink(toPlace, currentMarker, out navPath, out line);

            GetCurrentPath.pathFragment.Add(new PathFragmentData(currentMarker, toPlace, navPath, line));
            //GetCurrentPath.pathPoints.Add(toPlace);
            AddPathPoint(toPlace);
        }

        ToolManager.CreateLink(toPlace);

        previousMarker = currentMarker;
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

    //CODE REVIEW : Pareil que Path Point désormais, merge ?
    //Click on a marker
    public void CreatePathWithoutMarker(PathPoint markerAlreadyPlace)
    {
        if (markerAlreadyPlace != currentMarker)
        {
            if (isNewPath)
            {
                firstMarker = markerAlreadyPlace;
                CreatePath();
                //GetCurrentPath.pathPoints.Add(markerAlreadyPlace);
                GetCurrentPath.startPoint = markerAlreadyPlace;
                ToolManager.CreateLink(markerAlreadyPlace);
                isNewPath = false;
            }
            else
            {
                List<Vector3> navPath = new List<Vector3>(); //CODE REVIEW : Voir pour modifier les 3 lignes du dessous, essayer de récupérer plus facilement le NavPath et le Line
                LineRenderer line = new LineRenderer();
                ToolManager.EndPreviousLink(markerAlreadyPlace, currentMarker, out navPath, out line);

                List<Vector3> toAdd = new List<Vector3>();

                for (int i = 0; i < navPath.Count; i++)
                {
                    toAdd.Add(navPath[i]);
                }


                GetCurrentPath.pathFragment.Add(new PathFragmentData(currentMarker, markerAlreadyPlace, toAdd, line));
                //GetCurrentPath.pathPoints.Add(markerAlreadyPlace);                          //Add point to the path         
                AddPathPoint(markerAlreadyPlace);
                //AddPathPointWithoutMarker(markerAlreadyPlace, currentMarker);
            }

            ToolManager.CreateLink(markerAlreadyPlace);

            previousMarker = currentMarker;
            currentMarker = markerAlreadyPlace;
        }
    }

    public void OnModifyPath(Valley_PathData _pathData)
    {
        isNewPath = false;
        currentMarker = _pathData.pathFragment[_pathData.pathFragment.Count - 1].endPoint;
        //currentMarker = _pathData.pathPoints[_pathData.pathPoints.Count-1];
        SetCurrentPath(_pathData);

        //Je check ça aussi dans DeletePreviousMarker --> Peut être mettre dans une fonction à part
        /*if (_pathData.pathPoints.Count > 1)
        {
            previousMarker = _pathData.pathPoints[_pathData.pathPoints.Count - 2];
        }
        else
        {
            previousMarker = null;
        }*/

        previousMarker = _pathData.pathFragment[_pathData.pathFragment.Count - 1].startPoint;

        ToolManager.CreateLink(currentMarker);

        //ToolManager.AddLink(_pathData.pathPoints[_pathData.pathPoints.Count-1].gameObject, firstMarker);

        //Get Link in first Marker
        //Add Link Last to Mouse Position
    }

    private void OnCompletePath()
    {
        PlayOnCompletePath?.Invoke();
        EndPath();
        ToolManager.EndLink(currentMarker);
        isNewPath = true;
    }

    private void DeletePreviousMarker()
    {
        if (GetCurrentPath != null)
        {
            if (currentMarker == existingPaths[0].startPoint && GetCurrentPath == existingPaths[0])
            {
                OnCompletePath();  
            }
            else
            {
                RemovePathPoint(currentMarker, previousMarker);
                PathFragmentData removedFragment = GetCurrentPath.RemoveFragment(currentMarker, previousMarker);

                if (removedFragment != null)
                {
                    lineRendererToSave = removedFragment.line;
                }

                if (currentMarker.GetNbLinkedPoint() <= 0)
                {
                    currentMarker.DespawnObject();
                }
                
                /*if(lineRendererToSave != null)
                {
                    Destroy(lineRendererToSave.gameObject);
                }*/

                //Change CurrentMarker if CurrentPath have PathPoints left
                if (previousMarker != null)
                {
                    currentMarker = previousMarker;
                    currentMarker.GetLink.UpdateLineWithLineKnowed(lineRendererToSave);

                    //Don't Set PreviousMarker if PathPoint have only 1 point
                    if (GetCurrentPath.pathFragment.Count > 0) { previousMarker = GetCurrentPath.pathFragment[GetCurrentPath.pathFragment.Count - 1].startPoint; }
                    else { previousMarker = null; }
                }
                else
                {
                    currentMarker.GetLink.UpdateLineWithLineKnowed(null);
                    RemovePathData();
                    isNewPath = true;
                    currentMarker = null;
                }
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
