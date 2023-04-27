using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Q : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.playerState = PlayerController.PlayerState.Skill_Q;
        playerController.ResetAni();
        playerController.ResetRange();
        if (playerController.player.SkillRange[0] == null)
        {
            playerController.player.Skill_Q();
        }
        else
        {
            playerController.player.SkillRange[0].SetActive(true);
        }
    }
    public void StateExit()
    {
    }

    public void StateFixedUpdate()
    {
    }

    public void StateUpdate()
    {
        if (playerController.player.SkillRange[0].activeSelf)
        {

        }
    }
}
