using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.TileCore.Enums;
using _Scripts.TileCore.ScriptableObjects;
using UnityEngine;
using VInspector;

namespace _Scripts.TileCore {
    [RequireComponent(typeof(SpriteRenderer))]
    public class TileVisualHandler : MonoBehaviour {
        public TileVisualData tileVisualData; // Assign this in the Inspector

        public TileMainVisualStates CurrentMainState { get; private set; }
        public TileSubVisualStates CurrentSubState { get; private set; }

        private SpriteRenderer _spriteRenderer;

        // Queue for visual change tasks
        private Queue<VisualChangeTask> _taskQueue = new Queue<VisualChangeTask>();
        private bool _isProcessingQueue = false;

        private void Awake() {
            CurrentSubState = TileSubVisualStates.Unpressed;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void UpdateSprite() {
            if (tileVisualData is null) {
                Debug.LogError("TileVisualData is not assigned!");
                return;
            }

            HandleTransparency();

            // Retrieve and apply the appropriate sprite for the current composite state
            Sprite sprite = tileVisualData.GetSprite(CurrentMainState, CurrentSubState);
            _spriteRenderer.sprite = sprite;
        }

        private void HandleTransparency() {
            if (CurrentMainState is TileMainVisualStates.Empty) {
                Color col = _spriteRenderer.color;
                col.a = 0;
                _spriteRenderer.color = col;
                return;
            }

            if (_spriteRenderer.color.a == 0) {
                Color color = _spriteRenderer.color;
                color.a = 255;
                _spriteRenderer.color = color;
            }
        }

        /// <summary>
        /// Add a visual change task to the queue. Specify null for states you don't want to change.
        /// </summary>
        /// <param name="newMainState">The new main state to set, or null to keep the current state.</param>
        /// <param name="newSubState">The new sub state to set, or null to keep the current state.</param>
        /// <param name="delay">Optional delay before applying this change.</param>
        public void QueueVisualChange(TileMainVisualStates? newMainState, TileSubVisualStates? newSubState, float delay = 0f) {
            _taskQueue.Enqueue(new VisualChangeTask(newMainState, newSubState, delay));
            if (!_isProcessingQueue) {
                StartCoroutine(ProcessQueue());
            }
        }

        /// <summary>
        /// Process the visual change tasks in the queue.
        /// </summary>
        private IEnumerator ProcessQueue() {
            _isProcessingQueue = true;

            while (_taskQueue.Count > 0) {
                var task = _taskQueue.Dequeue();

                // Wait for the optional delay
                if (task.Delay > 0) {
                    yield return new WaitForSeconds(task.Delay);
                }

                // Apply the visual change (only change states that are not null)
                if (task.MainState.HasValue) {
                    CurrentMainState = task.MainState.Value;
                }

                if (task.SubState.HasValue) {
                    CurrentSubState = task.SubState.Value;
                }

                UpdateSprite();
            }

            _isProcessingQueue = false;
        }

        // public void SetMainAndSubState(TileMainVisualStates newMainState, TileSubVisualStates newSubState) {
        //     CurrentMainState = newMainState;
        //     CurrentSubState = newSubState;
        //     UpdateSprite();
        // }
        //
        // public void SetMainState(TileMainVisualStates newMainState) {
        //     CurrentMainState = newMainState;
        //     UpdateSprite();
        // }
        //
        // public void SetSubState(TileSubVisualStates newSubState) {
        //     if (CurrentMainState is TileMainVisualStates.Empty) return;
        //
        //     CurrentSubState = newSubState;
        //     UpdateSprite();
        // }

        // Internal class for storing visual change tasks
        private class VisualChangeTask {
            public TileMainVisualStates? MainState { get; }
            public TileSubVisualStates? SubState { get; }
            public float Delay { get; }

            public VisualChangeTask(TileMainVisualStates? mainState, TileSubVisualStates? subState, float delay) {
                MainState = mainState;
                SubState = subState;
                Delay = delay;
            }
        }
    }
}