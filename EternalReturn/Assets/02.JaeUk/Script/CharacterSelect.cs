using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CharacterSelect : MonoBehaviourPun
{
    public GameObject player;
    public Image[] PlayerSprite;
    public Sprite[] ImageSprite;
    public Sprite[] BgSprite;
    public Button startBtn;
    public GameObject selectPlayerBg;
    public bool isSelect;
    public int selectNumber;
    public Image gage;
    public Text timer;
    public int ReadyPlayerNum;
    public float totalTime;


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
        totalTime += Time.deltaTime;
        timer.text = $"{55 - (int)totalTime}";
        gage.fillAmount = totalTime / 55f;
        if (totalTime >= 55f)
        {
            SceneManager.LoadScene("LumiaIslandScene");
        }
    }
    public void SelectChar(int i)
    {
        PlayerSprite[0].sprite = ImageSprite[i - 1];
        selectPlayerBg.transform.GetChild(0).GetComponent<Image>().sprite = BgSprite[i - 1];
        selectNumber = i - 1;
        PlayerManager.Instance.characterNum = selectNumber;
    }
    public void OnClick()
    {
        Debug.Log("!!");
        PlayerManager.Instance.IsSelect = true;

    }
}

