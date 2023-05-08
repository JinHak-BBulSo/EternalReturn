using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ForbiddenAreaSelector : MonoBehaviour
{
    public List<int> areaIndex = new List<int>();
    public List<GameObject> allArea = new List<GameObject>();
    public List<SpriteRenderer> areaSprite = new List<SpriteRenderer>();


    void Start()
    {
        for (int i = 0; i < allArea.Count; i++)
        {
            areaIndex.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    [PunRPC]
    public void ForbiddenAreaSelect()
    {

    }
}
