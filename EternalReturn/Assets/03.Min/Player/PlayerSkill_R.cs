using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_R : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.playerState = PlayerController.PlayerState.Skill_R;
        playerController.ResetAni();
        playerController.ResetRange();
        playerController.player.Skill_R();

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
