using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valley_PathManager : MonoBehaviour
{
    private static Valley_PathManager instance;

    [SerializeField]
    private List<Valley_PathData> existingPaths = new List<Valley_PathData>();

    [Header("Tests")]
    [SerializeField] private List<PathPoint> firstPathPoints;
    [SerializeField] private List<PathPoint> secondPathPoints;

    private void Awake()
    {
        instance = this;

        Valley_PathData path = new Valley_PathData();
        for(int i = 0; i < firstPathPoints.Count; i++)
        {
            path.pathPoints.Add(firstPathPoints[i]);
        }
        existingPaths.Add(path);

        Valley_PathData path2 = new Valley_PathData();
        for (int i = 0; i < secondPathPoints.Count; i++)
        {
            path2.pathPoints.Add(secondPathPoints[i]);
        }
        existingPaths.Add(path2);
    }

    public static Valley_PathData GetRandomPath()
    {
        return instance.existingPaths[Random.Range(0, instance.existingPaths.Count)];
    }
}
