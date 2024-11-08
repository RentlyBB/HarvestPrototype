using System;
using System.Collections;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {

    public enum CountdownState {
        Counting,
        ReadyToCollect,
        Collected,
    }

    [RequireComponent(typeof(TileTextHandler))]
    public sealed class CountdownTile : TileBase {
        
        public int countdownValue;

        private TileTextHandler _tileTextHandler;
        private CountdownState _countdownState;
        
        private void OnEnable() {
            GameplayManager.CountdownDecreasing += DecreaseCountdownValue;
        }

        private void OnDisable() {
            GameplayManager.CountdownDecreasing -= DecreaseCountdownValue;
        }
        
        protected override void Awake() {
            base.Awake();
            TryGetComponent(out _tileTextHandler);
        }

        private void Start() {
            TileVisualHandler.SetMainAndSubState(TileMainVisualStates.Countdown, TileSubVisualStates.Unpressed);
            _tileTextHandler.AddText(countdownValue.ToString());
            _countdownState = CountdownState.Counting;
        }

        private void DecreaseCountdownValue() {
            
            if(_countdownState == CountdownState.Collected) return;

            if (_countdownState is CountdownState.Counting or CountdownState.ReadyToCollect) {
                countdownValue--;
                _tileTextHandler.UpdateText(countdownValue.ToString());
            }

            if (countdownValue == 0) {
               _countdownState = CountdownState.ReadyToCollect;
               _tileTextHandler.RemoveText();
               
               TileVisualHandler.SetMainState(TileMainVisualStates.Collect);
            } else if(countdownValue < 0) {
                _countdownState = CountdownState.Collected;
                TileVisualHandler.SetMainState(TileMainVisualStates.BadCollect);
                Debug.Log("BAD COLLECT! EVENT");
            }

        }

        public override void OnPlayerStep() {
            base.OnPlayerStep();

            if (_countdownState == CountdownState.Collected) {
                return;
            }

            if (_countdownState == CountdownState.ReadyToCollect) {
                TileVisualHandler.SetMainState(TileMainVisualStates.Collect);
                Debug.Log("GOOD COLLECT!");
            } else {
                TileVisualHandler.SetMainState(TileMainVisualStates.BadCollect);
                Debug.Log("BAD COLLECT!");
            }
            
            _countdownState = CountdownState.Collected;
        }
    }
}