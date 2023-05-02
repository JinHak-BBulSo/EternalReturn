using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperLoopUi : MonoBehaviour
{
    public HyperLoopPointList hyperLoopPointList = default;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
}
