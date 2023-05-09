using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoeFowUnit : FowUnit
{
    public bool isHidden = true;

    private List<Renderer> renderers = new List<Renderer>();
    private PlayerBase playerBase;
    private GameObject playerStatusUI;
    private Monster monster;
    private GameObject monsterStatusUI;

    private void Awake()
    {
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        playerBase = GetComponent<PlayerBase>();
        monster = GetComponent<Monster>();
    }

    private void Start()
    {
        if (playerBase != null)
        {
            playerStatusUI = playerBase.playerStatusUi.gameObject;
        }
        else if (monster != null)
        {
            monsterStatusUI = monster.monsterStatusUi;
        }
    }

    private void LateUpdate()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.forceRenderingOff = isHidden;
        }
        playerStatusUI?.SetActive(!isHidden);

        if (monster != null && monster.isDie)
        {
            isHidden = true;
            monsterStatusUI?.SetActive(!isHidden);
        }
        else
        {
            monsterStatusUI?.SetActive(!isHidden);
        }
    }

    private void OnEnable() => FowManager.AddFoeUnit(this);
    private void OnDisable() => FowManager.RemoveFoeUnit(this);
    private void OnDestroy() => FowManager.RemoveFoeUnit(this);
}