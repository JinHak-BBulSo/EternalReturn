using UnityEngine;
using UnityEngine.UI;

public class ItemTypeButton : MonoBehaviour
{
    private static WishListSetting wishListSetting;

    [SerializeField] private ItemType itemType;
    private Text buttonText;

    public void OnClickButton()
    {
        wishListSetting.SetFocusedItemType(itemType);
    }

    private void Awake()
    {
        if (wishListSetting == null)
        {
            wishListSetting = transform.parent.parent.GetComponent<WishListSetting>();
        }

        //buttonText = transform.GetChild(0).GetComponent<Text>();
        //switch (buttonText.text)
        //{
        //    case "옷":
        //        itemType = ItemType.Chest;
        //        break;
        //    case "머리":
        //        itemType = ItemType.Head;
        //        break;
        //    case "팔":
        //        itemType = ItemType.Arm;
        //        break;
        //    case "다리":
        //        itemType = ItemType.Leg;
        //        break;
        //    case "장식":
        //        itemType = ItemType.Accessory;
        //        break;
        //    default:
        //        itemType = ItemType.All;
        //        break;
        //}
    }
}