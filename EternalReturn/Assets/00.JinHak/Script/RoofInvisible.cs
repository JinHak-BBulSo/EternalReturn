using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofInvisible : MonoBehaviour
{
    [SerializeField]
    private GameObject[] roofObjects = default;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance.Player)
        {
            foreach(var roof in roofObjects)
            {
                roof.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == PlayerManager.Instance.Player)
        {
            foreach (var roof in roofObjects)
            {
                roof.SetActive(true);
            }
        }
    }
}
