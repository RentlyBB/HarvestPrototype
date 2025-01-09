using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.GameplayCore;
using _Scripts.GridCore;
using _Scripts.PlayerCore;
using _Scripts.TileCore;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using _Scripts.UnitySingleton;
using Mono.CSharp;
using QFSW.QC;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Managers {
    public class GameplayManager : MonoSingleton<GameplayManager> {
        public static event UnityAction<LevelData> OnLoadLevel = delegate { };

        public LevelData levelData;

        // Temp data from level to work with
        public List<CountdownTileBase> countdownTileBases = new List<CountdownTileBase>();
        public List<TileBase> frozenTiles = new List<TileBase>();

        private void OnEnable() {
            MovementHandler.OnPlayerReachedTarget += StartPhaseRunner;
        }

        private void OnDisable() {
            MovementHandler.OnPlayerReachedTarget -= StartPhaseRunner;
        }

        private void Start() {
            LoadLevel();
        }

        // OnStepPhase
        // OnDecreasePhase/CountdownPhase
        // UnfreezePhase 

        [Command]
        private void LoadLevel() {
            ClearTempData();

            OnLoadLevel?.Invoke(levelData);

            Grid<TileGridObject> grid = GridManager.Instance.GetGrid();

            // Store all countdown tiles to tempDataList
            foreach (TileData tile in levelData.tiles) {
                if (tile.tileTypeData.isCountdownTile) {
                    countdownTileBases.Add((CountdownTileBase)grid.GetGridObject(tile.gridPosition).GetTile());
                }
            }
        }

        private void ClearTempData() {
            countdownTileBases.Clear();
            frozenTiles.Clear();
        }

        private async void StartPhaseRunner(TileGridObject pressedTile) {
            await StepOnTilePhase(pressedTile);
            await CountdownPhase();
            await UnfreezePhase();
        }

        private async Task StepOnTilePhase(TileGridObject pressedTile) {
            await pressedTile.GetTile().OnPlayerStep();
        }
        
        private async Task CountdownPhase() {
            foreach (CountdownTileBase tile in countdownTileBases) {
                await tile.OnDecreaseCountdownValue();
            }
        }

        private async Task UnfreezePhase() {
            
            await Task.Delay(500);
            
            Grid<TileGridObject> grid = GridManager.Instance.GetGrid();
            
            foreach (TileData tile in levelData.tiles) {
                TileBase tileBase = grid.GetGridObject(tile.gridPosition).GetTile();

                if (tileBase.tileVisualHandler.CurrentMainState != TileMainVisualStates.FreezeState)
                    continue;
                
                if (!tileBase.TryGetComponent(out TileFreezeHandler tileFreezeHandler))
                    continue;

                tileFreezeHandler.UnfreezeTile();
                await Task.Delay(100);
            }
            await Task.Yield();
        }
    }
}