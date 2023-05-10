using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_W : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.playerState = PlayerController.PlayerState.Skill_W;
        playerController.ResetAni();
        playerController.ResetRange();
        playerController.player.Skill_W();
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
