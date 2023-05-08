using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapPlayerIcon : MonoBehaviour
{
    private GameObject targetPlayer = default;
    private Vector3 offset = default;
    private SpriteRenderer iconSprite = default;
    void Start()
    {
        targetPlayer = transform.parent.GetChild(0).gameObject;
        offset = new Vector3(0f, 0.4f, -0f);
        iconSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        transform.rotation = Quaternion.identity;
        if (PlayerManager.Instance.Player != default && Vector3.Distance(transform.position, PlayerManager.Instance.Player.transform.position) > 8)
        {
            iconSprite.color = new Color(1, 1, 1, 0);
        }
        else
        {
            iconSprite.color = new Color(1, 1, 1, 1);
        }
    }
}
