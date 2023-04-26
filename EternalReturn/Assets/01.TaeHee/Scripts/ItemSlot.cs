using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    private ItemStat item = null;
    private RectTransform rectTransform;
    private Image slotImage;
    private Image itemImage;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        slotImage = GetComponent<Image>();
        itemImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetNewItem(ItemStat newItem, float yPos)
    {
        item = newItem;

        Debug.Log(WishListSetting.itemBgSpritesRO.Count);
        slotImage.sprite = WishListSetting.itemBgSpritesRO[item.rare];
        itemImage.sprite = ItemManager.Instance.itemListObj[item.id].GetComponent<SpriteRenderer>().sprite;

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPos);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(item.name);
    }
}
