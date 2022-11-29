using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip FoodSound;
    public AudioClip DestructiveSound;
    public AudioClip Background;

    [Min(0)]
    public float Volume;

    private AudioSource Audio;

    private void Awake()
    {
        Audio = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        Audio.Play();
    }

    public void TakeFoodAudio()
    {
        Audio.PlayOneShot(FoodSound, Volume);
    }

    public void DestroyBlock()
    {
        Audio.PlayOneShot(DestructiveSound);
    }

    private void OnEnable()
    {
        Audio.PlayOneShot(Background);
    }

    private void OnDisable()
    {
        Audio.Stop();
    }
    private void OnDestroy()
    {
        Audio.Stop();
    }
}

