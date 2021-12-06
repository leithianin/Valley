using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DBF_ChangeSprite : MonoBehaviour
{
    [SerializeField] private Sprite selectedSprite;                   //New Image
    private Image img;
    private Sprite baseSprite;                                        //Old Image

    private void Start()
    {
        img = GetComponent<Image>();
        baseSprite = img.sprite;
    }

    public void ChangeSprite()
    {
        img.sprite = selectedSprite;
    }

    public void ReturnSprite()
    {
        img.sprite = baseSprite;
    }
}
