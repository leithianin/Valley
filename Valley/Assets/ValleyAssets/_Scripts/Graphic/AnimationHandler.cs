using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public AnimateFace face;
    private Animator body;

    private BodyAnimationType lastAnim;

    public bool isWalking;

    private void Awake()
    {
        body = GetComponent<Animator>();
    }

    void Start()
    {
        face.PlayIdle(Random.Range(1,3));
    }

    private void Update()
    {
        if (isWalking) body.SetBool("IsWalking", true);
    }

    #region Body
    public void PlayBodyAnim(BodyAnimationType animName)
    {
        if (lastAnim != animName)
        {
            StopBodyAnim(lastAnim);
            lastAnim = animName;

            switch (animName)
            {
                case BodyAnimationType.Idle:
                    body.SetBool("IsIdle", true);
                    break;

                case BodyAnimationType.Walk:
                    body.SetBool("IsWalking", true);
                    break;
            }
        }

    }

    public void StopBodyAnim(BodyAnimationType animName)
    {
        switch (animName)
        {
            case BodyAnimationType.Idle:
                body.SetBool("IsIdle", false);
                break;

            case BodyAnimationType.Walk:
                body.SetBool("IsWalking", false);
                break;
        }
    }
    #endregion

    #region Face
    public void PlayFaceAnim(ExpressionType animName)
    {
        switch (animName)
        {
            case ExpressionType.Idle_01:
                face.PlayIdle(1);
                break;

            case ExpressionType.Idle_02:
                face.PlayIdle(2);
                break;

            case ExpressionType.Shock_01:
                face.PlayOnce(3);
                break;

            case ExpressionType.Death_01:
                face.PlayOnce(4);
                break;

            case ExpressionType.Bored_01:
                face.PlayOnce(5);
                break;

            case ExpressionType.Happy_01:
                face.PlayOnce(6);
                break;

            case ExpressionType.Sad_01:
                face.PlayOnce(7);
                break;
        }
    }
    #endregion
}
