using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshColiderAddObj : MonoBehaviour
{
    private Renderer[] renderers;
    void Start()
    {
        GetAllRender();

        foreach(var renderer in renderers)
        {
            renderer.AddComponent<MeshCollider>();
        }
    }

    protected virtual void GetAllRender()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }
}
