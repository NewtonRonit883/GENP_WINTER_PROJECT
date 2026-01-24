using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomainWarping : MonoBehaviour
{
    public NoiseSettings noiseDomainX, noiseDomainY;
    public int amplitudeX = 20, amplitudeY = 20;

    public float GenerateDomainNoise(int x, int z, NoiseSettings defaultNoiseSettings) {
        Vector2 doaminOffset = GenerateDomainOffset(x, z);
        return Noise.OctavePerlin(x + doaminOffset.x, z + doaminOffset.y, defaultNoiseSettings);
    }

    public Vector2 GenerateDomainOffset(int x, int z) {
        var noiseX = Noise.OctavePerlin(x,z,noiseDomainX) * amplitudeX;
        var noiseY = Noise.OctavePerlin(x,z,noiseDomainY) * amplitudeY;
        return new Vector2 (noiseX,noiseY);
    }

    public Vector2Int GenerateDomainOffsetInt(int x, int z) { 
        return Vector2Int.RoundToInt(GenerateDomainOffset(x,z));
    }
}
