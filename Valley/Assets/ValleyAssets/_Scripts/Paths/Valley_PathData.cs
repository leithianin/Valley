using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Valley_PathData
{
    public List<PathPoint> pathPoints = new List<PathPoint>(); // Liste des points du chemins.

    /// <summary>
    /// V�rifie si le chemin poss�de le point.
    /// </summary>
    /// <param name="toCheck">Le point � v�rifier.</param>
    /// <returns>Renvoi TRUE si le chemin poss�de le point. Sinon, renvoi FALSE.</returns>
    public bool ContainsPoint(PathPoint toCheck)
    {
        return pathPoints.Contains(toCheck);
    }

}
