using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceLayerHandler : BlockLayerHandler {
    public BlockType surfaceBlockType;
    public BlockType type2;

    public int Snow_Threshold = 50;
    public int water_flag = 0;
    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset) {
        if(water_flag == 1)
        {
            if(surfaceHeightNoise < 30)surfaceHeightNoise = 30;
            
        }
        if (y == surfaceHeightNoise) {
            if(y >= Snow_Threshold)
            {
                Chunk.SetBlock(chunkData, new Vector3Int(x, y, z), type2);
                return true;
            }
            Vector3Int pos = new Vector3Int(x, y, z);
            Chunk.SetBlock(chunkData, pos, surfaceBlockType);
            return true;
        }
        return false;
    }
}