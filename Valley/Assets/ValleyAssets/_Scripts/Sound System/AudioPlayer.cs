using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSound sound;

    private Coroutine loopCoroutine;

    /// <summary>
    /// Play the current sound.
    /// </summary>
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

    /// <summary>
    /// Play a new sound.
    /// </summary>
    /// <param name="newSound">The new sound to play.</param>
    public void Play(AudioSound newSound)
    {
        sound = newSound;
        Play();
    }

    /// <summary>
    /// Play a new sound with a different settings.
    /// </summary>
    /// <param name="newSound">The new sound to play.</param>
    /// <param name="newSource">The new parameters.</param>
    public void Play(AudioSound newSound, AudioSource newSource)
    {
        source.outputAudioMixerGroup = newSource.outputAudioMixerGroup;
        Play(newSound);
    }

    /// <summary>
    /// Stop the sound.
    /// </summary>
    public void Stop()
    {
        StopCoroutine(loopCoroutine);
        loopCoroutine = null;
    }

    public void End()
    {

    }

    /// <summary>
    /// Make the sound loop after a specified time.
    /// </summary>
    /// <param name="loopTime">The time to wait before looping the sound.</param>
    /// <returns></returns>
    IEnumerator LoopSound(float loopTime)
    {
        yield return new WaitForSeconds(loopTime);
        Play();
    }
}
