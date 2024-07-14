using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip doorSound;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip gameOverSound;
    
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    public void PlayCorrectSound()
    {
        audioSource.PlayOneShot(correctSound);
    }

    public void PlayWrongSound()
    {
        audioSource.PlayOneShot(wrongSound);
    }

    public void PlayDoorSound()
    {
        audioSource.PlayOneShot(doorSound);
    }

    public void PlayGameOverSound()
    {
        audioSource.PlayOneShot(gameOverSound);
    }

}
