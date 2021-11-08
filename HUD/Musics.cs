using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musics : MonoBehaviour
{
    public bool randomPlay;
    public AudioClip[] clips;
    private AudioSource audioSource;
    AudioClip lastClip;
    int clipOrder = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        randomPlay = true;
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (randomPlay == true)
            {
                audioSource.clip = GetRandomClip();
                audioSource.Play();
            }
            else
            {
                audioSource.clip = GetNextClip();
                audioSource.Play();
            }
        }
    }

    private AudioClip GetRandomClip()
    {
        //return clips[Random.Range(0, clips.Length)];
        int attempts = 9;
        AudioClip newClip = clips[Random.Range(0, clips.Length)];
        while (newClip == lastClip && attempts > 0)
        {
            newClip = clips[Random.Range(0, clips.Length)];
            attempts--;
        }
        lastClip = newClip;

        return newClip;
    }

    private AudioClip GetNextClip()
    {
        if (clipOrder >= clips.Length - 1)
        {
            clipOrder = 0;
        }
        else
        {
            clipOrder += 1;
        }
        return clips[clipOrder];
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
