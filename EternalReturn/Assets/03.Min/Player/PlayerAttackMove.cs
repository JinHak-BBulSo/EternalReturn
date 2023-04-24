using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackMove : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.player.playerAni.Rebind();
        playerController.ResetRange();
        playerController.player.playerAni.SetBool("isMove", true);
    }

    public void StateExit()
    {
        playerController.player.playerAni.SetBool("isMove", false);
        playerController.player.isMove = false;
        playerController.player.isAttackMove = false;
    }

    public void StateFixedUpdate()
    {
    }

    public void StateUpdate()
    {
        Collider[] enemys = Physics.OverlapSphere(playerController.transform.position, playerController.player.playerStat.attackRange);
        playerController.player.Move();
        if (!playerController.player.isAttackMove)
        {
            playerController.ChangeState(new PlayerIdle());
        }
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].CompareTag("Enemy"))
            {
                playerController.player.enemy = enemys[i].gameObject;
                break;
            }
        }

        if (playerController.player.enemy != null && playerController.player.isAttackAble)
        {
            playerController.ChangeState(new PlayerAttack());
        }
    }
}