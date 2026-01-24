using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class BlockHelper
{
    private static Direction[] directions =
    {
        Direction.backward,
        Direction.foward,
        Direction.up,
        Direction.down,
        Direction.left,
        Direction.right
    };
    public static MeshData GetMeshData
        (ChunkData chunk, int x, int y, int z, MeshData meshData, BlockType blockType) {
        if (blockType == BlockType.Air || blockType == BlockType.Nothing)
            return meshData;

        foreach (Direction direction in directions) {
            var neighbourBlockCoordinates = new Vector3Int(x, y, z) + direction.GetVector();
            var neighbourBlockType = Chunk.GetBlockFromChunkCoordinates(chunk, neighbourBlockCoordinates);

            if (neighbourBlockType != BlockType.Nothing && BlockDataManager.blocTextureDataDictionary[neighbourBlockType].isSolid == false) {

                if (blockType == BlockType.Water) {
                    if (neighbourBlockType == BlockType.Air)
                        meshData.waterMesh = GetFaceDataIn(direction, chunk, x, y, z, meshData.waterMesh, blockType);
                }
                else {
                    meshData = GetFaceDataIn(direction, chunk, x, y, z, meshData, blockType);
                }

            }
        }

        return meshData;
    }

    public static MeshData GetFaceDataIn(Direction direction, ChunkData chunk, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        GetVertices(direction, x, y, z, meshData, blockType);
        meshData.AddQuadTriangles(BlockDataManager.blocTextureDataDictionary[blockType].generatesCollider);
        meshData.uvs.AddRange(FaceUVs(direction, blockType));
        return meshData;
    }

    public static Vector2[] FaceUVs(Direction direction, BlockType blockType)
    {
        Vector2[] UVs = new Vector2[4];
        var TilePos = TexturePosition(direction, blockType);
        
        UVs[0] = new Vector2(BlockDataManager.tileSizeX * TilePos.x + BlockDataManager.tileSizeX - BlockDataManager.textureOffset,
            BlockDataManager.tileSizeY * TilePos.y + BlockDataManager.textureOffset);

        UVs[1] = new Vector2(BlockDataManager.tileSizeX * TilePos.x + BlockDataManager.tileSizeX - BlockDataManager.textureOffset,
            BlockDataManager.tileSizeY * TilePos.y + BlockDataManager.tileSizeY - BlockDataManager.textureOffset);

        UVs[2] = new Vector2(BlockDataManager.tileSizeX * TilePos.x + BlockDataManager.textureOffset,
            BlockDataManager.tileSizeY * TilePos.y + BlockDataManager.tileSizeY - BlockDataManager.textureOffset);

        UVs[3] = new Vector2(BlockDataManager.tileSizeX * TilePos.x + BlockDataManager.textureOffset,
            BlockDataManager.tileSizeY * TilePos.y + BlockDataManager.textureOffset);


        return UVs;
    } 

    public static void GetVertices(Direction direction, int x, int y, int z, MeshData meshData, BlockType blockType)
    {
        var generatesCollider = BlockDataManager.blocTextureDataDictionary[blockType].generatesCollider;

         switch (direction) {
            case Direction.backward:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                break;
            case Direction.foward:
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.left:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                break;

            case Direction.right:
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.down:
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                break;
            case Direction.up:
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                break;
            default:
                break;
        };
    }

    public static Vector2Int TexturePosition (Direction direction, BlockType blockType)
    {
        return direction switch{
            Direction.up => BlockDataManager.blocTextureDataDictionary[blockType].up,
            Direction.down => BlockDataManager.blocTextureDataDictionary[blockType].down,
            Direction.right => BlockDataManager.blocTextureDataDictionary[blockType].side,
            Direction.left => BlockDataManager.blocTextureDataDictionary[blockType].side,
            Direction.foward => BlockDataManager.blocTextureDataDictionary[blockType].side,
            Direction.backward => BlockDataManager.blocTextureDataDictionary[blockType].side,
            _ => BlockDataManager.blocTextureDataDictionary[blockType].side
        };
    }

}
