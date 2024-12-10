using _Scripts.Managers;
using _Scripts.TileCore.Enums;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        public TileTextHandler tileTextHandler;
        private bool _skipDecrease = false; 
        
        protected virtual void OnEnable() {
            GameplayManager.CountdownDecreasing += OnDecreaseCountdownValue;
        }

        protected virtual void OnDisable() {
            GameplayManager.CountdownDecreasing -= OnDecreaseCountdownValue;
        }
        
        protected override void Awake() {
            base.Awake();
            TryGetComponent(out tileTextHandler);
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
            
            //Countdown animation
            if (countdownValue >= 0) {
                tileAnimationHandler.CountdownAnimation();
            }

            tileTextHandler.UpdateText(countdownValue.ToString());
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
            tileTextHandler.RemoveText();
               
            //tileVisualHandler.SetMainState(TileMainVisualStates.ReadyToCollect);
            tileVisualHandler.QueueVisualChange(TileMainVisualStates.ReadyToCollect, null);
        }
        
        protected virtual void GoodCollect() {
            countdownState = CountdownState.Collected;
            //tileVisualHandler.SetMainState(TileMainVisualStates.GoodCollect);
            tileVisualHandler.QueueVisualChange(TileMainVisualStates.GoodCollect, null);
            tileTextHandler.RemoveText();
            Debug.Log("GOOD COLLECT!");
        }
        
        protected virtual void BadCollect() {
            countdownState = CountdownState.Collected;
            //tileVisualHandler.SetMainState(TileMainVisualStates.BadCollect);
            tileVisualHandler.QueueVisualChange(TileMainVisualStates.BadCollect, null);
            tileTextHandler.RemoveText();
            Debug.Log("BAD COLLECT!");
        }

        public void SkipNextDecrease() {
            _skipDecrease = true;
        }
    }
}