using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSound sound;

    private Coroutine loopCoroutine;

    public void Play()
    {
        if (loopCoroutine != null)
        {
            Stop();
        }

        AudioClip toPlay = sound.GetClip;

        if (toPlay != null)
        {
            source.PlayOneShot(toPlay);
            if (sound.LoopTime >= 0)
            {
                loopCoroutine = StartCoroutine(LoopSound(toPlay.length + sound.LoopTime));
            }
        }
    }

    public void Play(AudioSound newSound)
    {
        sound = newSound;
        Play();
    }

    public void Play(AudioSound newSound, AudioSource newSource)
    {
        source.outputAudioMixerGroup = newSource.outputAudioMixerGroup;
        Play(newSound);
    }

    public void Stop()
    {
        StopCoroutine(loopCoroutine);
        loopCoroutine = null;
    }

    public void End()
    {

    }

    IEnumerator LoopSound(float loopTime)
    {
        yield return new WaitForSeconds(loopTime);
        Play();
    }
}
