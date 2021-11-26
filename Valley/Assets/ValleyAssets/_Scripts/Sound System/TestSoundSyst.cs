using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundSyst : MonoBehaviour
{
    [SerializeField] private AudioPlayer player;
    [SerializeField] private AudioSound sound;
    [SerializeField] private AudioSound music;

    [ContextMenu("Set sound")]
    public void PlaySound()
    {
        player.Play(sound);
    }

    [ContextMenu("Set music")]
    public void ChangeMusic()
    {
        ValleyAudioManager.SetMainMusic(music);
    }
}
