﻿using System.Threading.Tasks;
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
        public TileFreezeHandler tileFreezeHandler;
        private bool _skipDecrease = false; 
        
        protected override void Awake() {
            base.Awake();
            TryGetComponent(out tileTextHandler);
            TryGetComponent(out tileFreezeHandler);
        }
        
        public virtual async Task OnDecreaseCountdownValue() {
            if(countdownState == CountdownState.Collected) return;

            if (_skipDecrease) {
                _skipDecrease = false;

                // unfreeze tile only if its freeze
                if (tileVisualHandler.CurrentMainState != TileMainVisualStates.FreezeState) return;
                tileAnimationHandler?.FreezeAnimation();
                tileFreezeHandler?.UnfreezeTile();

                return;
            }

            await UpdateCountdownValue();
            await UpdateStateAfterDecreasing();
        }

        protected virtual async Task UpdateCountdownValue() {
            if (countdownState is not (CountdownState.Counting or CountdownState.ReadyToCollect))
                return;

            countdownValue--;
            
            //Countdown animation
            if (countdownValue >= 0) {
                tileAnimationHandler?.CountdownAnimation();
            }

            tileTextHandler?.UpdateText(countdownValue.ToString());

            await Task.Yield();
        }

        protected virtual async Task UpdateStateAfterDecreasing() {
            if (countdownValue == 0) {
                ReadyToCollect();
            } else if(countdownValue < 0) {
                BadCollect();
            }

            await Task.Yield();
        }

        protected virtual void ReadyToCollect() {
            countdownState = CountdownState.ReadyToCollect;
            tileTextHandler?.RemoveText();
            tileVisualHandler?.ProcessVisualChange(TileMainVisualStates.ReadyToCollect, null);
        }
        
        protected virtual void GoodCollect() {
            countdownState = CountdownState.Collected;
            tileVisualHandler?.ProcessVisualChange(TileMainVisualStates.GoodCollect, null);
            tileTextHandler?.RemoveText();
            Debug.Log("GOOD COLLECT!");
        }
        
        protected virtual void BadCollect() {
            countdownState = CountdownState.Collected;
            tileVisualHandler?.ProcessVisualChange(TileMainVisualStates.BadCollect, null);
            tileTextHandler?.RemoveText();
            Debug.Log("BAD COLLECT!");
        }

        public void SkipNextDecrease() {
            _skipDecrease = true;
        }
    }
}