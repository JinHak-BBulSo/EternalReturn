using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HyperLoopObj : MonoBehaviour
{
    [SerializeField]
    private GameObject hyperLoopUi = default;
    Outline outline = default;

    void Start()
    {
        outline = GetComponent<Outline>();
    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && this.enabled)
        {
            PlayerBase nowContactPlayer = other.GetComponent<PlayerBase>();

            if (nowContactPlayer.clickTarget == this.gameObject && PlayerManager.Instance.Player == other.gameObject)
            {
                hyperLoopUi.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && this.enabled)
        {
            PlayerBase nowContactPlayer = other.GetComponent<PlayerBase>();

            if (nowContactPlayer.clickTarget == this.gameObject && PlayerManager.Instance.Player == other.gameObject)
            {
                hyperLoopUi.SetActive(false);
            }
        }
    }

    private void OnMouseEnter()
    {
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        if (!outline.isClick)
        {
            outline.enabled = false;
        }
    }
}
