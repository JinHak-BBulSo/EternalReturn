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
       if(PlayerList.Instance.playerCount == 1)
        {
            gameEndTimer += Time.deltaTime;
            if(gameEndTimer > 0.5f && !isGameEnd)
            {
                if(PlayerManager.Instance.Player.GetComponent<PlayerBase>().PlayerController.playerState != PlayerController.PlayerState.DIE)
                {
                    gameEndUi[0].SetActive(true);
                }
                else
                {
                    gameEndUi[1].SetActive(true);
                }
            }
        }
        else
        {
            gameEndTimer = 0;
        }
    }
}
