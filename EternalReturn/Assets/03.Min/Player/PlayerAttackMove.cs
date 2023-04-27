using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackMove : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.playerState = PlayerController.PlayerState.ATTACKMOVE;

        // playerController.player.playerAni.Rebind();
        playerController.ResetRange();
        playerController.player.playerAni.SetBool("isAttack", false);
        playerController.player.playerAni.SetBool("isSkill", false);
        playerController.player.playerAni.SetBool("skillStart", false);
        // playerController.player.playerAni.SetBool("isMove", false);
        if (playerController.player.playerAni.GetBool("isMove"))
        {
            playerController.player.playerAni.SetBool("isMove", true);
        }
        else
        {
            playerController.player.playerAni.SetBool("isMove", true);
        }
    }

    public void StateExit()
    {
        // playerController.player.playerAni.SetBool("isMove", false);
        playerController.player.isMove = false;
        playerController.player.isAttackMove = false;
    }

    public void StateFixedUpdate()
    {
    }

    public void StateUpdate()
    {
        Collider[] enemys = Physics.OverlapSphere(playerController.transform.position, playerController.player.playerStat.attackRange);

        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].CompareTag("Enemy"))
            {
                playerController.player.enemy = enemys[i].gameObject;
                break;
            }
        }
        if (playerController.player.enemy != null)
        {
            if (ExceptY.ExceptYDistance(ExceptY.ExceptYPos(playerController.player.transform.position), ExceptY.ExceptYPos(playerController.player.enemy.transform.position))
            <= playerController.player.playerTotalStat.attackRange)
            {
                if (playerController.player.isAttackAble)
                {
                    playerController.ChangeState(new PlayerAttack());
                }
            }
        }
        playerController.player.Move();


        if (!playerController.player.skillCooltimes[0])
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                playerController.ChangeState(new PlayerSkill_Q());
            }
        }

        if (!playerController.player.skillCooltimes[1])
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                playerController.ChangeState(new PlayerSkill_W());
            }
        }

        if (!playerController.player.skillCooltimes[2])
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                playerController.ChangeState(new PlayerSkill_E());
            }
        }

        if (!playerController.player.skillCooltimes[3])
        {

            if (Input.GetKeyDown(KeyCode.R))
            {
                playerController.ChangeState(new PlayerSkill_R());
            }
        }

        if (!playerController.player.skillCooltimes[4])
        {

            if (Input.GetKeyDown(KeyCode.D))
            {
                playerController.ChangeState(new PlayerSkill_D());
            }
        }
        // if (!playerController.player.isAttackMove)
        // {
        //     Debug.Log("이거 실행됨?");
        //     playerController.ChangeState(new PlayerIdle());
        // }
    }
}