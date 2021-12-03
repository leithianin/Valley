using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleLinkManager : MonoBehaviour
{
    public static VisibleLinkManager instance;

    private LineRenderer currentLine;

    public static LineRenderer CurrentLine => instance.currentLine;

    private void Awake()
    {
        instance = this;
    }

    public static void SetLine(LineRenderer nLine)
    {
        instance.currentLine = nLine;
    }

    public static void DestroyLine()
    {
        Destroy(instance.currentLine.gameObject);
    }

    public static float GetLineLength()
    {
        return instance.OnGetLineLength();
    }

    private float OnGetLineLength()
    {
        float distance = 0;
        if (currentLine != null)
        {
            for (int i = 0; i < currentLine.positionCount - 1; i++)
            {
                distance += Vector3.Distance(currentLine.GetPosition(i), currentLine.GetPosition(i + 1));
            }
        }
        return distance;
    }
}
