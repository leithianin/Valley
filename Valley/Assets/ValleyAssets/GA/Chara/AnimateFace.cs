using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFace : MonoBehaviour
{
    private Material face;
    private Animator anim;

    private int frameCounter;
    private bool loopPlayed;

    public float stripLength = 5;
    public float stripHeight = 5;
    public float xStripOffset = 0;
    public float yStripOffset = 0;

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

        StartCoroutine(PlayIdle());
    }

    private void Update()
    {
        if (isWalking) anim.SetBool("IsWalking", true);

        else anim.SetBool("IsWalking", false);
    }


    IEnumerator PlayIdle()
    {
        var delay = 0f;
        var delayMin = 1f;
        var delayMax = 5f;
        var frameRate = .1f;

        //var idleType = 0;

        if (loopPlayed && frameCounter == 0) loopPlayed = !loopPlayed;

        if (!loopPlayed && frameCounter == animLength - 1) loopPlayed = !loopPlayed;

        if (loopPlayed)
        {
            frameCounter = 0;
            idleType = Random.Range(1, 3);
        }

        else
        {
            frameCounter++;
            delay = frameRate;
        }

        switch (idleType)
        {
            case 1:
                Debug.Log("Idle_01");
                Debug.Log(tabAnim[frameCounter]);
                Debug.Log(frameCounter);
                textureOffset = tabAnim[frameCounter];
                break;

            case 2:
                Debug.Log("Idle_02");
                Debug.Log(tabAnim[frameCounter + 3]);
                Debug.Log(frameCounter);
                textureOffset = tabAnim[frameCounter + 3];
                break;
        }

        face.mainTextureOffset = textureOffset;

        if (loopPlayed) delay = Random.Range(delayMin, delayMax);

        Debug.Log("Wait : " + delay);
        yield return new WaitForSeconds(delay);
        Debug.Log("End Wait : " + delay);

        StartCoroutine(PlayIdle());
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
