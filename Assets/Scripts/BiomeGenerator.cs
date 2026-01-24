using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    public int waterThreshold = 25;
    public int minWorldLevel = 20; // For desert biome, set this to 25

    public NoiseSettings biomeNoiseSettings;

    public TreeGenerator treeGenerator;

    public DomainWarping domainWarping;
    public bool useDomainWarping = true;

    public BlockLayerHandler startLayerHandler;
    public List<BlockLayerHandler> additionalLayerHandlers;
    public ChunkData ProcessChunkColumn(ChunkData data, int x, int z, Vector2Int mapSeedOffset,int? terrainSurfaceNoise)
    {
        biomeNoiseSettings.worldOffset = mapSeedOffset;
        int groundPosition = GetSurfaceHeightNoise(data.worldPosition.x + x, data.worldPosition.z+z, data.chunkHeight);

        for (int y = 0; y < data.chunkHeight; y++)
        {
            startLayerHandler.Handle(data,x,y,z,groundPosition,mapSeedOffset);

        }
        foreach(BlockLayerHandler handler in additionalLayerHandlers) {
            handler.Handle(data,x,data.worldPosition.y,z,groundPosition,mapSeedOffset);
        }
        return data;
    }

    internal TreeData GetTreeData(ChunkData data, Vector2Int mapSeedOffset) {
        if (treeGenerator == null) {
            return new TreeData();
        }
        return treeGenerator.GenerateTreeData(data, mapSeedOffset);
    }

    public int GetSurfaceHeightNoise(int x, int z, int chunkHeight) //it returns surface height in world coordinates
    {
        float terrainHeight;
        if (useDomainWarping) {
            terrainHeight = domainWarping.GenerateDomainNoise(x, z, biomeNoiseSettings);
        }
        else {
            terrainHeight = Noise.OctavePerlin(x, z, biomeNoiseSettings);
        }
        terrainHeight = Noise.Redistribution(terrainHeight, biomeNoiseSettings);
        int surfaceHeight = Noise.RemapValue01ToInt(terrainHeight, minWorldLevel, chunkHeight);
        return surfaceHeight;
    }


}