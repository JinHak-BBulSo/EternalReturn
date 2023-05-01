using UnityEngine;
using UnityEngine.UI;

public class ItemTypeButton : MonoBehaviour
{
    private static WishListSetting wishListSetting;

    [SerializeField] private ItemType itemType;
    private Image buttonImage;

    public void OnClickButton()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetComponent<Image>().color = Color.white;
        }

        buttonImage.color = Color.gray;
        wishListSetting.SetFocusedItemType(itemType);
    }

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        if (itemType == ItemType.All)
        {
            buttonImage.color = Color.gray;
        }

        if (wishListSetting == null)
        {
            wishListSetting = transform.parent.parent.GetComponent<WishListSetting>();
        }

    }
}