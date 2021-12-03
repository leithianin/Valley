using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    private AnimateFace face;
    private Animator body;

    private void Awake()
    {
        face = transform.GetChild(0).GetComponent<AnimateFace>();
        body = GetComponent<Animator>();
    }

    void Start()
    {
        face.PlayIdle();
    }

    #region Body
    public void PlayBodyAnim(BodyAnimationType animName)
    {
        switch(animName)
        {
            case BodyAnimationType.Idle:
                body.SetBool("IsIdle", true);
                break;

            case BodyAnimationType.Walk:
                body.SetBool("IsWalking", true);
                break;
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
    public void PlayFaceIdle()
    {
        face.PlayIdle();
    }
    #endregion
}
