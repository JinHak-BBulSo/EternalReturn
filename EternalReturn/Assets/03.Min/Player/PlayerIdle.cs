using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
    }
    public void StateExit()
    {
    }

    public void StateFixedUpdate()
    {
    }

    public void StateUpdate()
    {
        if (playerController.player.isMove)
        {
            playerController.ChangeState(new PlayerMove());
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
