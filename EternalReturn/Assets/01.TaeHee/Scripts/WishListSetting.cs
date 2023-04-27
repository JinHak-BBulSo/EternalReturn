using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WishListSetting : MonoBehaviour
{
    List<string> itemNameList = new List<string>();
    InputField inputField;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTextChanged()
    {
        int stringLength = int.MaxValue;
        int currentIndex = 0;

        foreach (var itemName in itemNameList)
        {
            stringLength = itemName.Length;
            foreach (var nameChar in itemName)
            {

                ++currentIndex;
            }
        }
    }
}
