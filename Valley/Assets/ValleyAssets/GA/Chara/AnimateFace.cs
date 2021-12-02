using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFace : MonoBehaviour
{
    private Material face;
    private Animator anim;

    private float frameCounter;
    private bool loopPlayed;

    public float stripLength = 5;
    public float stripHeight = 5;
    public float xStripOffset = 0;
    public float yStripOffset = 0;

    private Vector2 textureOffset;

    private float animLength = 3;

    public bool isWalking;

    void Start()
    {
        face = GetComponent<SkinnedMeshRenderer>().material;
        anim = gameObject.GetComponentInParent<Animator>();

        var textureScale = new Vector2(1f / stripLength, 1f / stripHeight);
        face.mainTextureScale = textureScale;

        StartCoroutine(PlayLoop("Idle_01", .1f, 1f, 5f));
    }

    private void Update()
    {
        if (isWalking) anim.SetBool("IsWalking", true);

        else anim.SetBool("IsWalking", false);
    }

    IEnumerator PlayLoop(string name, float frameRate, float delayMin, float delayMax)
    {
        var delay = 0.1f;

        if (loopPlayed && frameCounter == 0) loopPlayed = !loopPlayed;

        if (!loopPlayed && frameCounter == animLength - 1) loopPlayed = !loopPlayed;

        if (loopPlayed)
        {
            frameCounter = 0;
            delay = Random.Range(delayMin, delayMax);
        }

        else
        { 
            frameCounter++;
            delay = frameRate;
        }

        switch (name)
        {
            case "Idle_01":
                textureOffset = new Vector2(1f / stripLength * frameCounter, 1f / stripHeight * (stripHeight - 1));
                break;

            case "Idle_02":
                
                break;
        }

        face.mainTextureOffset = textureOffset;

        yield return new WaitForSeconds(delay);

        StartCoroutine(PlayLoop(name, .1f, 1f, 5f));
    }
}
