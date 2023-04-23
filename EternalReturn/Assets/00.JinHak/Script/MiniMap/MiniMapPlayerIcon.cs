using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapPlayerIcon : MonoBehaviour
{
    GameObject targetPlayer = default;
    void Start()
    {
        targetPlayer = transform.parent.GetChild(0).gameObject;
    }
    void Update()
    {
        transform.position = ExceptY.ExceptYPos(targetPlayer.transform.position);
    }
}
