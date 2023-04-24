using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.AI;

public class MiniMapCamera : MonoBehaviour
{
    private Camera miniMapCamera = default;

    private void Start()
    {
        miniMapCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(miniMapCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Camera.main.transform.position = hit.point;
            }
        }
    }
}
