using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DFB_MoveUI : MonoBehaviour
{
    [SerializeField] private RectTransform rtTarget;
    private RectTransform rt;
    private Vector3 originPos;

    public float timeFeedback;

    public void Start()
    {
        rt = GetComponent<RectTransform>();
        originPos = rt.position;
    }

    public void MoveToTarget()
    {
        rt.DOMove(rtTarget.position, timeFeedback);
    }

    public void MoveToOrigin()
    {
        rt.DOMove(originPos, timeFeedback);
    }
}
