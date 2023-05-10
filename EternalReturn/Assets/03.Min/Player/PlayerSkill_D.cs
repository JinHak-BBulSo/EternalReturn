using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_D : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.playerState = PlayerController.PlayerState.Skill_D;
        playerController.ResetAni();
        playerController.ResetRange();
        playerController.player.Skill_D();
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
