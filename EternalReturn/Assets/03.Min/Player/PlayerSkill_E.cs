using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_E : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.ResetAni();
        playerController.ResetRange();
        playerController.transform.LookAt(controller_.player.nowMousePoint);
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
