using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Q : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.player.Skill_Q();
    }
    public void StateExit()
    {
        playerController.player.playerAni.SetBool("isSkill", false);
        if (playerController.player.Skill_Q_Range != null)
        {
            playerController.player.Skill_Q_Range.SetActive(false);
        }
    }

    public void StateFixedUpdate()
    {
    }

    public void StateUpdate()
    {

    }
}
