using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public PlayerBase player = default;

    void Update()
    {
        if (player != default)
        {
            transform.position = player.transform.position;
        }
        else
        {
            /* do nothing */
        }
    }
}
