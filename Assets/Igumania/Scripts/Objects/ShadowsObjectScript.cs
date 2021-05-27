using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowsObjectScript : ScriptableObject
{
    [Header("Textures")]
    public Texture2D CS_Clouds1;
    public Texture2D CS_Clouds2;

    [Header("Speed")]
    public float CS_BaseSpeed;
    public Vector3 CS_Speed1;
    public Vector3 CS_Speed2;

    [Header("Tiling")]
    public float CS_Tiling1;
    public float CS_Tiling2;

    [Header("Clouds Settings")]
    public float CS_CloudsPower;
    public float CS_CloudsMultiply;
    public float CS_CloudsTransparency;
}
