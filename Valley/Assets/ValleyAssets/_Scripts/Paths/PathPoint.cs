using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : Construction
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

    public Action OnPointDestroyed;

    /// <summary>
    /// Permet de prendre un point al�atoire dans la liste des points existants
    /// </summary>
    /// <returns>Renvoie un point de la liste.</returns>
    public PathPoint GetRandomPoint()
    {
        return linkedPoints[UnityEngine.Random.Range(0, linkedPoints.Count)].point;
    }

    public PathPoint GetRandomPointFromList(List<PathPoint> toSearch)
    {
        return toSearch[UnityEngine.Random.Range(0, toSearch.Count)];
    }

    /// <summary>
    /// Permet de r�cup�rer un point du m�me chemin, ou de revenir en arri�re si il n'existe pas d'autres points.
    /// </summary>
    /// <param name="lastPoint">Le point visit� avant celui sur lequel le visiteur est.</param>
    /// <param name="path">Le chemin choisit par le visiteur.</param>
    /// <returns>Le nouveau point de destination du visiteur.</returns>
    public PathPoint GetNextPathPoint(PathPoint lastPoint, Valley_PathData path)
    {
        PathPoint toReturn = lastPoint;

        List<PathPoint> usablePoints = new List<PathPoint>(); // Liste des points appartenants au chemin d'entr�e
        for(int i = 0; i < linkedPoints.Count; i++)
        {
            if(linkedPoints[i].path == path)
            {
                usablePoints.Add(linkedPoints[i].point);
            }
        }

        if (usablePoints.Count > 1 || lastPoint == null) //V�rification d'un point inconnu existant
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

    private void OnDestroy()
    {
        OnPointDestroyed?.Invoke();
    }

    protected override bool OnPlaceObject(Vector3 position)
    {
        // R�cup�re le chemin actuel depuis le Manager
        // R�cup�re le dernier point poser depuis le Manager
        //   (On peut aussi appeler le manager pour SetPoint depuis ce dernier)
        // V�rifie si on peut le poser ou non (Pour l'instant, on retourne toujours True
        return true;
    }

    protected override void OnRemoveObject()
    {
        // On peut mettre la logique de DeletePreviousMarker ici
        /*
        if (localMarker.GetComponent<PathPoint>().GetNbLinkedPoint() > 0)
        {
            //Dont deztroy
        }
        else
        {
            Destroy(localMarker);
        }
        */
    }
}
