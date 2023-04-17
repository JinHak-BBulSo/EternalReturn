using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.player.playerAni.SetBool("isMove", true);
    }
    public void StateExit()
    {
        playerController.player.playerAni.SetBool("isMove", false);
    }

    public void StateFixedUpdate()
    {

    }

    public void StateUpdate()
    {
        playerController.player.Move();
        if (!playerController.player.isMove)
        {
            playerController.ChangeState(new PlayerIdle());
        }
    }
}



