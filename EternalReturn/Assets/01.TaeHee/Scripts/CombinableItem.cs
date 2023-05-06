using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CombinableItem : MonoBehaviour
{
    private Image bgImage;
    private Image itemImage;

    private void Awake()
    {
        bgImage = transform.GetChild(0).GetComponent<Image>();
        itemImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void UpdateItem(ItemStat item)
    {
        bgImage.sprite = UIResources.ItemBgSpritesRO[item.rare];
        itemImage.sprite = ItemManager.Instance.itemListObj[item.id].GetComponent<SpriteRenderer>().sprite;
    }
}