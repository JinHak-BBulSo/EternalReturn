using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    private GameObject mainUi = default;
    private GameObject playerStatusUi = default;
    public Image playerHpBar = default;
    public Image playerMpBar = default;
    public Image playerExpBar = default;
    public Text playerLevelTxt = default;
    public PlayerBase player = default;
    private Vector3 offset = new Vector3(0, 2.5f, 0);

    private void Start()
    {
        mainUi = GameObject.Find("TestUi");
        playerStatusUi = this.gameObject;
        //player = PlayerManager.Instance.Player.GetComponent<PlayerBase>();

        playerHpBar = transform.GetChild(1).GetComponent<Image>();
        playerMpBar = transform.GetChild(3).GetComponent<Image>();
        playerExpBar = mainUi.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<Image>();
        playerLevelTxt = transform.GetChild(4).GetChild(0).GetComponent<Text>();
        playerHpBar.fillAmount = player.playerStat.nowHp / player.playerStat.maxHp;
    }

    private void Update()
    {
        transform.position = player.transform.position + offset;
        playerHpBar.fillAmount = player.playerStat.nowHp / player.playerStat.maxHp;
    }
}
