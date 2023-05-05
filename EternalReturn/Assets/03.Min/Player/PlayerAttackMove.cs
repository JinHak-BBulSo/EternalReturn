using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerAttackMove : IPlayerState
{
    private PlayerController playerController;
    public void StateEnter(PlayerController controller_)
    {
        this.playerController = controller_;
        playerController.playerState = PlayerController.PlayerState.ATTACKMOVE;

        // playerController.player.playerAni.Rebind();
        playerController.ResetRange();
        playerController.player.playerAni.SetBool("isAttack", false);
        playerController.player.playerAni.SetBool("isSkill", false);
        playerController.player.playerAni.SetBool("skillStart", false);
        // playerController.player.playerAni.SetBool("isMove", false);
        if (playerController.player.playerAni.GetBool("isMove"))
        {
            playerController.player.playerAni.SetBool("isMove", true);
            playerController.player.photonView.RPC("SetAnimationBool", Photon.Pun.RpcTarget.All, "isMove", true);
        }
        else
        {
            playerController.player.playerAni.SetBool("isMove", true);
        }
    }


    public void StateExit()
    {
        // playerController.player.playerAni.SetBool("isMove", false);
        playerController.player.isMove = false;
        playerController.player.isAttackMove = false;
    }

    public void StateFixedUpdate()
    {
    }

    public void StateUpdate()
    {
        Collider[] enemys = Physics.OverlapSphere(playerController.transform.position, playerController.player.playerTotalStat.attackRange);

        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].CompareTag("Enemy") && !enemys[i].gameObject.GetComponent<Monster>().isDie)
            {
                playerController.player.enemy = enemys[i].gameObject;
                break;
            }
        }
        if (playerController.player.enemy != null)
        {
            if (ExceptY.ExceptYDistance(ExceptY.ExceptYPos(playerController.player.transform.position), ExceptY.ExceptYPos(playerController.player.enemy.transform.position))
            <= playerController.player.playerTotalStat.attackRange)
            {
                if (playerController.player.isAttackAble)
                {
                    playerController.ChangeState(new PlayerAttack());
                }
                else
                {
                    NavMeshHit navHit;
                    PlayerBase player_ = playerController.player;
                    if (NavMesh.SamplePosition(player_.enemy.transform.position, out navHit, 5.0f, NavMesh.AllAreas))
                    {
                        player_.SetDestination(new Vector3(navHit.position.x, navHit.position.y, navHit.position.z));

                        player_.path = new NavMeshPath();
                        player_.playerNav.CalculatePath(player_.Destination, player_.path);
                        playerController.player.corners.Clear();
                        for (int i = 0; i < player_.path.corners.Length; i++)
                        {
                            player_.corners.Add(player_.path.corners[i]);
                        }
                        player_.currentCorner = 0;
                    }
                }
            }
        }
        playerController.player.Move();


        if (!playerController.player.skillCooltimes[0])
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                playerController.ChangeState(new PlayerSkill_Q());
            }
        }

        if (!playerController.player.skillCooltimes[1])
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                playerController.ChangeState(new PlayerSkill_W());
            }
        }

        if (!playerController.player.skillCooltimes[2])
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                playerController.ChangeState(new PlayerSkill_E());
            }
        }

        if (!playerController.player.skillCooltimes[3])
        {

            if (Input.GetKeyDown(KeyCode.R))
            {
                playerController.ChangeState(new PlayerSkill_R());
            }
        }

        if (!playerController.player.skillCooltimes[4])
        {

            if (Input.GetKeyDown(KeyCode.D))
            {
                playerController.ChangeState(new PlayerSkill_D());
            }
        }
        playerController.Craft();
        // if (!playerController.player.isAttackMove)
        // {
        //     Debug.Log("이거 실행됨?");
        //     playerController.ChangeState(new PlayerIdle());
        // }
    }
}