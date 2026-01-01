using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class ChunkRenderer : MonoBehaviour {

    MeshFilter meshFilter;
    MeshCollider meshCollider;
    Mesh mesh;
    public bool showGizmo = false;

    public ChunkData chunkData { get; private set; }

    public bool modifiedByThePlayer {
        get {
            return chunkData.modifiedByThePlayer;
        }
        set {
            chunkData.modifiedByThePlayer = value;
        }
    }

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = meshFilter.mesh;

    }

    public void InitializeChunk(ChunkData data) {
        this.chunkData = data;
    }

    private void RenderMesh(MeshData meshData) {
        mesh.Clear();

        mesh.subMeshCount = 2;
        mesh.vertices = meshData.vertices.Concat(meshData.waterMesh.vertices).ToArray(); //Concatenate vertices of water mesh to main mesh

        mesh.SetTriangles(meshData.triangles.ToArray(), 0);
        mesh.SetTriangles(meshData.waterMesh.triangles.Select(val => val + meshData.vertices.Count).ToArray(), 1); //Adding the triangles of ater mesh to main mesh with an offset

        mesh.uv = meshData.uvs.Concat(meshData.waterMesh.uvs).ToArray();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = null;
        Mesh collisionMesh = new Mesh();
        collisionMesh.vertices = meshData.colliderVertices.ToArray();
        collisionMesh.triangles = meshData.colliderTriangles.ToArray();
        collisionMesh.RecalculateNormals();

        meshCollider.sharedMesh = collisionMesh;
    }

    public void UpdateChunk() {
        RenderMesh(Chunk.GetChunkMeshData(chunkData));
    }

    public void UpdateChunk(MeshData data) {
        RenderMesh(data);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (showGizmo) {
            if (Application.isPlaying && chunkData != null) {
                if (Selection.activeObject == gameObject)
                    Gizmos.color = new Color(0, 1, 0, 0.4f);
                else
                    Gizmos.color = new Color(1, 0, 1, 0.4f);

                Gizmos.DrawCube(transform.position + new Vector3(chunkData.chunkSize / 2f, chunkData.chunkHeight / 2f, chunkData.chunkSize / 2f), new Vector3(chunkData.chunkSize, chunkData.chunkHeight, chunkData.chunkSize));
            }
        }
    }
#endif
}