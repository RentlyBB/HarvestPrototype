using System;
using System.Collections;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using _Scripts.TileCore.Interfaces;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public sealed class CountdownTile : TileBase, IInteractableTile {

        public int countdownValue;

        private TextMesh _middleText;
        private bool _isCollected;

        private void OnEnable() {
            GameplayManager.OnCountdownDecreasing += DecreaseCountdownValue;
        }

        private void OnDisable() {
            GameplayManager.OnCountdownDecreasing -= DecreaseCountdownValue;
        }

        private void Start() {
            _middleText = Utils.CreateTextWorld(countdownValue.ToString(), transform.position, 32, transform);
        }

        private void DecreaseCountdownValue() {
            countdownValue--;

            if (countdownValue == 0) {
                //Destroy / Unload this tile and change it with the CollectTile
                GridManager.Instance.ReplaceTileWith(gridPosition, TileType.CollectTile);

            }
            _middleText.text = countdownValue.ToString();
        }

        public void OnPlayerStep() {
            //Destroy / Unload this tile and change it with the BadCollectTile
            GridManager.Instance.ReplaceTileWith(gridPosition, TileType.BadCollectTile);
        }
    }
}