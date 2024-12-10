using System;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {

    public sealed class CountdownTile : CountdownTileBase {
        
        public override void SetupTile() {
            // tileVisualHandler.SetMainAndSubState(TileMainVisualStates.DefaultState, TileSubVisualStates.Unpressed);
            tileVisualHandler.QueueVisualChange(TileMainVisualStates.DefaultState, TileSubVisualStates.Unpressed);
            tileTextHandler.AddText(countdownValue.ToString(), 72, new Color32(61,61,61,255));
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