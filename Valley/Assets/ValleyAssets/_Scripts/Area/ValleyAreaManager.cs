using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class ValleyAreaManager : MonoBehaviour
{
    [SerializeField] private List<ValleyArea> areas;

    [SerializeField] private List<VisitorAgentBehave> visitors = new List<VisitorAgentBehave>();

    [SerializeField] private int areasByFrame = 20;

    private int aerasChecked = 0;

    private void Start()
    {
        for(int i = 0; i < visitors.Count; i++)
        {
            areas[Random.Range(0, areas.Count)].visitorInZone.Add(visitors[i]);
        }

        for(int i = 0; i < areas.Count; i++)
        {
            areas[i].borders = areas[0].borders;
        }

        if(areasByFrame > areas.Count)
        {
            areasByFrame = areas.Count;
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        //visitors = VisitorManager.GetVisitors;
        for(int i = 0; i < areasByFrame; i++)
        {
            int areasIndex = (aerasChecked + i) % areas.Count;
            for (int j = 0; j < areas[areasIndex].visitorInZone.Count; j++)
            {
                IsPositionInArea(Vector2.zero, areas[areasIndex]);
                Debug.Log("Allo");
            }
            //ValleyArea toUpdate = GetZoneFromPosition(visitors[i].GetPosition);
        }

        aerasChecked = (aerasChecked + areasByFrame) % areas.Count;
    }

    private void GetRandomValley()
    {
        int rng = Random.Range(0, areas.Count);
        for (int i = 0; i < areas.Count; i++)
        {
            Debug.Log("Test perf");
            if (i == rng)
            {
                break;
            }
        }

    }

    private ValleyArea GetZoneFromPosition(Vector2 toCheck)
    {
        ValleyArea toReturn = null;

        for (int i = 0; i < areas.Count; i++)
        {
            Debug.Log("Test perf");
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
        return IsPointInPolygon(toCheck, area.GetBorders.ToArray());
    }

    // Check de la présence d'un objet dans la zone
    // Update de toutes les zones
    // 
    // 
    // Différenciation des data Runtime (Bruit, Attractivité, ...) et data Brut (Animaux présent, ...)
    /* Exemple : Bruit dans la zone
     * - Attractivité <= Bruit dans la zone
     * - Placement de balise => Get la Data brut (Assignation des animaux présent sur le chemin depuis la data Brut)
     * - 
     * 
     */


    public bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
    {
        int polygonLength = polygon.Length, i = 0;
        bool inside = false;
        // x, y for tested point.
        float pointX = point.x, pointY = point.y;
        // start / end point for the current polygon segment.
        float startX, startY, endX, endY;
        Vector2 endPoint = polygon[polygonLength - 1];
        endX = endPoint.x;
        endY = endPoint.y;
        while (i < polygonLength)
        {
            startX = endX; startY = endY;
            endPoint = polygon[i++];
            endX = endPoint.x; endY = endPoint.y;
            //
            inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
                      && /* if so, test if it is under the segment */
                      ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
        }
        return inside;
    }

}
