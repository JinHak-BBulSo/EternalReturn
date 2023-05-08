using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbiddenArea : MonoBehaviour
{
    private void Start()
    {
        this.enabled = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == PlayerManager.Instance.Player)
        {
            other.GetComponent<PlayerBase>().isInForbiddenArea = true;
        }
    }
}
