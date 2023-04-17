using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class Debuff
{
    public bool[] applyDebuffCheck = new bool[10];
    public float[] continousTime = new float[10];
    public float[] debuffDelayTime = new float[10];
}
