using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpLine : MonoBehaviour
{
    PlayerBase player = default;
    private List<GameObject> hpLineList = new List<GameObject>();
    
    void Start()
    {
        player = PlayerManager.Instance.Player.GetComponent<PlayerBase>();
        for(int i = 0; i < transform.childCount; i++)
        {
            hpLineList.Add(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHpBar()
    {
        float scaleX = 5 / (player.playerStat.maxHp / 100);
        foreach(var hpLine in hpLineList)
        {
            hpLine.transform.localScale = new Vector3(scaleX, 1, 1);
        }
    }
}
