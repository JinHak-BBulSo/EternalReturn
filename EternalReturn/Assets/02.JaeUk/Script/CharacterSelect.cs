using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class CharacterSelect : MonoBehaviourPun
{
    public GameObject player;
    public Image[] PlayerSprite = null;
    public Sprite[] ImageSprite;
    public Button startBtn;
    public bool isSelect;
    public int selectNumber;

    public int ReadyPlayerNum;


    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Instance.canvas = transform.gameObject;
        GameObject playerClone = PhotonNetwork.Instantiate("Global/Player", new Vector3(0, 0, 0), Quaternion.identity, 0);
        playerClone.transform.SetParent(transform.GetChild(0), false);

    }




    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance.SelectChk)
        {
            Debug.Log("!!!");
            PlayerManager.Instance.SelectChk = false;
            ReadyPlayerNum++;
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                if (transform.GetChild(0).GetChild(i).GetComponent<CharacterSelectController>().isSelect)
                    PlayerSprite[i].sprite = ImageSprite[transform.GetChild(0).GetChild(i).GetComponent<CharacterSelectController>().selectCharacterNum];
            }
        }




    }
    public void OnClick()
    {
        Debug.Log("!!");
        PlayerSprite[0].sprite = ImageSprite[0];
        selectNumber = 0;
        PlayerManager.Instance.IsSelect = true;
        PlayerManager.Instance.characterNum = 0;
    }
}

