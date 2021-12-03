using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private AnimateFace face;
    private Animator body;

    private BodyAnimationType lastAnim;

    private void Awake()
    {
        face = transform.GetChild(0).GetComponent<AnimateFace>();
        body = GetComponent<Animator>();
    }

    void Start()
    {
        face.PlayIdle(Random.Range(1,3));
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
        }
    }
    #endregion
}
