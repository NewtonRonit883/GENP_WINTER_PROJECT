using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "noiseSettings", menuName = "Data/NoiseSettings")]

public class NoiseSettings : ScriptableObject
{
    public float noiseZoom;   //controls zoom level
    public int octaves;   //layers of perlin noise
    public Vector2Int offset;    //local seed offset 
    public Vector2Int worldOffset; //world seed offset
    public float persistence; //controls amplitude of octaves
    public float redistributionModifier; //controls the intensity of redistribution
    public float exponent;  //controls the sharpness of redistribution

    // Start is called before the first frame update
}