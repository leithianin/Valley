using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFace : MonoBehaviour
{
    private Material face;
    private Animator anim;

    private int frameCounter;
    private bool loopPlayed;

    public float stripLength, stripHeight = 5;
    public float xStripOffset, yStripOffset = 0;

    private int idleType;

    private Vector2 textureOffset;

    private float animLength = 3;

    public bool isWalking;

    public Vector2[] tabAnim = new Vector2[] { new Vector2(0.1f, 0.2f)};

    void Start()
    {
        face = GetComponent<SkinnedMeshRenderer>().material;
        anim = gameObject.GetComponentInParent<Animator>();

        var textureScale = new Vector2(1f / stripLength, 1f / stripHeight);
        face.mainTextureScale = textureScale;

        StartCoroutine(PlayIdle(Random.Range(1, 3)));
    }

    private void Update()
    {
        if (isWalking) anim.SetBool("IsWalking", true);

        else anim.SetBool("IsWalking", false);
    }


    public IEnumerator PlayIdle(int _idleType)
    {
        var delay = 0f;
        var delayMin = 1f;
        var delayMax = 5f;
        var frameRate = .1f;

        idleType = _idleType;

        if (loopPlayed && frameCounter == 0) loopPlayed = !loopPlayed;

        if (!loopPlayed && frameCounter == animLength - 1) loopPlayed = !loopPlayed;

        if (loopPlayed)
        {
            frameCounter = 0;
        }

        else
        {
            frameCounter++;
            delay = frameRate;
        }

        switch (idleType)
        {
            case 1:
                textureOffset = tabAnim[frameCounter];
                break;

            case 2:
                textureOffset = tabAnim[frameCounter + 3];
                break;
        }

        face.mainTextureOffset = textureOffset;

        if (loopPlayed) delay = Random.Range(delayMin, delayMax);

        yield return new WaitForSeconds(delay);

        StartCoroutine(PlayIdle(Random.Range(1, 3)));
    }

    IEnumerator PlayLoop(string _name, float frameRate, float delayMin, float delayMax)
    {
        var delay = 0f;

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
                textureOffset = tabAnim[frameCounter];
                break;

            case "Idle_02":
                
                break;
        }

        face.mainTextureOffset = textureOffset;

        yield return new WaitForSeconds(delay);

        StartCoroutine(PlayLoop(_name, .1f, 1f, 5f));
    }
}
