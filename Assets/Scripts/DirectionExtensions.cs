using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DirectionExtensions
{
    public static Vector3Int GetVector(this Direction direction) {
        return direction switch {
            Direction.up => Vector3Int.up,
            Direction.down => Vector3Int.down,
            Direction.left => Vector3Int.left,
            Direction.right => Vector3Int.right,
            Direction.forward => Vector3Int.forward,
            Direction.backward => Vector3Int.back,
            _ => throw new Exception("Invalid input direction")
        };
    }
}
