using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class PlayerController : MonoBehaviourPun
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
        COLLECT,
        CRAFT

    }
    public PlayerBase player = default;
    public IPlayerState currentState = new PlayerIdle();
    public PlayerState playerState = PlayerState.NONE;
    public GameObject useSkillRange = default;

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
        if (!photonView.IsMine) { return; }
        for (int i = 0; i < player.SkillRange.Length; i++)
        {
            if (player.SkillRange[i] != null)
            {
                player.SkillRange[i].SetActive(false);
            }
        }
        player.ExtraRange();
    }

    public void ResetAni()
    {
        if (!photonView.IsMine) { return; }
        player.playerAni.SetBool("isMove", false);
        player.playerAni.SetBool("isAttack", false);
        player.playerAni.SetBool("isSkill", false);
        player.playerAni.SetBool("skillStart", false);
        player.playerAni.SetBool("isCollect", false);
        player.playerAni.SetBool("isCraft", false);
        player.playerAni.SetBool("isRest", false);
        player.ExtraAni();
    }

    public void toolReset()
    {
        if (!photonView.IsMine) { return; }
        player.weapon.SetActive(true);
        player.fishingRod.SetActive(false);
        player.craftTool.SetActive(false);
        player.hammer.SetActive(false);
        player.driver.SetActive(false);
    }

    public void ShowAllRange()
    {
        if (!photonView.IsMine) { return; }
        if (Input.GetMouseButtonDown(0))
        {
        }
        else if (Input.anyKeyDown)
        {
            ResetRange();
        }
        ShowRange(0, KeyCode.Q);
        ShowRange(1, KeyCode.W);
        ShowRange(2, KeyCode.E);
        ShowRange(3, KeyCode.R);
        ShowRange(4, KeyCode.D);
    }
    private void ShowRange(int index_, KeyCode inputKey_)
    {
        if (!photonView.IsMine) { return; }
        if (!player.skillCooltimes[index_] && player.skillSystem.skillInfos[index_].CurrentLevel != 0)
        {
            if (Input.GetKeyDown(inputKey_))
            {
                ResetRange();
                if (player.SkillRange[index_] != null)
                {
                    player.SkillRange[index_].SetActive(true);
                }
                else
                {
                    switch (inputKey_)
                    {
                        case KeyCode.Q:
                            ChangeState(new PlayerSkill_Q());
                            break;
                        case KeyCode.W:
                            ChangeState(new PlayerSkill_W());
                            break;
                        case KeyCode.E:
                            ChangeState(new PlayerSkill_E());
                            break;
                        case KeyCode.R:
                            ChangeState(new PlayerSkill_R());
                            break;
                        case KeyCode.D:
                            ChangeState(new PlayerSkill_D());
                            break;
                    }
                }
            }
        }
    }
    public void Rest()
    {
        if (!photonView.IsMine) { return; }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ChangeState(new PlayerRest());
        }
    }
    public void Craft()
    {
        if (!photonView.IsMine) { return; }
        if (Input.GetKeyDown(KeyCode.Z) && ItemManager.Instance.combineAbleList.Count > 0)
        {
            ChangeState(new PlayerCraft());
        }
    }
    public void SkillUse()
    {
        if (!photonView.IsMine) { return; }
        for (int i = 0; i < player.SkillRange.Length; i++)
        {
            if (player.SkillRange[i] != null && player.SkillRange[i].activeSelf)
            {
                useSkillRange = player.SkillRange[i];
                break;
            }
            else if (player.SkillRange[i] != null && !player.SkillRange[i].activeSelf)
            {
                useSkillRange = default;
            }
        }
        if (useSkillRange != null)
        {
            Vector3 distance = ExceptY.ExceptYPos(player.nowMousePoint) - ExceptY.ExceptYPos(useSkillRange.transform.position);
            Vector3 dir = Vector3.Normalize(distance);
            useSkillRange.transform.forward = dir;
            useSkillRange.transform.rotation *= Quaternion.Euler(0, 0, -90);
        }

        if (Input.GetMouseButtonDown(0) && useSkillRange != null)
        {
            if (useSkillRange == player.SkillRange[0])
            {
                ChangeState(new PlayerSkill_Q());
            }
            else if (useSkillRange == player.SkillRange[1])
            {
                ChangeState(new PlayerSkill_W());
            }
            else if (useSkillRange == player.SkillRange[2])
            {
                ChangeState(new PlayerSkill_E());
            }
            else if (useSkillRange == player.SkillRange[3])
            {
                ChangeState(new PlayerSkill_R());
            }
            else if (useSkillRange == player.SkillRange[4])
            {
                ChangeState(new PlayerSkill_D());
            }
        }

    }

}

