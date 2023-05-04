using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.playerState = PlayerController.PlayerState.IDLE;
    }
    public void StateExit()
    {
    }

    public void StateFixedUpdate()
    {
    }

    public void StateUpdate()
    {
        if (playerController.player.isMove && !playerController.player.isAttackMove)
        {
            playerController.ChangeState(new PlayerMove());
        }
        else if (playerController.player.isAttackMove)
        {
            playerController.ChangeState(new PlayerAttackMove());
        }
        playerController.Craft();
        playerController.ShowAllRange();
        playerController.SkillUse();

    }
}
