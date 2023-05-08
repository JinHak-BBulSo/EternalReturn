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
    private Vector3 offset = new Vector3(0, 3f, 0);

    private void Start()
    {
        mainUi = GameObject.Find("Main UI Canvas");
        playerStatusUi = this.gameObject;
        //player = PlayerManager.Instance.Player.GetComponent<PlayerBase>();

        playerHpBar = transform.GetChild(1).GetComponent<Image>();
        playerMpBar = transform.GetChild(3).GetComponent<Image>();
        playerExpBar = mainUi.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<Image>();
        playerLevelTxt = transform.GetChild(4).GetChild(0).GetComponent<Text>();
        playerHpBar.fillAmount = player.playerStat.nowHp / player.playerStat.maxHp;

        if(player != PlayerManager.Instance.Player.GetComponent<PlayerBase>())
        {
            playerHpBar.color = new Color(1, 0, 0, 1);

            Material foeMaterial = new Material(Shader.Find("UI/Default"));
            foeMaterial.renderQueue = transform.GetChild(0).GetComponent<Image>().material.renderQueue - 50;
            foreach (var img in GetComponentsInChildren<Image>())
            {
                img.material = foeMaterial;
            }
            playerLevelTxt.material = foeMaterial;
        }
    }

    private void Update()
    {
        if (player != default)
        {
            transform.position = player.transform.position + offset;
            playerHpBar.fillAmount = player.playerStat.nowHp / player.playerStat.maxHp;
        }
    }
}
