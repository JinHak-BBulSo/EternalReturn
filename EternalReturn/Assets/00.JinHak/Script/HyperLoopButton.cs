using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HyperLoopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public HyperLoopPointList hyperLoopPointList = default;
    public HyperLoop hyperLoop = default;
    public int index = -1;
    private Image image = default;
    private GameObject hyperLoopUi = default;

    private void Start()
    {
        hyperLoop = hyperLoopPointList.hyperLoops[index];
        image = GetComponent<Image>();
        hyperLoopUi = transform.parent.parent.gameObject;
    }
    public void OnClickHyperLoop()
    {
        int random_ = Random.Range(0, hyperLoop.telePoints.Length);
        hyperLoop.player.playerNav.enabled = false;
        hyperLoop.player.transform.position = hyperLoop.telePoints[random_].transform.position;
        hyperLoop.player.playerNav.enabled = true;
        image.color = new Color(255, 255, 255, 0);
        hyperLoopUi.SetActive(false);
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
