using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesManager : MonoBehaviour
{
    private static RessourcesManager instance;

    private int woods = 20;

    [Header("X wood per Y seconds")]
    public float seconds = 2.5f;
    public int woodToGain = 5;

    public static int GetCurrentWoods => instance.woods;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(WoodConstant());
    }

    IEnumerator WoodConstant()
    {
        woods += woodToGain;
        UIManager.UpdateWood(woods);

        yield return new WaitForSeconds(seconds);
        StartCoroutine(WoodConstant());
    }

    public static void RemoveWood(int woodsToRemove)
    {
        instance.woods -= woodsToRemove;
        UIManager.UpdateWood(instance.woods);
    }
}
