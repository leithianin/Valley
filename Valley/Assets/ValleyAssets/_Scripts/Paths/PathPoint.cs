using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    public Vector3 Position => transform.position;

    [SerializeField]
    private List<PathPoint> linkedPoints;

    /// <summary>
    /// Permet de prendre un point aléatoire dans la liste des points existants
    /// </summary>
    /// <returns>Renvoie un point de la liste.</returns>
    public PathPoint GetRandomPoint()
    {
        return linkedPoints[Random.Range(0, linkedPoints.Count)];
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
            if(path.ContainsPoint(linkedPoints[i]))
            {
                usablePoints.Add(linkedPoints[i]);
            }
        }

        if (usablePoints.Count > 1 || lastPoint == null) //Vérification d'un point inconnu existant
        {
            while (toReturn == lastPoint) //Tant qu'on connait le point choisit, on en rechoisit un
            {
                PathPoint newPoint = GetRandomPoint();
                if (path.ContainsPoint(newPoint))
                {
                    toReturn = newPoint;
                }
            }
        }
        return toReturn;
    }

    public void AddPoint(PathPoint pathPoint)
    {
        linkedPoints.Add(pathPoint);
    }

    public void RemovePoint(PathPoint pathPoint)
    {
        foreach(PathPoint pp in linkedPoints)
        {
            if(pp == pathPoint)
            {
                linkedPoints.Remove(pp);
                return;
            }
        }
    }
}
