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
    

    private void Awake()
    {
        instance = this;    
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

    public static int GetNumberOfPathPoints(PathPoint pathPoint)
    {
        int n = 0;

        for (int i = 0; i < instance.existingPaths.Count; i++)
        {
            if(instance.existingPaths[i].ContainsPoint(pathPoint))
            {
                UIManager.pathToModify.Add(instance.existingPaths[i]);
                n++;
            }                 
        }

        Debug.Log(n);
        return n;
    }
}
