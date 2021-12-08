using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFace : MonoBehaviour
{
    private Material face;
    private Animator anim;

    private int frameCounter;
    private bool loopPlayed;
    private bool animPlayedOnce;

    public float stripLength, stripHeight = 5;
    public float xStripOffset, yStripOffset = 0;

    private int idleType;
    private int animType;

    private Vector2 textureOffset;

    private float animLength = 3;

    //public bool isWalking;

    public Vector2[] tabAnim = new Vector2[] { new Vector2(0.1f, 0.2f)};

    void Start()
    {
        face = GetComponent<SkinnedMeshRenderer>().material;
        anim = gameObject.GetComponentInParent<Animator>();

        var textureScale = new Vector2(1f / stripLength, 1f / stripHeight);
        face.mainTextureScale = textureScale;

        StartCoroutine(PlayIdle(Random.Range(1, 3)));
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

    public IEnumerator PlayOnce(int _animType)
    {
        var delay = 0f;
        var frameRate = .1f;

        delay = frameRate;

        animType = _animType;

        if (animPlayedOnce && frameCounter == 0) animPlayedOnce = !animPlayedOnce;

        if (!animPlayedOnce && frameCounter == animLength - 1) animPlayedOnce = !animPlayedOnce;

        if (animPlayedOnce)
        {
            frameCounter = 0;
            yield return new WaitForSeconds(delay);
            StartCoroutine(PlayIdle(Random.Range(1, 3)));
        }

        else
        {
            frameCounter++;
        }

        switch (animType)
        {
            case 3:
                textureOffset = tabAnim[frameCounter + 6];
                break;

            case 4:
                textureOffset = tabAnim[frameCounter + 9];
                break;

            case 5:
                textureOffset = tabAnim[frameCounter + 12];
                break;

            case 6:
                textureOffset = tabAnim[frameCounter + 15];
                break;

            case 7:
                textureOffset = tabAnim[frameCounter + 18];
                break;
        }

        yield return new WaitForSeconds(delay);
    }
}
