using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DBF_ChangeColor : MonoBehaviour
{
    [SerializeField] private Color color;
    private Color baseColor = new Color(255, 255, 255, 255);
    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    public void ChangeColor()
    {
        Debug.Log("ChangeColor");
        img.color = color;
    }

    public void ResetColor()
    {
        Debug.Log("ResetColor");
        img.color = baseColor;    
    }
}
