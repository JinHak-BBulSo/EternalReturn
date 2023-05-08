using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbiddenArea : MonoBehaviour
{
    public int areaIndex = -1;
    private void Start()
    {
        this.enabled = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == PlayerManager.Instance.Player)
        {
            other.GetComponent<PlayerBase>().isInForbiddenArea = true;
            PlayerManager.Instance.areaIndex = areaIndex;
        }
    }
}
