using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.GridCore;
using _Scripts.TileCore.BaseClasses;
using _Scripts.UnitySingleton;
using UnityEngine.Events;
using UnitySingleton;

namespace _Scripts.Managers {
    public class GameplayManager : MonoSingleton<GameplayManager> {

        public static UnityAction CountdownDecreasing = delegate { };
        public static UnityAction UnfreezeTiles = delegate { };

        private readonly Queue<TileGridObject> _phaseQueue = new Queue<TileGridObject>();
        private bool _isPhaseRunning = false;

        /// <summary>
        /// Handles the phases during puzzle solving.
        /// </summary>
        /// <param name="pressedTile">The tile that was pressed.</param>
        public void PhaseHandler(TileGridObject pressedTile) {
            _phaseQueue.Enqueue(pressedTile);

            if (!_isPhaseRunning) {
                StartCoroutine(ProcessPhaseQueue());
            }
        }
        
        private IEnumerator ProcessPhaseQueue() {
            _isPhaseRunning = true;

            while (_phaseQueue.Count > 0) {
                var pressedTile = _phaseQueue.Dequeue();

                // Get the TileBase component
                pressedTile.GetTile().TryGetComponent(out TileBase tileBase);

                // Phase #1: Unfreeze Tiles
                UnfreezeTiles?.Invoke();
                yield return null;

                // Phase #2: Player steps on the tile
                tileBase?.OnPlayerStep();
                yield return null;

                // Phase #3: Countdown Decreasing
                CountdownDecreasing?.Invoke();
                yield return null;

                // Phase #4: After countdown, any post-step logic
                tileBase?.OnPlayerStepAfterDecreasing();
                yield return null;
            }

            _isPhaseRunning = false;
        }
    }
}