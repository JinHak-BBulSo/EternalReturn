using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    public enum PlayerState
    {
        NONE = -1,
        IDLE,
        MOVE,
        ATTACKMOVE,
        ATTACK,
        DIE,
        Skill_Q,
        Skill_W,
        Skill_E,
        Skill_R,
        Skill_D,
        COLLECT

    }
    public PlayerBase player = default;
    public IPlayerState currentState = new PlayerIdle();
    public PlayerState playerState = PlayerState.NONE;

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
        player.playerAni.SetBool("isCollect", false);
    }

    public void toolReset()
    {
        player.weapon.SetActive(true);
        player.fishingRod.SetActive(false);
    }
}

