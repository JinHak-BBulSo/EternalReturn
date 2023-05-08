using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.AI;

public class MiniMapCamera : MonoBehaviour
{
    private Camera miniMapCamera = default;

    private bool isExtended = false;

    private void Start()
    {
        miniMapCamera = transform.GetChild(0).GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isExtended)
        {
            if (miniMapCamera.gameObject.activeSelf)
            {
                miniMapCamera.gameObject.SetActive(false);
            }
            else
            {
                miniMapCamera.gameObject.SetActive(true);
            }
        }
    }
}
