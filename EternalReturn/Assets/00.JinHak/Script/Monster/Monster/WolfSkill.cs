using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSkill : MonoBehaviour
{
    public List<MonsterWolf> targetWolfs = new List<MonsterWolf>();

    private void Start()
    {
        transform.parent.parent.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        targetWolfs.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<MonsterWolf>() != default)
        {
            MonsterWolf nowTargetWolf = other.GetComponent<MonsterWolf>();
            if (!targetWolfs.Contains(other.GetComponent<MonsterWolf>()))
            {
                targetWolfs.Add(other.GetComponent<MonsterWolf>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MonsterWolf>() != default)
        {
            MonsterWolf nowTargetWolf = other.GetComponent<MonsterWolf>();
            if (targetWolfs.Contains(other.GetComponent<MonsterWolf>()))
            {
                targetWolfs.Remove(other.GetComponent<MonsterWolf>());
            }
        }
    }
}
