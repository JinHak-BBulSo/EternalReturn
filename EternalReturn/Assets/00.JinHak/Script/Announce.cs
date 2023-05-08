using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Announce : MonoBehaviour
{
    public AudioClip[] allAnnounce = new AudioClip[3];
    public AudioSource announceAudio = default;
    private bool isGameStart = false;
    void Start()
    {
        announceAudio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerManager.Instance.IsGameStart && !isGameStart)
        {
            isGameStart = true;

            announceAudio.clip = allAnnounce[0];
            announceAudio.Play();
        }
    }
}
