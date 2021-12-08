using System;
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

    private Action<int> onUpdateWood;

    public static int GetCurrentWoods => instance.woods;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        onUpdateWood += UIManager.UpdateWood;
        StartCoroutine(WoodConstant());
    }

    IEnumerator WoodConstant()
    {
        woods += woodToGain;
        onUpdateWood?.Invoke(woods);

        yield return new WaitForSeconds(seconds);
        StartCoroutine(WoodConstant());
    }

    public static void RemoveWood(int woodsToRemove)
    {
        instance.woods -= woodsToRemove;
        instance.onUpdateWood?.Invoke(instance.woods);
    }
}
