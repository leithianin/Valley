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

    public void Start()
    {
        initialPos = transform.position;
    }

    public void OnMove()
    {
        if (transform.position == initialPos)               //Si c'est à sa place initiale = Je dois le cacher
        {
            rt.DOMove(move.position, timeFeedback);
        }
        else                                                //Sinon, il n'est pas à sa place initiale, il veut y retourner ! 
        {
            rt.DOMove(initialPos, timeFeedback);
        }
    }
}
