using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRest : IPlayerState
{
    private PlayerController playerController;
    private float time = 0f;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.playerState = PlayerController.PlayerState.REST;
        playerController.ResetAni();
        playerController.ResetRange();
        playerController.player.playerAni.SetBool("isRest", true);
    }
    public void StateExit()
    {
        playerController.player.isMove = false;
    }

    public void StateFixedUpdate()
    {

    }

    public void StateUpdate()
    {
        if (time >= 0.5f)
        {
            time = 0f;
            playerController.player.playerStat.nowHp += playerController.player.playerStat.maxHp * 0.1f;
            if (playerController.player.playerStat.nowHp >= playerController.player.playerStat.maxHp)
            {
                playerController.player.playerStat.nowHp = playerController.player.playerStat.maxHp;
            }
            playerController.player.playerStat.nowStamina += playerController.player.playerStat.maxStamina * 0.1f;
            if (playerController.player.playerStat.nowStamina >= playerController.player.playerStat.maxStamina)
            {
                playerController.player.playerStat.nowStamina = playerController.player.playerStat.maxStamina;
            }
        }
        time += Time.deltaTime;

        if (Input.GetMouseButtonDown(1))
        {
            playerController.player.playerAni.SetBool("isRest", false);
        }
    }
}
