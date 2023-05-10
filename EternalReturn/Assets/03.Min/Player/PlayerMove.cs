using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.playerState = PlayerController.PlayerState.MOVE;
        playerController.ResetRange();
        playerController.player.playerAni.SetBool("isAttack", false);
        playerController.player.playerAni.SetBool("isSkill", false);
        playerController.player.playerAni.SetBool("skillStart", false);
        if (Random.Range(0, 11) == 1)
        {
            playerController.player.PlayAudio(PlayerBase.PlayerSound.MOVE);
        }
        if (!playerController.player.playerAni.GetBool("isMove"))
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
    }

    public void StateFixedUpdate()
    {

    }

    public void StateUpdate()
    {

        playerController.player.Move();
        if (playerController.player.isAttackMove)
        {
            playerController.ChangeState(new PlayerAttackMove());
        }

        if (playerController.player.clickTarget != null)
        {
            if (playerController.player.clickTarget.GetComponent<GatheringItemBox>() != null)
            {
                if (ExceptY.ExceptYDistance(playerController.transform.position, playerController.player.clickTarget.transform.position) <= 1f)
                {
                    playerController.ChangeState(new PlayerCollect());
                }
            }
        }

        if (!playerController.player.isMove)
        {
            playerController.ChangeState(new PlayerIdle());
        }
        playerController.Craft();
        playerController.ShowAllRange();
        playerController.SkillUse();
        playerController.Rest();


    }
}



