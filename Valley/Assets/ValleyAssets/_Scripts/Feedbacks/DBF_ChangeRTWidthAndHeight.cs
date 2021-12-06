using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBF_ChangeRTWidthAndHeight : MonoBehaviour
{
    [SerializeField] private Vector2 newSize;
    private RectTransform rt;
    private Vector2 baseSize;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        baseSize = rt.sizeDelta;
    }

    public void ChangeWidthAndHeight()
    {
        rt.sizeDelta = newSize;
    }

    public void ResetWidthAndHeight()
    {
        rt.sizeDelta = baseSize;
    }
}
