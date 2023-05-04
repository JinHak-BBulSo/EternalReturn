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
            playerController.player.photonView.RPC("SetAnimationBool", Photon.Pun.RpcTarget.All, "isMove", true);
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
        if (playerController.player.photonView.IsMine)
        {
            Collider[] enemys = Physics.OverlapSphere(playerController.transform.position, playerController.player.playerTotalStat.attackRange);

            for (int i = 0; i < enemys.Length; i++)
            {
                if (enemys[i].CompareTag("Enemy"))
                {
                    playerController.player.enemy = enemys[i].gameObject;
                    break;
                }
                if (enemys[i].gameObject != playerController.gameObject && enemys[i].CompareTag("Player"))
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
        }
        playerController.Craft();
        playerController.ShowAllRange();
        playerController.SkillUse();

    }
}