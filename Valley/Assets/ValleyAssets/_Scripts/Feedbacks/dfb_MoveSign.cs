using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class dfb_MoveSign : MonoBehaviour
{
    public RectTransform rt;
    public RectTransform move;
    public float timeFeedback;
    private Vector3 initialPos;

    private bool isActive = true;

    public void Start()
    {
        initialPos = transform.position;
    }

    public void OnMove()
    {
        if (isActive)                                         //Si c'est à sa place initiale = Je dois le cacher
        {
            Vector3 moveVector = move.position;
            rt.DOMove(moveVector, timeFeedback);
            isActive = false;
        }
        else                                                //Sinon, il n'est pas à sa place initiale, il veut y retourner ! 
        {
            rt.DOMove(initialPos, timeFeedback);
            isActive = true;
        }
    }
}
