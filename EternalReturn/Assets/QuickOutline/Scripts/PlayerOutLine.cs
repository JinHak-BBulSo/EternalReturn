using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOutLine : Outline
{

    protected override void BringRender()
    {
        renderers = new Renderer[1];
        renderers[0] = transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
    }
}
