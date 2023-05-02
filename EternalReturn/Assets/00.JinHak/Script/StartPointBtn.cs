using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPointBtn : MonoBehaviour
{
    public HyperLoopPointList hyperLoopPointList = default;
    public HyperLoop hyperLoop = default;
    public int index = -1;
    private Image image = default;
    private GameObject hyperLoopUi = default;

    private void Start()
    {
        image = transform.parent.GetComponent<Image>();
        hyperLoopUi = transform.parent.parent.parent.gameObject;
        hyperLoopPointList = hyperLoopUi.GetComponent<HyperLoopUi>().hyperLoopPointList;
        hyperLoop = hyperLoopPointList.hyperLoops[index];
    }
}
