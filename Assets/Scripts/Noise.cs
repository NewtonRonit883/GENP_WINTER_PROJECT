using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    // Start is called before the first frame update
    public static float RemapValue(float value, float initialMin, float initialMax, float OutputMin, float OutputMax)    //value = 0.3 , range 0-1 , range 0 - 100 => value = 60
    {
        return OutputMin + (value - initialMin) * (OutputMax - OutputMin) / (initialMax - initialMin);
    }
    public static int RemapValueToInt(float value, float initialMin, float initialMax, float OutputMin, float OutputMax)
    {
        return (int)RemapValue(value, 0, 1, OutputMin, OutputMax);
    }
    public static float OctavePerlin(float x, float z, NoiseSettings settings) //Octave = Single Perlin function
        //
    {
        x *= settings.noiseZoom;
        z *= settings.noiseZoom;
        x += settings.noiseZoom;
        z += settings.noiseZoom;

        float total = 0;  //total noise value
        float frequency = 1; //controls how often the noise changes
        float amplitude = 1; //controls the height of the noise
        float amplitudeSum = 0; //normalization factor

        for (int i = 0;i < settings.octaves; i++)
        {
            total +=  Mathf.PerlinNoise((settings.offset.x + settings.worldOffset.x + x) * frequency, (settings.offset.y + settings.worldOffset.y + z) * frequency) * amplitude;  //0-1 range
            amplitudeSum += amplitude;
            amplitude *= settings.persistence;
            frequency *= 2;
        }
        return total / amplitudeSum;
    }

    public static float Redistribution(float noise, NoiseSettings biomeNoiseSettings) //distibutes the noise value based on exponent and modifier and Let's us shape the terrain into exaggerated hills or flatlands
    {
        return Mathf.Pow(noise*biomeNoiseSettings.redistributionModifier, biomeNoiseSettings.exponent);  //Mathf.Pow(base,exponent) , Mathf.Pow(2,3) = 8
    }

    public static int RemapValue01ToInt(float terrainHeight, int v, int chunkHeight)
    {
        return RemapValueToInt(terrainHeight, 0, 1, v, chunkHeight);
    }
}