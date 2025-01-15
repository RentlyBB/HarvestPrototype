using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.GridCore;
using _Scripts.LevelEditor;
using _Scripts.PlayerCore;
using _Scripts.TileCore;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using _Scripts.UnitySingleton;
using DG.Tweening;
using QFSW.QC;
using UnityEngine;
using UnityEngine.Events;
using VInspector;

namespace _Scripts.Managers {
    
    // TODO: Break it to the parts like: PhaseHandler.cs LevelLoader.cs
    public class GameplayManager : MonoSingleton<GameplayManager> {
        public static event UnityAction<LevelData> OnLoadLevel = delegate { };

        public MovementHandler movementHandler;
        public LevelData levelData;

        // Temp data from level to work with
        public List<CountdownTileBase> countdownTileBases = new List<CountdownTileBase>();
        public List<TileBase> frozenTiles = new List<TileBase>();
        
        private readonly LinkedList<Queue<Func<Task>>> _phaseQueue = new LinkedList<Queue<Func<Task>>>();

        private Queue<Func<Task>> _currentPhaseBulk = new Queue<Func<Task>>();
        
        private bool _isPhaseRunning = false;


        private void OnEnable() {
            InputManager.OnClickOnTile += PhaseHandler;
        }

        private void OnDisable() {
            InputManager.OnClickOnTile -= PhaseHandler;
        }

        protected override void Awake() {
            base.Awake();
            GameObject.FindGameObjectWithTag("Player").TryGetComponent(out movementHandler);
        }

        private void Start() {
            //LoadLevel();
        }
        
        [Command]
        [Button]
        public void LoadLevel() {
            
            // Kill and reset everything
            ClearTempData();
            SkipCurrentPhaseBulk();
            _phaseQueue.Clear();
            DOTween.KillAll();
            movementHandler?.Reset();

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

       
        // MovementPhase
        // OnStepPhase
        // CountdownPhase
        // UnfreezePhase 
        private void PhaseHandler(TileGridObject pressedTile) {
            _phaseQueue.AddLast(CreatePhaseBulk(pressedTile));

            if (!_isPhaseRunning) {
                ProcessPhaseQueue();
            }
        }

        private Queue<Func<Task>> CreatePhaseBulk(TileGridObject pressedTile) {
            Queue<Func<Task>> phaseBulk = new Queue<Func<Task>>();
            phaseBulk.Enqueue(() => MovementPhase(pressedTile));
            // phaseBulk.Enqueue(() => DelayMethod(50));
            phaseBulk.Enqueue(() => StepOnTilePhase(pressedTile));
            //phaseBulk.Enqueue(() => DelayMethod(100));
            phaseBulk.Enqueue(CountdownPhase);
            phaseBulk.Enqueue(() => DelayMethod(200));
            phaseBulk.Enqueue(UnfreezePhase);
            //phaseBulk.Enqueue(() => DelayMethod(50));
            return phaseBulk;
        }

        private async void ProcessPhaseQueue() {
            try {
                _isPhaseRunning = true;
                while (_phaseQueue.Count > 0) {
                    _currentPhaseBulk = _phaseQueue.First.Value;
                    _phaseQueue.RemoveFirst();

                    while (_currentPhaseBulk.Count > 0) {
                        var currentPhase = _currentPhaseBulk.Dequeue();
                        await currentPhase();
                    }
                }

                await Task.Yield();
                _isPhaseRunning = false;
            } catch (Exception e) {
                throw e.GetBaseException();
            }
        }

        private void SkipCurrentPhaseBulk() {
            _currentPhaseBulk.Clear();
        }

        private void AddPhaseBulkFirst() {
            
            //TODO: Somehow create a new Phase Bulk with new pressedTile
            // This is needed for new push tile
            //_phaseQueue.AddFirst(CreatePhaseBulk());
        }


        /// <summary>
        /// MOVEMENT PHASE
        /// - Check if the pressed tile is valid and if player can go there
        ///     - If not => skip whole this phase
        /// - Move player to the target tile
        /// </summary>
        /// <param name="pressedTile"> Tile in the grid which player selects</param>
        private async Task MovementPhase(TileGridObject pressedTile) {
            if (!movementHandler.AddTargetTile(pressedTile)) {
                SkipCurrentPhaseBulk();
                return;
            }
            await movementHandler.MoveToNextPosition();
        }
        
        /// <summary>
        /// DELAY METHOD
        /// </summary>
        /// <param name="ms">How much ms want delay between phases</param>
        private static async Task DelayMethod(int ms) {
            await Task.Delay(ms);
        }

        
        /// <summary>
        /// STEP ON TILE PHASE
        /// - The press tile do its actions (freeze tile, collect, etc...) 
        /// </summary>
        /// <param name="pressedTile"></param>
        private async Task StepOnTilePhase(TileGridObject pressedTile) {
            await pressedTile.GetTile()?.OnPlayerStep()!;
            await Task.Delay(100);
        }
        
        /// <summary>
        /// COUNTDOWN PHASE
        /// - All countdown tiles decrease their value
        /// </summary>
        private async Task CountdownPhase() {
            foreach (CountdownTileBase tile in countdownTileBases) {
                await tile.OnDecreaseCountdownValue();
            }
        }

        
        /// <summary>
        /// UNFREEZE PHASE
        /// TODO: This should not be a game phase because it should be part of the AfterCountdownPhase 
        /// </summary>
        private async Task UnfreezePhase() {

            Grid<TileGridObject> grid = GridManager.Instance.GetGrid();
            
            foreach (TileData tile in levelData.tiles) {
                TileBase tileBase = grid.GetGridObject(tile.gridPosition).GetTile();

                if(!tileBase) 
                    continue;
                
                if (tileBase.tileVisualHandler?.CurrentMainState != TileMainVisualStates.FreezeState)
                    continue;
                
                if (!tileBase.TryGetComponent(out TileFreezeHandler tileFreezeHandler))
                    continue;
                
                tileBase.tileAnimationHandler?.FreezeAnimation();
                tileFreezeHandler?.UnfreezeTile();
                await Task.Delay(200);
            }
            await Task.Yield();
        }
    }
}