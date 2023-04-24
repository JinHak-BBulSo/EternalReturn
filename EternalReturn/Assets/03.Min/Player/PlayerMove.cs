using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        // playerController.player.playerAni.Rebind();
        playerController.ResetRange();
        playerController.player.playerAni.SetBool("isAttack", false);
        playerController.player.playerAni.SetBool("isSkill", false);
        playerController.player.playerAni.SetBool("skillStart", false);
        if (!playerController.player.playerAni.GetBool("isMove"))
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


        if (!playerController.player.isMove)
        {
            playerController.ChangeState(new PlayerIdle());
        }

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
    }
}



