using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectController : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerClone = Instantiate(player);
        playerClone.transform.SetParent(transform, false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
