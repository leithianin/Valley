using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName ="AudioSound", menuName ="Create AudioSound")]
public class AudioSound : ScriptableObject
{
    [SerializeField, Tooltip("A list of all the sound that can be played. Each time the AudioSound is played, it takes a random clip from here.")] private AudioClip[] clips;
    [SerializeField, Tooltip("The miwer used for this sound.")] private AudioMixerGroup mixer;
    [SerializeField, Tooltip("The time before the sound loops. Put it less than zero make the sound not looping.")] private float loopTime = -1;

    public AudioClip GetClip => clips[Random.Range(0, clips.Length)];
    public AudioMixerGroup Mixer => mixer;
    public float LoopTime => loopTime;
}
