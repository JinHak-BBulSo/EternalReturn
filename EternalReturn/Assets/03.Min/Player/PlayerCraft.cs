using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCraft : IPlayerState
{
    private PlayerController playerController;
    private float time = 0f;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.playerState = PlayerController.PlayerState.CRAFT;
        playerController.ResetAni();
        playerController.ResetRange();
        playerController.player.weapon.SetActive(false);
        playerController.player.hammer.SetActive(true);
        playerController.player.craftTool.SetActive(true);
        playerController.player.playerAni.SetBool("isCraft", true);
        time = ItemManager.Instance.combineAbleList[0].craftTime;
    }
    public void StateExit()
    {
        playerController.toolReset();
        playerController.player.playerAni.SetBool("isCraft", false);
    }

    public void StateFixedUpdate()
    {

    }

    public void StateUpdate()
    {
        time -= Time.deltaTime;
        if (time <= 0f)
        {
            ItemManager.Instance.CombineItem(ItemManager.Instance.combineAbleList[0], ItemManager.Instance.itemCombineDictionary);
            switch (ItemManager.Instance.combineAbleList[0].rare)
            {
                case 1:
                    playerController.player.GetExp(100, PlayerStat.PlayerExpType.CRAFT);
                    break;
                case 2:
                    playerController.player.GetExp(200, PlayerStat.PlayerExpType.CRAFT);
                    break;
                case 3:
                    playerController.player.GetExp(350, PlayerStat.PlayerExpType.CRAFT);
                    break;
                default:
                    break;
            }
            if (!(ItemManager.Instance.itemInferiorList.Count < 1))
            {
                ItemManager.Instance.DeleteInferiorList(ItemManager.Instance.combineAbleList[0]);
            }

            ItemManager.Instance.InventoryChange();
            playerController.ChangeState(new PlayerIdle());
        }

        if (Input.GetMouseButton(1))
        {
            playerController.ChangeState(new PlayerIdle());
        }
        playerController.ShowAllRange();
        playerController.SkillUse();
    }
}
