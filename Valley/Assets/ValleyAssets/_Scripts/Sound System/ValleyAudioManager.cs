using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleyAudioManager : MonoBehaviour
{
    private static ValleyAudioManager instance;

    [SerializeField] private AudioSound music;
    [SerializeField] private AudioPlayer musicPlayer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        musicPlayer.Play(music);
    }

    public static void SetMainMusic(AudioSound newMusic)
    {
        instance.OnSetMainMusic(newMusic);
    }

    private void OnSetMainMusic(AudioSound newMusic)
    {
        music = newMusic;
        musicPlayer.Play(music);
    }
}
