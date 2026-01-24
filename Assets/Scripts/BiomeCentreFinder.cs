using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeCentreFinder
{
        public static List<Vector2Int> neighbouringDirections = new List<Vector2Int> {
        new Vector2Int(0,1), 
        new Vector2Int(1,1), 
        new Vector2Int(1,0),
        new Vector2Int(1,-1),
        new Vector2Int(0,-1),
        new Vector2Int(-1,-1),
        new Vector2Int(-1,0),
        new Vector2Int(-1,1)
    };
    public static List<Vector3Int> CalculateBiomeCentre(Vector3 playerPosition, int drawRange, int mapSize)
    {
        int biomeLength = drawRange*mapSize;
        Vector3Int Origin = new Vector3Int(
            Mathf.RoundToInt(playerPosition.x/biomeLength) * biomeLength,
            0,
            Mathf.RoundToInt(playerPosition.z/biomeLength) * biomeLength
        );

        HashSet<Vector3Int> biomeCentresTemp = new HashSet<Vector3Int>();
        biomeCentresTemp.Add(Origin);
        foreach(Vector2Int offset in neighbouringDirections)
        {
            Vector3Int newBiomePoint_1 = new Vector3Int(Origin.x + offset.x * biomeLength,0, Origin.y + offset.y * biomeLength);
            Vector3Int newBiomePoint_2 = new Vector3Int(Origin.x + offset.x * biomeLength,0, Origin.y + offset.y *2* biomeLength);
            Vector3Int newBiomePoint_3 = new Vector3Int(Origin.x + offset.x *2* biomeLength,0, Origin.y + offset.y * biomeLength);
            Vector3Int newBiomePoint_4 = new Vector3Int(Origin.x + offset.x *2* biomeLength,0, Origin.y + offset.y *2* biomeLength);
            biomeCentresTemp.Add(newBiomePoint_1);
            biomeCentresTemp.Add(newBiomePoint_2);
            biomeCentresTemp.Add(newBiomePoint_3);
            biomeCentresTemp.Add(newBiomePoint_4);
        }

        return new List<Vector3Int>(biomeCentresTemp);
    }
}
