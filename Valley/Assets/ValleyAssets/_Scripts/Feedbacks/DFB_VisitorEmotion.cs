using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DFB_VisitorEmotion : MonoBehaviour
{
    [SerializeField] private Transform toMakeJump;
    [SerializeField] private Transform startPosition;
    [SerializeField] private float jumpHeight;

    Tween jumpTween;

    public void PlayFeedback()
    {
        toMakeJump.gameObject.SetActive(true);
        jumpTween = toMakeJump.DOLocalJump(startPosition.localPosition + new Vector3(0, jumpHeight, 0), 1f, 1, 1f).OnComplete(EndFeedback);
    }

    private void EndFeedback()
    {
        toMakeJump.gameObject.SetActive(false);
    }
}
