using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solidify : MonoBehaviour
{
    [SerializeField]
    private Shader flatShader;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.SetReplacementShader(flatShader, string.Empty);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
