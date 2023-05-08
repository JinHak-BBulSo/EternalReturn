using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    AudioSource audioSource = default;
    private bool isGameStart = false;
    public AudioClip[] allBgms = new AudioClip[15];
    public int nowAreaIndex = -1;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = allBgms[PlayerManager.Instance.areaIndex];
        nowAreaIndex = PlayerManager.Instance.areaIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerManager.Instance.IsGameStart && !isGameStart)
        {
            isGameStart = true;
            audioSource.Play();
        }

        if(nowAreaIndex != PlayerManager.Instance.areaIndex)
        {
            nowAreaIndex = PlayerManager.Instance.areaIndex;
            audioSource.clip = allBgms[nowAreaIndex];
            audioSource.Play();
        }
    }
}
