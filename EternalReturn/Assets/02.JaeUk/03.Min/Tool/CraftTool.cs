using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftTool : MonoBehaviour
{
    public PlayerBase craftPlayer = default;

    private void HammerOn()
    {
        craftPlayer.hammer.SetActive(true);
    }

    private void DriverOn()
    {
        craftPlayer.hammer.SetActive(false);
        craftPlayer.driver.SetActive(true);
    }
}
