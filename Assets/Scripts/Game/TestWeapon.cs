using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TestWeapon
{
    public float level;
    public ATK atk;
    public ATKspeed atkSpeed;
}
public struct ATK
{
    Elemental elemental;
    float atlValue;
}
public struct ATKspeed
{
    Elemental elemental;
    float atkSpeedlValue;
}
