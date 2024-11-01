using System;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.GameplayCore {
    public class LevelParser : MonoBehaviour {

        public GameObject TileTypeToGameObject(TileType tileType) {
            return tileType switch {
                TileType.EmptyTile => null,
                TileType.DefaultTile => Resources.Load<GameObject>("TilePrefabs/DefaultTile"),
                TileType.CountdownTile => Resources.Load<GameObject>("TilePrefabs/CountdownTile"),
                TileType.DoubleCountdownTile => null,
                TileType.FreezeTile => null,
                TileType.PushTile => null,
                _ => throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null)
            };
        }


    }
}