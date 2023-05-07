using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinableItem : MonoBehaviour
{
    private static PlayerController playerController;

    private int index;
    private Image bgImage;
    private Image itemImage;

    private void Awake()
    {
        //if (playerController == null)
        //{
        //    playerController = PlayerManager.Instance.Player.GetComponent<PlayerController>();
        //}

        index = transform.GetSiblingIndex();
        bgImage = transform.GetChild(0).GetComponent<Image>();
        itemImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        //gameObject.SetActive(false);
    }

    public void UpdateItem(ItemStat item)
    {
        bgImage.sprite = UIResources.ItemBgSpritesRO[item.rare];
        itemImage.sprite = ItemManager.Instance.itemListObj[item.id].GetComponent<SpriteRenderer>().sprite;
    }

    public void OnClickButton()
    {
        if (playerController == null)
        {
            playerController = PlayerManager.Instance.Player.GetComponent<PlayerController>();
        }

        ItemManager.Instance.combineAbleList[0] = ItemManager.Instance.combineAbleList[index];
        playerController.ChangeState(new PlayerCraft());
    }
}