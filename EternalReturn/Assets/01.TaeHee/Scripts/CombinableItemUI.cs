using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinableItemUI : MonoBehaviour
{
    private List<CombinableItem> combinableItems = new List<CombinableItem>();

    private void Awake()
    {
        ItemManager.Instance.updateCombinableItemEvent.AddListener(UpdateCombinableItemUI);
        for (int i = 0; i < transform.childCount; i++)
        {
            combinableItems.Add(transform.GetChild(i).GetComponent<CombinableItem>());
        }
    }

    private void UpdateCombinableItemUI()
    {
        int combinableItemCount = Mathf.Min(ItemManager.Instance.combineAbleList.Count, transform.childCount);

        Debug.Log("Count" +  combinableItemCount);
        for (int i = 0; i < combinableItemCount; i++)
        {
            Debug.Log("comb" + combinableItems[i]);
            Debug.Log("combList" + ItemManager.Instance.combineAbleList[i]);
            combinableItems[i].UpdateItem(ItemManager.Instance.combineAbleList[i]);
            combinableItems[i].gameObject.SetActive(true);
        }

        for (int i = combinableItemCount; i < transform.childCount; i++)
        {
            combinableItems[i].gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        ItemManager.Instance.updateCombinableItemEvent.RemoveListener(UpdateCombinableItemUI);
    }
}
