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





    }
    void OnClick()
    {
        PlayerSprite[0].sprite = ImageSprite[0];
        selectNumber = 0;
        isSelect = true;
    }
}

