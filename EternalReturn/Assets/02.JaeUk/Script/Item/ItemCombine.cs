using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemCombine : MonoBehaviour
{
    public GameObject player;
    public GameObject ItemCanvas;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ItemManager.Instance.InventoryChange();
            ItemManager.Instance.func(ItemManager.Instance.itemList, 1);

        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (ItemManager.Instance.combineAbleList.Count != 0)
            {

                ItemManager.Instance.CombineItem(ItemManager.Instance.combineAbleList[0], ItemManager.Instance.itemCombineDictionary);
                ItemManager.Instance.DeleteInferiorList(ItemManager.Instance.combineAbleList[0]);
                ItemManager.Instance.combineAbleList.RemoveAt(0);
                ItemManager.Instance.InventoryChange();

            }

        }
    }
    // 인벤토리에 변동 사항이 있을 경우 호출 되는 메서드

}
