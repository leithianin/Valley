using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaDisplay : MonoBehaviour
{
    [SerializeField] protected float natureLevel;

    /// <summary>
    /// Change the nature level of the display. (Currently only set the noise score).
    /// </summary>
    /// <param name="level">The nature level to set.</param>
    public abstract void SetNatureLevel(float level);
}
