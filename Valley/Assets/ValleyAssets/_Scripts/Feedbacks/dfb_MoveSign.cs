using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

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
            UIManager.GetOnMovingTool?.Invoke();
            Vector3 moveVector = move.position;
            rt.DOMove(moveVector, timeFeedback).OnComplete(() =>
            {
                UIManager.GetOnMovingToolCompleted?.Invoke();
            });
            isActive = false;
        }
        else                                                //Sinon, il n'est pas à sa place initiale, il veut y retourner ! 
        {
            UIManager.GetOnMovingTool?.Invoke();
            rt.DOMove(initialPos, timeFeedback).OnComplete(() =>
            {
                UIManager.GetOnMovingToolCompleted?.Invoke();
            });
            isActive = true;
        }

    }
}
