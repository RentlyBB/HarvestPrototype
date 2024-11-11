using _Scripts.Managers;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.BaseClasses {
    
    public enum CountdownState {
        Counting,
        ReadyToCollect,
        Collected,
    }
    
    [RequireComponent(typeof(TileTextHandler))]
    public abstract class CountdownTileBase : TileBase {
        
        public int countdownValue;
        
        public CountdownState countdownState;
        
        protected TileTextHandler TileTextHandler;
        private bool _skipDecrease = false; 
        
        protected virtual void OnEnable() {
            GameplayManager.CountdownDecreasing += OnDecreaseCountdownValue;
        }

        protected virtual void OnDisable() {
            GameplayManager.CountdownDecreasing -= OnDecreaseCountdownValue;
        }
        
        protected override void Awake() {
            base.Awake();
            TryGetComponent(out TileTextHandler);
        }
        
        protected virtual void OnDecreaseCountdownValue() {
            if(countdownState == CountdownState.Collected) return;

            if (_skipDecrease) {
                _skipDecrease = false;
                return;
            }

            UpdateCountdownValue();
            UpdateStateAfterDecreasing();
        }

        protected virtual void UpdateCountdownValue() {
            if (countdownState is not (CountdownState.Counting or CountdownState.ReadyToCollect))
                return;

            countdownValue--;
            TileTextHandler.UpdateText(countdownValue.ToString());
        }

        protected virtual void UpdateStateAfterDecreasing() {
            if (countdownValue == 0) {
                ReadyToCollect();
            } else if(countdownValue < 0) {
                BadCollect();
            }
        }

        protected virtual void ReadyToCollect() {
            countdownState = CountdownState.ReadyToCollect;
            TileTextHandler.RemoveText();
               
            TileVisualHandler.SetMainState(TileMainVisualStates.ReadyToCollect);
        }
        
        protected virtual void GoodCollect() {
            countdownState = CountdownState.Collected;
            TileVisualHandler.SetMainState(TileMainVisualStates.GoodCollect);
            TileTextHandler.RemoveText();
            Debug.Log("GOOD COLLECT!");
        }
        
        protected virtual void BadCollect() {
            countdownState = CountdownState.Collected;
            TileVisualHandler.SetMainState(TileMainVisualStates.BadCollect);
            TileTextHandler.RemoveText();
            Debug.Log("BAD COLLECT!");
        }

        protected void SkipNextDecrease() {
            _skipDecrease = true;
        }
    }
}