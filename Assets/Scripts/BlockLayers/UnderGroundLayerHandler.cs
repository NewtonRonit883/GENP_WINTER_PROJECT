using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundLayerHandler : BlockLayerHandler {
    public BlockType undergroundBlockType;

    public int water_Threshold = 50;

    public int water_flag = 0;
    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset) {

        if(water_flag == 1)
        {
            if(y < water_Threshold || y < surfaceHeightNoise)
            {
                Vector3Int pos = new Vector3Int(x, y, z);
                Chunk.SetBlock(chunkData, pos, BlockType.Sand);
                return true;
            }
            else return false;
        }

        if(y < water_Threshold)
        {
            if (y < surfaceHeightNoise) {
            Vector3Int pos = new Vector3Int(x, y, z);
            Chunk.SetBlock(chunkData, pos, BlockType.Sand);
            return true;
            }
        }


        if (y < surfaceHeightNoise) {
            Vector3Int pos = new Vector3Int(x, y, z);
            Chunk.SetBlock(chunkData, pos, undergroundBlockType);
            return true;
        }
        return false;
    }
}