using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.ResetAni();
        playerController.ResetRange();
        controller_.player.Attack();
    }

    public void StateExit()
    {
    }

    public void StateFixedUpdate()
    {
    }

    public void StateUpdate()
    {
    }
}
