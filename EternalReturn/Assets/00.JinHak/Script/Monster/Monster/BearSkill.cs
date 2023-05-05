using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearSkill : MonoBehaviour
{
    public List<PlayerBase> targetPlayers = new List<PlayerBase>();

    private void Start()
    {
        transform.parent.parent.parent.GetComponent<MonsterBear>().bearSkillMesh = this;
        transform.parent.parent.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        targetPlayers.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerBase nowTargetPlayer_ = other.GetComponent<PlayerBase>();
            if (!targetPlayers.Contains(other.GetComponent<PlayerBase>()))
            {
                targetPlayers.Add(nowTargetPlayer_);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerBase nowTargetPlayer_ = other.GetComponent<PlayerBase>();
            if (targetPlayers.Contains(other.GetComponent<PlayerBase>()))
            {
                targetPlayers.Remove(nowTargetPlayer_);
            }
        }
    }
}
