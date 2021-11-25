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

        StartCoroutine(PlayLoop(.1f, 1f, 5f));
    }

    private void Update()
    {
       
    }

    IEnumerator PlayLoop(float frameRate, float delayMin, float delayMax)
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

        var textureOffset = new Vector2(1f / stripLength * frameCounter, 1f / stripHeight * (stripHeight - 1));
        face.mainTextureOffset = textureOffset;

        yield return new WaitForSeconds(delay);

        StartCoroutine(PlayLoop(.1f, 1f, 5f));
    }
}
