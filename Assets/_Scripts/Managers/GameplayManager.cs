using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.GridCore;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Tiles;
using _Scripts.UnitySingleton;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Managers {
    public class GameplayManager : MonoSingleton<GameplayManager> {

        public static UnityAction CountdownDecreasing = delegate { };
        public static UnityAction UnfreezeTiles = delegate { };
        public static UnityAction<TileGridObject> MovePlayer = delegate { };

        private readonly Queue<Func<Task>> _phaseQueue = new Queue<Func<Task>>();
        private bool _isPhaseRunning = false;

        private void OnEnable() {
            InputManager.OnClickOnTile += PhaseHandler;
        }
      
        private void OnDisable() {
            InputManager.OnClickOnTile -= PhaseHandler;
        }
        
        public void PhaseHandler(TileGridObject pressedTile) {
            
            _phaseQueue.Enqueue(() => UnfreezePhase());
            _phaseQueue.Enqueue(() => PlayerMovePhase(pressedTile));
            _phaseQueue.Enqueue(() => FreezePhase(pressedTile));
            _phaseQueue.Enqueue(() => PlayerStepPhase(pressedTile));
            _phaseQueue.Enqueue(() => CountdownPhase());
            _phaseQueue.Enqueue(() => PostStepPhase(pressedTile));

            if (!_isPhaseRunning) {
                ProcessPhaseQueue();
            }
        }

        private async void ProcessPhaseQueue() {
            _isPhaseRunning = true;
            while (_phaseQueue.Count > 0) {
                var currentPhase = _phaseQueue.Dequeue();
                await currentPhase();
                await Task.Delay(50);
            }
            _isPhaseRunning = false;
        }

        private async Task UnfreezePhase() {
            Debug.Log("Unfreeze phase started.");
            UnfreezeTiles?.Invoke();
            await Task.Delay(100);
            Debug.Log("Unfreeze phase completed.");
        }

        private async Task PlayerMovePhase(TileGridObject pressedTile) {
            Debug.Log("Player Move phase started.");
            //Find player and make him move
            MovePlayer?.Invoke(pressedTile);
            //await to player move completed
            await Task.Delay(100);
            Debug.Log("Player Move phase completed.");
        }

        private async Task PlayerStepPhase(TileGridObject pressedTile) {
            Debug.Log("Player step phase started.");
            pressedTile.GetTile().TryGetComponent(out TileBase tileBase);
            tileBase?.OnPlayerStep();
            await Task.Delay(0);
            Debug.Log("Player step phase completed.");
        }
        
        private async Task FreezePhase(TileGridObject pressedTile) {
            Debug.Log("Freeze phase started.");
            if (pressedTile.GetTile().TryGetComponent(out FreezeVerticalTile freezeTile)) {
                await freezeTile.FreezeLine();
            }
            Debug.Log("Freeze phase completed.");
        }

        private async Task CountdownPhase() {
            Debug.Log("Countdown phase started.");
            await Task.Delay(0);
            CountdownDecreasing?.Invoke();
            Debug.Log("Countdown phase completed.");
        }

        private async Task PostStepPhase(TileGridObject pressedTile) {
            Debug.Log("Post-step phase started.");
            pressedTile.GetTile().TryGetComponent(out TileBase tileBase);
            tileBase?.OnPlayerStepAfterDecreasing();
            await Task.Delay(0);
            Debug.Log("Post-step phase completed.");
        }
    }
}