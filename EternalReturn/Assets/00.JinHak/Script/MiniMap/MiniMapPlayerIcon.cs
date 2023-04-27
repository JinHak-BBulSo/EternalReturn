using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapPlayerIcon : MonoBehaviour
{
    private GameObject targetPlayer = default;
    private Vector3 offset = default;
    void Start()
    {
        targetPlayer = transform.parent.GetChild(0).gameObject;
        offset = new Vector3(0f, 0.4f, -0f);
    }
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
