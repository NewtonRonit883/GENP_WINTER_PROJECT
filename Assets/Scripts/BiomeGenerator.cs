using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    public int waterThreshold = 50;

    public NoiseSettings biomeNoiseSettings;

    public BlockLayerHandler startLayerHandler;
    public List<BlockLayerHandler> additionalLayerHandlers;

    public DomainWarping domainWarping;
    public bool useDomainWarping = true;

    public TreeGenerator treeGenerator;


    internal TreeData GetTreeData(ChunkData data, Vector2Int mapSeedOffset)
    {
        if (treeGenerator == null)
        {
            return new TreeData();
        }
        return treeGenerator.GenerateTreeData(data, mapSeedOffset);
    }

    public ChunkData ProcessChunkColumn(ChunkData data, int x, int z, Vector2Int mapSeedOffset, int? terrainSurfaceNoise)
    {

        //float noiseValue = Mathf.PerlinNoise((mapSeedOffset.x+data.worldPosition.x + x) * noiseScale, mapSeedOffset.y+(data.worldPosition.z + z) * noiseScale);
        //int groundPosition = Mathf.RoundToInt(noiseValue * data.chunkHeight);
        biomeNoiseSettings.worldOffset = mapSeedOffset;
        int groundPosition = GetSurfaceHeightNoise(data.worldPosition.x + x, data.worldPosition.z+z ,data.chunkHeight);

        for (int y = 0; y < data.chunkHeight; y++)
        {
            startLayerHandler.Handle(data, x, y, z, groundPosition, mapSeedOffset);
        }
        foreach(BlockLayerHandler handler in additionalLayerHandlers)
        {
            handler.Handle(data, x, data.worldPosition.y, z, groundPosition, mapSeedOffset);        
        }
        return data;
    }

    public int GetSurfaceHeightNoise(int x, int z,int chunkHeight) //it returns surface height in world coordinates
    {
        float terrainHeight;
        if (useDomainWarping)
        {
            terrainHeight = domainWarping.GenerateDomainNoise(x, z, biomeNoiseSettings);
        }
        else
        {
            terrainHeight = Noise.OctavePerlin(x, z, biomeNoiseSettings);
        }
        terrainHeight = Noise.Redistribution(terrainHeight, biomeNoiseSettings);
        int surfaceHeight = (int)Noise.RemapValue01ToInt(terrainHeight, 20, chunkHeight);
        return surfaceHeight;
    }
}