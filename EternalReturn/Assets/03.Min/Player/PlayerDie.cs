using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.playerState = PlayerController.PlayerState.DIE;
        playerController.ResetAni();
        playerController.ResetRange();
        playerController.player.weapon.SetActive(false);
        playerController.player.playerAni.SetBool("isDie", true);
        playerController.player.PlayAudio(PlayerBase.PlayerSound.DIE);
        PlayerList.Instance.playerCount--;
        // playerController.player.enabled = false;
        // playerController.enabled = false;
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
