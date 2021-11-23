using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    [System.Serializable]
    public class LinkedPointData
    {
        public PathPoint point;
        public Valley_PathData path;

        public LinkedPointData()
        {

        }

        public LinkedPointData(PathPoint nPoint, Valley_PathData nPath)
        {
            point = nPoint;
            path = nPath;
        }
    }

    public Vector3 Position => transform.position;

    [SerializeField]
    private List<LinkedPointData> linkedPoints = new List<LinkedPointData>();

    /// <summary>
    /// Permet de prendre un point aléatoire dans la liste des points existants
    /// </summary>
    /// <returns>Renvoie un point de la liste.</returns>
    public PathPoint GetRandomPoint()
    {
        return linkedPoints[Random.Range(0, linkedPoints.Count)].point;
    }

    public PathPoint GetRandomPointFromList(List<PathPoint> toSearch)
    {
        return toSearch[Random.Range(0, toSearch.Count)];
    }

    /// <summary>
    /// Permet de récupérer un point du même chemin, ou de revenir en arrière si il n'existe pas d'autres points.
    /// </summary>
    /// <param name="lastPoint">Le point visité avant celui sur lequel le visiteur est.</param>
    /// <param name="path">Le chemin choisit par le visiteur.</param>
    /// <returns>Le nouveau point de destination du visiteur.</returns>
    public PathPoint GetNextPathPoint(PathPoint lastPoint, Valley_PathData path)
    {
        PathPoint toReturn = lastPoint;

        List<PathPoint> usablePoints = new List<PathPoint>(); // Liste des points appartenants au chemin d'entrée
        for(int i = 0; i < linkedPoints.Count; i++)
        {
            if(linkedPoints[i].path == path)
            {
                usablePoints.Add(linkedPoints[i].point);
            }
        }

        if (usablePoints.Count > 1 || lastPoint == null) //Vérification d'un point inconnu existant
        {
            while (toReturn == lastPoint) //Tant qu'on connait le point choisit, on en rechoisit un
            {
                PathPoint newPoint = GetRandomPointFromList(usablePoints);
                if (path.ContainsPoint(newPoint))
                {
                    toReturn = newPoint;
                }
            }
        }
        return toReturn;
    }

    public int GetNbLinkedPoint()
    {
        return linkedPoints.Count;
    }

    public void AddPoint(PathPoint pathPoint, Valley_PathData path)
    {
        LinkedPointData toAdd = new LinkedPointData(pathPoint, path);
        linkedPoints.Add(new LinkedPointData(pathPoint, path));
    }

    public void RemovePoint(PathPoint pathPoint)
    {
        for(int i = 0; i < linkedPoints.Count; i++)
        {
            if(pathPoint == linkedPoints[i].point)
            {
                linkedPoints.RemoveAt(i);
                return;
            }
        }
    }
}
