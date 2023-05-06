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
        playerController.player.castingBar.transform.parent.gameObject.SetActive(true);
        playerController.player.weapon.SetActive(false);
        playerController.player.hammer.SetActive(true);
        playerController.player.craftTool.SetActive(true);
        playerController.player.playerAni.SetBool("isCraft", true);
        time = ItemManager.Instance.combineAbleList[0].craftTime;
    }
    public void StateExit()
    {
        playerController.toolReset();
        playerController.player.castingBar.fillAmount = 0;
        playerController.player.castingBar.transform.parent.gameObject.SetActive(false);
        playerController.player.playerAni.SetBool("isCraft", false);
    }

    public void StateFixedUpdate()
    {

    }

    public void StateUpdate()
    {
        time -= Time.deltaTime;
        playerController.player.castingBar.fillAmount = ItemManager.Instance.combineAbleList[0].craftTime - time / ItemManager.Instance.combineAbleList[0].craftTime;
        if (time <= 0f)
        {
            ItemManager.Instance.CombineItem(ItemManager.Instance.combineAbleList[0], ItemManager.Instance.itemCombineDictionary);
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
