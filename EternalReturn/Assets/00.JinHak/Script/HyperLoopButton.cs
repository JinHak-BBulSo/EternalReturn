using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HyperLoopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public HyperLoopPointList hyperLoopPointList = default;
    public HyperLoopPoint hyperLoopPoint = default;
    public int index = -1;
    private Image image = default;
    private GameObject hyperLoopUi = default;
    private PlayerBase targetPlayer = default;

    private void Start()
    {
        image = transform.parent.GetComponent<Image>();
        hyperLoopUi = transform.parent.parent.parent.gameObject;
        hyperLoopPointList = hyperLoopUi.GetComponent<HyperLoopUi>().hyperLoopPointList;
        hyperLoopPoint = hyperLoopPointList.hyperLoopPoints[index];
    }
    public void OnClickHyperLoop()
    {
        if(targetPlayer == default)
        {
            targetPlayer = PlayerManager.Instance.Player.GetComponent<PlayerBase>();
        }

        int random_ = Random.Range(0, hyperLoopPoint.telePoints.Length);
        targetPlayer.playerNav.enabled = false;
        targetPlayer.transform.position = hyperLoopPoint.telePoints[random_].transform.position;
        targetPlayer.playerNav.enabled = true;
        image.color = new Color(255, 255, 255, 0);
        hyperLoopUi.SetActive(false);
    }

    public void OnClickSetPlayerPos()
    {
        int random_ = Random.Range(0, hyperLoopPoint.telePoints.Length);
        PlayerManager.Instance.PlayerPos = hyperLoopPoint.telePoints[random_].transform.position;
        image.color = new Color(255, 255, 255, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(255, 255, 255, 130);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color(255, 255, 255, 0);
    }
}
