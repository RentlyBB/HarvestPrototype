using System.Collections;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class FreezeHorizontalTile : TileBase {

        public override void SetupTile() {
            tileVisualHandler.SetMainAndSubState(TileMainVisualStates.DefaultState, TileSubVisualStates.Unpressed);
        }

        public override void OnPlayerStepAfterDecreasing() {
            base.OnPlayerStepAfterDecreasing();
            StartCoroutine(FreezeLine());
        }

        private IEnumerator FreezeLine() {
            var grid = GridManager.Instance.GetGrid();

            yield return new WaitForSeconds(0.2f);
            
            for (var i = 0; i < grid.GetWidth(); i++) {
                var tile = grid.GetGridObject(new Vector2(i, gridPosition.y)).GetTile();

                if (tile.TryGetComponent(out TileFreezeHandler tileFreezeHandler)) {
                    yield return new WaitForSeconds(0.1f);
                    tile.tileAnimationHandler.FreezeAnimation();
                    tileFreezeHandler.FreezeTile();
                }
            }
        }
    }
}