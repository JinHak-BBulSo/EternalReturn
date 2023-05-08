using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.AI;

public class MiniMapCamera : MonoBehaviour
{
    private Camera miniMapCamera = default;
    private float viewfortRectX = 0.84375f;
    private float viewfortRectY = 0.01f;
    private float viewfortRectW = 0.155f;
    private float viewfortRectH = 0.2759f;

    private bool isExtended = false;

    private void Start()
    {
        miniMapCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isExtended)
        {
            
        }
        else if(Input.GetKeyDown(KeyCode.Tab) && isExtended) 
        {
            
        }
    }
}
