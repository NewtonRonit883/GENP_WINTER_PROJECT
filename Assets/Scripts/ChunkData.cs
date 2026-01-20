using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChunkData
{
    public BlockType[] blocks; //BlockType array to hold block data rather than 3D array for memory efficiency
    public int chunkSize = 16; //Width and Breadth of chunk 
    public int chunkHeight = 128; //Height of chunk 
    public World worldReference; //DIfferent worlds can have different chunk data ex : overworld , nether, end
    public Vector3Int worldPosition; //Position of chunk in world space

    public bool modifiedByThePlayer = false;
    public TreeData treeData;

    public ChunkData(int chunkSize, int chunkHeight, World worldReference, Vector3Int worldPosition) //Constructor initializing chunk data
    {
        this.chunkSize = chunkSize; //this.chunksize refers to object property while chunkSize refers to parameter
        this.chunkHeight = chunkHeight; //this.chunkHeight refers to object property while chunkHeight refers to parameter
        this.worldReference = worldReference;
        this.worldPosition = worldPosition;
        blocks = new BlockType[chunkSize * chunkSize * chunkHeight]; //Initialize block array volume = width * breadth * height

    }
    

    //ChunkData chunk = new ChunkData(16, 128, worldReference, new Vector3Int(0, 0, 0));
}
