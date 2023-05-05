using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingRotation : MonoBehaviour
{
    private float speed = -180;
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, speed * Time.deltaTime);
    }
}
