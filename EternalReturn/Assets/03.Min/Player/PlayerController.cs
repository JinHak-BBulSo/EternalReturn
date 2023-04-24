using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public PlayerBase player = default;
    public IPlayerState currentState = new PlayerIdle();

    public void Start()
    {
        Initialize();
    }
    public void ChangeState(IPlayerState newState)
    {
        currentState.StateExit();
        currentState = newState;
        currentState.StateEnter(this);
    }

    private void Update()
    {
        currentState.StateUpdate();
    }

    private void FixedUpdate()
    {
        currentState.StateFixedUpdate();
    }

    public void Initialize()
    {
        currentState.StateEnter(this);
        player = this.gameObject.GetComponent<PlayerBase>();
    }

    public void ResetRange()
    {
        for (int i = 0; i < player.SkillRange.Length; i++)
        {
            player.SkillRange[i].SetActive(false);
        }
    }

    public void ResetAni()
    {
        player.playerAni.SetBool("isMove", false);
        player.playerAni.SetBool("isAttack", false);
        player.playerAni.SetBool("isSkill", false);
        player.playerAni.SetBool("skillStart", false);
    }
}

