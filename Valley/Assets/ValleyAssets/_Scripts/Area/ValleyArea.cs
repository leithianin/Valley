using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ValleyArea
{
    public string nom;
    [Tooltip("List of all the corners of the area.")]
    public List<Transform> bordersTransform;

    [Tooltip("Every elements that will be modified depending on the nature level of the area.")]
    [SerializeField] private List<AreaDisplay> displays;

    [Tooltip("Every interest point in the zone that visitors can interact with.")]
    [SerializeField] private List<InterestPoint> interestPoints;


    [HideInInspector]
    public List<Vector2> borders;

    // A list of all the visitor currently in the area.
    [HideInInspector]
    public List<VisitorAgentBehave> visitorInZone;

    private float noiseScore = 0;

    /// <summary>
    /// Return alist of all the borders of the area.
    /// </summary>
    public List<Vector2> GetBorders => borders;

    /// <summary>
    /// Return a list of all the Display contained in the area.
    /// </summary>
    public List<AreaDisplay> Displays => displays;

    /// <summary>
    /// Return a list of all the InterestPoints contained in the area.
    /// </summary>
    public List<InterestPoint> InterestPoints => interestPoints;

    /// <summary>
    /// Return the noise score.
    /// </summary>
    public float NoiseScore => Mathf.Clamp(noiseScore, 0, 20);

    /// <summary>
    /// Check if the area contains a certain type of Landmark.
    /// </summary>
    /// <param name="landmarkType">The Landmark to search.</param>
    /// <returns>Return true if it contains the Landmark we search.</returns>
    public bool ContainsLandmarkType(LandMarkType landmarkType)
    {
        for (int i = 0; i < interestPoints.Count; i++)
        {
            if (interestPoints[i].LandmarkType == landmarkType)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Get the number of time an InterestPoint is contained in the area.
    /// </summary>
    /// <param name="interestType">The type of Interest Point we search.</param>
    /// <returns>The number of Interest Point contained.</returns>
    public int GetNumberInterestType(InterestPointType interestType)
    {
        int toReturn = 0;
        for(int i = 0; i < interestPoints.Count; i++)
        {
            if(interestPoints[i].PointType == interestType)
            {
                toReturn++;
            }
        }
        return toReturn;
    }

    /// <summary>
    /// Set the sound level of the area.
    /// </summary>
    /// <param name="soundLevel">The sound level to set.</param>
    public void SetSoundLevel(float soundLevel)
    {
        noiseScore = soundLevel;
        for (int i = 0; i < displays.Count; i++)
        {
            displays[i].SetNatureLevel(soundLevel);
        }
    }

    /// <summary>
    /// Remove a visitor from the VisitorInZone list.
    /// </summary>
    /// <param name="toRemove">The visitor to remove.</param>
    public void RemoveVisitor(VisitorAgentBehave toRemove)
    {
        visitorInZone.Remove(toRemove);
    }
}
