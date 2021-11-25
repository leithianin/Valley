using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFace : MonoBehaviour
{
    private Material face; 

    private float frameCounter;
    private bool loopPlayed;

    public float stripLength = 5;
    public float stripHeight = 5;
    public float xStripOffset = 0;
    public float yStripOffset = 0;

    private float animLength = 3;

    void Start()
    {
        face = GetComponent<SkinnedMeshRenderer>().material;

        var textureScale = new Vector2(1f / stripLength, 1f / stripHeight);
        face.mainTextureScale = textureScale;

        StartCoroutine(PlayLoop(0.1f));
    }

    private void Update()
    {
        /*var textureOffset = new Vector2(xStripOffset, yStripOffset);
        face.mainTextureOffset = textureOffset;*/
    }

    IEnumerator PlayLoop(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (loopPlayed && frameCounter == 0) loopPlayed = !loopPlayed;

        if (!loopPlayed && frameCounter == animLength - 1) loopPlayed = !loopPlayed;

        if (loopPlayed) frameCounter = 0;

        else frameCounter++;

        var textureOffset = new Vector2(1f / stripLength * frameCounter, 1f / stripHeight * (stripHeight - 1));
        face.mainTextureOffset = textureOffset;

        StartCoroutine(PlayLoop(0.1f));
    }
}
