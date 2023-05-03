using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperLoopPoint : MonoBehaviour
{
    public GameObject[] telePoints = new GameObject[3];

    private void Awake()
    {
        for(int i = 0; i < telePoints.Length; i++)
        {
            telePoints[i] = transform.GetChild(i).gameObject;
        }
    }
}
