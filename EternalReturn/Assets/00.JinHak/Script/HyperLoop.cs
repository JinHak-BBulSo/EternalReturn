using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperLoop : MonoBehaviour
{
    public GameObject[] telePoints = new GameObject[3];
    public PlayerBase player;

    private void Awake()
    {
        for(int i = 0; i < telePoints.Length; i++)
        {
            telePoints[i] = transform.GetChild(i).gameObject;
        }
        player = transform.parent.GetComponent<HyperLoopPointList>().player;
    }

    private void Update()
    {
        if(player == default)
        {
            player = PlayerManager.Instance.Player.GetComponent<PlayerBase>();
        }
    }
}
