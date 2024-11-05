using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using _Scripts.TileCore.Interfaces;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class CollectTile : TileBase, IInteractableTile {

        private void OnEnable() {
            GameplayManager.OnCountdownDecreasing += ReplaceTile;
        }

        private void OnDisable() {
            GameplayManager.OnCountdownDecreasing -= ReplaceTile;
        }

        public void OnPlayerStep() {
            throw new System.NotImplementedException();
        }

        private void ReplaceTile() {
            GridManager.Instance.ReplaceTileWith(gridPosition, TileType.BadCollectTile);
        }
    }
}