using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class World : MonoBehaviour {

    public int mapSizeInChunks = 6;
    public int chunkSize = 16, chunkHeight = 128;
    public int chunkDrawingRange = 4;

    public GameObject chunkPrefab;
    public Vector2Int mapSeedOffset;

    public TerrainGenerator terrainGenerator;

    public UnityEvent OnWorldCreated, OnNewChunksGenerated;

    public WorldData worldData {  get; private set; }

    private void Awake() {
        worldData = new WorldData { 
            chunkHeight = this.chunkHeight,
            chunkSize = this.chunkSize,
            chunkDataDictionary = new Dictionary<Vector3Int, ChunkData>(),
            chunkDictionary = new Dictionary<Vector3Int, ChunkRenderer>()
        };
    }

    public void GenerateWorld() {

        WorldGenerationData worldGenerationData = GetPositionsThatPlayerSees(Vector3Int.zero);

        foreach(var pos in worldGenerationData.chunkDataPositionsToCreate) {
            ChunkData data = new ChunkData(chunkSize, chunkHeight, this, pos);
            ChunkData newData = terrainGenerator.GenerateChunkData(data, mapSeedOffset);
            worldData.chunkDataDictionary.Add(pos, newData);
        }

        foreach (var pos in worldGenerationData.chunkPositionsToCreate) {
            ChunkData data = worldData.chunkDataDictionary[pos];
            MeshData meshData = Chunk.GetChunkMeshData(data);
            GameObject chunkObject = Instantiate(chunkPrefab, data.worldPosition, Quaternion.identity);
            ChunkRenderer chunkRenderer = chunkObject.GetComponent<ChunkRenderer>();
            worldData.chunkDictionary.Add(data.worldPosition, chunkRenderer);
            chunkRenderer.InitializeChunk(data);
            chunkRenderer.UpdateChunk(meshData);
        }

        OnWorldCreated?.Invoke();
    }

    private WorldGenerationData GetPositionsThatPlayerSees(Vector3Int playerPosition) {
        
        List<Vector3Int> allChunkPositionsNeeded = WorldDataHelper.GetChunkPositionsAroundPlayer(this,playerPosition);
        List<Vector3Int> allChunkDataPositionsNeeded = WorldDataHelper.GetDataPositionsAroundPlayer(this,playerPosition);

        List<Vector3Int> chunkPositionsToCreate = WorldDataHelper.SelectPositionsToCreate(worldData,allChunkPositionsNeeded,playerPosition);
        List<Vector3Int> chunkDataPositionsToCreate = WorldDataHelper.SelectDataPositionsToCreate(worldData, allChunkDataPositionsNeeded, playerPosition);

        WorldGenerationData data = new WorldGenerationData {
            chunkPositionsToCreate = chunkPositionsToCreate,
            chunkDataPositionsToCreate = chunkDataPositionsToCreate,
            chunkPositionsToRemove = new List<Vector3Int>(),
            chunkDataToRemove = new List<Vector3Int>()
        };
        return data;
    }

    public BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, int x, int y, int z) { 
        Vector3Int pos = Chunk.ChunkPositionFromBlockCoordinates(this, x,y,z);
        ChunkData containerChunk = null;

        worldData.chunkDataDictionary.TryGetValue(pos, out containerChunk);

        if (containerChunk == null) {
            return BlockType.Nothing;
        }
        Vector3Int positionInChunkCoordinates = Chunk.GetPositionInChunkCoordinates(containerChunk,new Vector3Int(x,y,z));
        return Chunk.GetBlockFromChunkCoordinates(containerChunk,positionInChunkCoordinates);
    }

    internal void LoadAdditionalChunksRequest(GameObject player) {
        Debug.Log("Generate new chunks");
        OnNewChunksGenerated?.Invoke();
    }

    public struct WorldData {
        public Dictionary<Vector3Int, ChunkData> chunkDataDictionary;
        public Dictionary<Vector3Int,ChunkRenderer> chunkDictionary;
        public int chunkSize;
        public int chunkHeight;
    };

    public struct WorldGenerationData {
        public List<Vector3Int> chunkPositionsToCreate;
        public List<Vector3Int> chunkDataPositionsToCreate;
        public List<Vector3Int> chunkPositionsToRemove;
        public List<Vector3Int> chunkDataToRemove;
    };
}