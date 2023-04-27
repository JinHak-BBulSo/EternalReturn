using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private static FocusedItem focusedItem;

    private ItemStat item = null;
    private RectTransform rectTransform;
    private Image slotImage;
    private Image itemImage;

    private void Awake()
    {
        if (focusedItem == null)
        {
            focusedItem = transform.root.GetChild(3).GetChild(3).GetComponent<FocusedItem>();
        }

        rectTransform = GetComponent<RectTransform>();
        slotImage = GetComponent<Image>();
        itemImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void UpdateNewItem(ItemStat newItem, float yPos)
    {
        item = newItem;

        slotImage.sprite = WishListSetting.ItemBgSpritesRO[item.rare];
        itemImage.sprite = ItemManager.Instance.itemListObj[item.id].GetComponent<SpriteRenderer>().sprite;

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPos);
    }

    public void OnClickButton()
    {
        //Debug.Log(item?.name ?? "null");
        focusedItem.UpdateFocusedItem(item);
    }
}
