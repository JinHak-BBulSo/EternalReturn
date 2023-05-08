using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

    private static FocusedItem focusedItem;

    private ItemStat item;
    private Image slotImage;
    private Image itemImage;

    private void Awake()
    {
        if (focusedItem == null)
        {
            //focusedItem = transform.root.GetChild(3).GetChild(3).GetComponent<FocusedItem>();
            focusedItem = GameObject.Find("Focused Item").GetComponent<FocusedItem>();
        }

        slotImage = GetComponent<Image>();
        itemImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void UpdateNewItem(ItemStat newItem)
    {
        item = newItem;

        slotImage.sprite = UIResources.ItemBgSpritesRO[item.rare];
        itemImage.sprite = ItemManager.Instance.itemListObj[item.id].GetComponent<SpriteRenderer>().sprite;
    }

    public void OnClickButton()
    {
        //Debug.Log($"Type: {item.type}");
        //Debug.Log(item?.name ?? "null");
        focusedItem.UpdateFocusedItem(item);
    }
}
