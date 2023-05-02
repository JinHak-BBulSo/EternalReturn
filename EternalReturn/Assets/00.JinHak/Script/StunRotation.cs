using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunRotation : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, 140 * Time.deltaTime);
    }
}
