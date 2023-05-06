using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : IPlayerState
{
    private GatheringItemBox gatheringItem = default;
    private PlayerController playerController;
    private float gatheringTime = 0f;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.playerState = PlayerController.PlayerState.COLLECT;
        gatheringItem = playerController.player.clickTarget.GetComponent<GatheringItemBox>();
        playerController.ResetAni();
        playerController.ResetRange();
        if (gatheringItem.gatherAble)
        {
            playerController.player.weapon.SetActive(false);
            switch (gatheringItem.itemType)
            {
                case GatheringItemBox.GatherItemType.PEBBLE:
                    playerController.player.playerAni.SetBool("isCollect", true);
                    playerController.player.playerAni.SetFloat("CollectType", 0f);
                    break;
                case GatheringItemBox.GatherItemType.WATERPOINT:
                    playerController.player.playerAni.SetBool("isCollect", true);
                    playerController.player.playerAni.SetFloat("CollectType", 1f);
                    break;
                case GatheringItemBox.GatherItemType.FISHING:
                    playerController.player.playerAni.SetBool("isCollect", true);
                    playerController.player.playerAni.SetFloat("CollectType", 2f);
                    playerController.player.fishingRod.SetActive(true);
                    break;
                default:
                    playerController.player.playerAni.SetBool("isCollect", true);
                    playerController.player.playerAni.SetFloat("CollectType", 3f);
                    break;
            }
        }
        else
        {
            playerController.player.isMove = false;
            playerController.ChangeState(new PlayerIdle());
        }
    }
    public void StateExit()
    {
        playerController.player.playerAni.SetBool("isCollect", false);
        gatheringItem = default;
        playerController.toolReset();
    }

    public void StateFixedUpdate()
    {

    }

    public void StateUpdate()
    {
        if (gatheringItem != null)
        {
            if (gatheringItem.gatherAble)
            {
                gatheringTime += Time.deltaTime;
                if (gatheringTime >= gatheringItem.GatherTime)
                {
                    gatheringItem.GetItem();
                    playerController.player.GetExp(80, PlayerStat.PlayerExpType.SEARCH);
                    playerController.player.clickTarget = default;
                    playerController.player.isMove = false;
                    playerController.ChangeState(new PlayerIdle());
                }
            }
            else
            {
                playerController.player.isMove = false;
                playerController.ChangeState(new PlayerIdle());
            }
        }
        else
        {
            playerController.player.isMove = false;
            playerController.ChangeState(new PlayerIdle());
        }
        if (Input.GetMouseButton(1))
        {
            playerController.ChangeState(new PlayerIdle());
        }
    }
}
