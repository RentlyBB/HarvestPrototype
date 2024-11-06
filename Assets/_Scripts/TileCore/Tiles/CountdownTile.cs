using System;
using System.Collections;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using _Scripts.TileCore.Interfaces;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public sealed class CountdownTile : TileBase {
        public int countdownValue;

        private TextMesh _middleText;
        private bool _isCollected;

        private void OnEnable() {
            GameplayManager.CountdownDecreasing += DecreaseCountdownValue;
        }

        private void OnDisable() {
            GameplayManager.CountdownDecreasing -= DecreaseCountdownValue;
        }

        private void Start() {
            _middleText = Utils.CreateTextWorld(countdownValue.ToString(), transform.position, 32, transform);
        }

        private void DecreaseCountdownValue() {
            countdownValue--;

            if (countdownValue == 0) {


                //countdownState - CountingDown / ReadyToCollect / Collected;
                
                
                
                
                //Destroy / Unload this tile and change it with the CollectTile
                TileType tileType = TileType.CollectTile;
                
                GridManager.Instance.ReplaceAndDestroyTileWith(gridPosition, tileType);
            } 

            _middleText.text = countdownValue.ToString();
        }

        public override void OnPlayerStep() {
            //Destroy / Unload this tile and change it with the BadCollectTile
            GridManager.Instance.ReplaceAndDestroyTileWith(gridPosition, TileType.BadCollectTile);
        }

    }
}