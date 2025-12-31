using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class World : MonoBehaviour {
    public int chunkSize = 16, chunkHeight = 128;
    public GameObject chunkPrefab;

    public int chunkDrawingRange = 4;

    public Vector2Int mapSeedOffset = new Vector2Int();

    public WorldData worldData {get; private set;}

    public Transform playerTransform;
    private Vector3 previousPlayerPosition;

    public TerrainGenerator terrainGenerator;

    public float RegenTerrainDistance = 40f;
    public UnityEvent OnWorldCreated, OnNewChuncksGenerated;

    public void Awake()
    {
        worldData = new WorldData
        {
            chunkHeight = this.chunkHeight,
            chunkSize = this.chunkSize,
            chunkDataDictionary = new Dictionary<Vector3Int, ChunkData>(),
            chunkDictionary = new Dictionary<Vector3Int, ChunkRenderer>()
        };
    }
    public void GenerateWorld(Vector3Int playerpos)
    {
        WorldGenrationData worldGenrationData = GetPositionsThatPlayerSees(playerpos);
        foreach(var pos in worldGenrationData.chunkDataPositionsToCreate)
        {
            ChunkData data = new ChunkData(chunkSize,chunkHeight, this,pos);
            //GenerateVoxels(data);
            ChunkData newData = terrainGenerator.GenerateChunkData(data, mapSeedOffset);
            worldData.chunkDataDictionary.Add(data.worldPosition,data);
        }

        foreach (var pos in worldGenrationData.chunkPositionsToCreate)
        {
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

    private WorldGenrationData GetPositionsThatPlayerSees(Vector3Int playerPosition)
    {
        List<Vector3Int> allChunkPositionsNeeded = WorldDataHelper.GetChunkPositionsAroundPlayer(this, playerPosition);
        List<Vector3Int> allChunkDataPositionsNeeded = WorldDataHelper.GetDataPositionsAroundPlayer(this, playerPosition);

        List<Vector3Int> ChunkPositionsToCreate = WorldDataHelper.SelectPositonsToCreate(this.worldData, allChunkPositionsNeeded, playerPosition);
        List<Vector3Int> ChunkDataPositionsToCreate = WorldDataHelper.SelectDataPositonsToCreate(this.worldData, allChunkDataPositionsNeeded, playerPosition);

        WorldGenrationData data = new WorldGenrationData
        {
            chunkPositionsToCreate = ChunkPositionsToCreate,
            chunkDataPositionsToCreate = ChunkDataPositionsToCreate,
            chunkDataToRemove = new List<Vector3Int>(),
            chunkPositionsToRemove = new List<Vector3Int>()

        };
        return data;
    }

    public BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
    {
        Vector3Int pos = Chunk.ChunkPositionFromBlockCoordinates(this, x, y, z);
        ChunkData containerChunk = null;
        worldData.chunkDataDictionary.TryGetValue(pos, out containerChunk);

        if(containerChunk == null)
        {
            return BlockType.Nothing;
        }
        Vector3Int positionInChunkCoordinates = Chunk.GetPositionInChunkCoordinates(containerChunk, new Vector3Int(x,y,z));
        return Chunk.GetBlockFromChunkCoordinates(containerChunk, positionInChunkCoordinates);
    }

    void Start(){
        //RegenTerrainDistance = mapSizeInChunks*chunkSize/2;
        GenerateWorld(Vector3Int.zero);
        //previousPlayerPosition = playerTransform.position;

    }

    void Update()
    {
        //Vector2 a = new Vector2(playerTransform.position.x, playerTransform.position.z);
        //Vector2 b = new Vector2(previousPlayerPosition.x, previousPlayerPosition.z);
        //if((a-b).magnitude > RegenTerrainDistance)
        //{
        //    GenerateWorld(playerTransform.position);
        //    previousPlayerPosition = playerTransform.position;
        //}
    }

    internal void LoadAdditionalChunksRequest(GameObject player)
    {
        Debug.Log("generate new chunks");
        OnNewChuncksGenerated?.Invoke();
        GenerateWorld( new Vector3Int((int)player.transform.position.x,(int)player.transform.position.y,(int)player.transform.position.z));
    }

    public struct WorldData
    {
        public Dictionary<Vector3Int, ChunkData> chunkDataDictionary;
        public Dictionary<Vector3Int, ChunkRenderer> chunkDictionary;
        public int chunkSize;
        public int chunkHeight;
    }

    public struct WorldGenrationData
    {
        public List<Vector3Int> chunkPositionsToCreate;
        public List<Vector3Int> chunkDataPositionsToCreate;
        public List<Vector3Int> chunkPositionsToRemove;
        public List<Vector3Int> chunkDataToRemove;
    }
}