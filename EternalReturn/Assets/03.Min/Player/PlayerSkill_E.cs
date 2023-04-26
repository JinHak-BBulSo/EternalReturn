using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_E : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.playerState = PlayerController.PlayerState.Skill_E;
        playerController.ResetAni();
        playerController.ResetRange();
        playerController.player.Skill_E();
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
