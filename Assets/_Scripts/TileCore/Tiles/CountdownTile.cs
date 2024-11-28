using System;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {

    public sealed class CountdownTile : CountdownTileBase {
        
        private void Start() {
            tileVisualHandler.SetMainAndSubState(TileMainVisualStates.Countdown, TileSubVisualStates.Unpressed);
            tileTextHandler.AddText(countdownValue.ToString(), 72, Color.green);
            countdownState = CountdownState.Counting;
        }

        public override void OnPlayerStep() {
            base.OnPlayerStep();

            if(countdownState == CountdownState.Collected) return;

            if (countdownState == CountdownState.ReadyToCollect) {
                GoodCollect();
            } else {
                BadCollect();
            }
        }
        public override void OnPlayerStepAfterDecreasing() {
        }
    }
}