using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaDisplay : MonoBehaviour
{
    [SerializeField] protected float natureLevel;

    public abstract void SetNatureLevel(float level);
}
