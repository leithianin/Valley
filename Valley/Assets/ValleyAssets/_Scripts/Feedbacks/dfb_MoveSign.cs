using System;
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

    [SerializeField] private AnimationCurve curve;

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

            StartCoroutine(MoveObject(moveVector, timeFeedback, () => UIManager.GetOnMovingToolCompleted?.Invoke()));

            /*rt.DOMove(moveVector, timeFeedback).OnComplete(() =>
            {
                UIManager.GetOnMovingToolCompleted?.Invoke();
            });*/

            isActive = false;
        }
        else                                                //Sinon, il n'est pas à sa place initiale, il veut y retourner ! 
        {
            UIManager.GetOnMovingTool?.Invoke();

            StartCoroutine(MoveObject(initialPos, timeFeedback, () => UIManager.GetOnMovingToolCompleted?.Invoke()));

            /*rt.DOMove(initialPos, timeFeedback).OnComplete(() =>
            {
                UIManager.GetOnMovingToolCompleted?.Invoke();
            });*/

            isActive = true;
        }

    }

    IEnumerator MoveObject(Vector3 endPosition, float travelTime, Action callback)
    {
        float timeTraveled = 0;

        Vector3 startPosition = rt.position;
        Vector3 direction = endPosition - startPosition;

        while(timeTraveled < travelTime)
        {
            timeTraveled += Time.deltaTime;

            if(timeTraveled > travelTime)
            {
                timeTraveled = travelTime;
            }

            rt.position = startPosition + (direction * curve.Evaluate(timeTraveled / travelTime));

            yield return new WaitForSeconds(Time.deltaTime);
        }

        callback?.Invoke();
    }
}
