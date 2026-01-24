using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    public BiomeGenerator biomeGenerator;
    public DomainWarping biomeDomainWarping;

    [SerializeField] List<Vector3Int> biomeCentres = new List<Vector3Int>();
    List<float> biomeNoise = new List<float>();

    [SerializeField] List<BiomeData> biomeGeneratorData = new List<BiomeData>();
    [SerializeField] private NoiseSettings biomeNoiseSettings;


    public ChunkData GenerateChunkData(ChunkData data, Vector2Int mapSeedOffset)
    {
        BiomeGeneratorSelection biomeSelection = SelectBiomeGenerator(data.worldPosition,data,false);
        //TreeData treeData = biomeGenerator.GetTreeData(data,mapSeedOffset);
        data.treeData = biomeSelection.biomeGenerator.GetTreeData(data,mapSeedOffset);
        for (int x = 0; x < data.chunkSize; x++)
        {
            for (int z = 0; z < data.chunkSize; z++)
            {
                biomeSelection = SelectBiomeGenerator(new Vector3Int(data.worldPosition.x + x, 0,data.worldPosition.z + z),data);
                data = biomeSelection.biomeGenerator.ProcessChunkColumn(data, x, z, mapSeedOffset, biomeSelection.terrainSurfaceNoise);


            }
        }
        return data;
    }

    private BiomeGeneratorSelection SelectBiomeGenerator(Vector3Int worldPosition, ChunkData data, bool useDomainWarping = true) {
        if (useDomainWarping == true) {
            Vector2Int domainOffset = Vector2Int.RoundToInt(biomeDomainWarping.GenerateDomainOffset(worldPosition.x, worldPosition.z));
            worldPosition += new Vector3Int(domainOffset.x, 0, domainOffset.y);
        }

        List<BiomeSelectionHelper> biomeSelectionHelpers = GetBiomeGeneratorSelectionHelpers(worldPosition);
        BiomeGenerator generator_1 = SelectBiome(biomeSelectionHelpers[0].Index);
        BiomeGenerator generator_2 = SelectBiome(biomeSelectionHelpers[1].Index);

        float weight_0 = biomeSelectionHelpers[1].Distance / (biomeSelectionHelpers[0].Distance + biomeSelectionHelpers[1].Distance);
        weight_0 = Mathf.SmoothStep(0, 1, weight_0);
        float weight_1 = 1 - weight_0;
        int terrainHeightNoise_0 = generator_1.GetSurfaceHeightNoise(worldPosition.x, worldPosition.z, data.chunkHeight);
        int terrainHeightNoise_1 = generator_2.GetSurfaceHeightNoise(worldPosition.x, worldPosition.z, data.chunkHeight);
        return new BiomeGeneratorSelection(generator_1, Mathf.RoundToInt(terrainHeightNoise_0 * weight_0 + terrainHeightNoise_1 * weight_1));
    }

    private BiomeGenerator SelectBiome(int index) {

        float temp = biomeNoise[index];
        foreach (var data in biomeGeneratorData) {
            if (temp >= data.temperatureStartThreshold && temp <= data.temperatureEndThreshold) {
                return data.biomeTerrainGenerator;
            }
        }
        return biomeGeneratorData[0].biomeTerrainGenerator;
    }

    private List<BiomeSelectionHelper> GetBiomeGeneratorSelectionHelpers(Vector3Int position) {
        position.y = 0;
        return GetClosestBiomeIndex(position);

    }

    private List<BiomeSelectionHelper> GetClosestBiomeIndex(Vector3Int position) {
        return biomeCentres.Select((center, index) =>
        new BiomeSelectionHelper {
            Index = index,
            Distance = UnityEngine.Vector3.Distance(center, position)
        }).OrderBy(helper => helper.Distance).Take(4).ToList();
    }
    

    public void GenerateBiomePoints(UnityEngine.Vector3 playerPosition, int drawRange, int mapSize, Vector2Int mapSeedOffset) {

        biomeCentres = new List<Vector3Int>();
        biomeCentres = BiomeCentreFinder.CalculateBiomeCentre(playerPosition, drawRange, mapSize);

        for(int i = 0; i < biomeCentres.Count; i++) {

            Vector2Int domainWarpingOffset = biomeDomainWarping.GenerateDomainOffsetInt(biomeCentres[i].x, biomeCentres[i].z);
            biomeCentres[i] += new Vector3Int(domainWarpingOffset.x, 0, domainWarpingOffset.y);
        }
        biomeNoise = CalculateBiomeNoise(biomeCentres, mapSeedOffset);
    }

    private List<float> CalculateBiomeNoise(List<Vector3Int> biomeCentres, Vector2Int mapSeedOffset) {
        biomeNoiseSettings.worldOffset = mapSeedOffset;
        return biomeCentres.Select(center => Noise.OctavePerlin(center.x, center.z, biomeNoiseSettings)).ToList();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;

        foreach(var biomeCentrePoint in biomeCentres) {
            Gizmos.DrawLine(biomeCentrePoint, biomeCentrePoint + UnityEngine.Vector3.up *255);
        }
    }

    [Serializable]
    public struct BiomeData {

        [Range(0f,1f)]
        public float temperatureStartThreshold, temperatureEndThreshold;
        public BiomeGenerator biomeTerrainGenerator;

    }

    private struct BiomeSelectionHelper {
        public int Index;
        public float Distance;
    }
}

public class BiomeGeneratorSelection {
    public BiomeGenerator biomeGenerator = null;
    public int? terrainSurfaceNoise = null;

    public BiomeGeneratorSelection(BiomeGenerator biomeGenerator, int? terrainSurfaceNoise = null) {
        this.biomeGenerator = biomeGenerator;
        this.terrainSurfaceNoise = terrainSurfaceNoise;
    }
}