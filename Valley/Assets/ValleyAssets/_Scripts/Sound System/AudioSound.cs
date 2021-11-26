using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName ="AudioSound", menuName ="Create AudioSound")]
public class AudioSound : ScriptableObject
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioMixerGroup mixer;
    [SerializeField] private float loopTime = -1;

    public AudioClip GetClip => clips[Random.Range(0, clips.Length)];
    public AudioMixerGroup Mixer => mixer;
    public float LoopTime => loopTime;
}
