using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] gameEndUi = new GameObject[2];
    public Sprite[] characterSprite = new Sprite[2];
    private bool isGameEnd = false;
    private float gameEndTimer = 0;
    public Announce announce = default;
    public void Start()
    {
        if(PlayerManager.Instance.characterNum == 0)
        {
            foreach(GameObject go in gameEndUi)
            {
                go.transform.GetChild(2).GetComponent<Image>().sprite = characterSprite[0];
            }
        }
        else
        {
            foreach (GameObject go in gameEndUi)
            {
                go.transform.GetChild(2).GetComponent<Image>().sprite = characterSprite[1];
            }
        }
    }
    void Update()
    {
       if(PlayerList.Instance.playerCount == 1 && PlayerList.Instance.playerDictionary.Count == 2)
        {
            if(!isGameEnd)
            {
                isGameEnd = true;
                if(PlayerManager.Instance.Player.GetComponent<PlayerBase>().PlayerController.playerState != PlayerController.PlayerState.DIE)
                {
                    announce.announceAudio.clip = announce.allAnnounce[1];
                    announce.announceAudio.Play();
                    gameEndUi[0].SetActive(true);
                }
                else
                {
                    announce.announceAudio.clip = announce.allAnnounce[3];
                    announce.announceAudio.Play();
                    gameEndUi[1].SetActive(true);
                }
            }
        }
    }
}
