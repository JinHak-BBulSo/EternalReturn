using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyArea : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance.Player)
        {
            other.GetComponent<PlayerBase>().isInForbiddenArea = false;
        }
    }
}
