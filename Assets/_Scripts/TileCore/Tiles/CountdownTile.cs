using System;
using System.Threading.Tasks;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {

    public sealed class CountdownTile : CountdownTileBase {
        
        public override void SetupTile() {
            tileVisualHandler?.ProcessVisualChange(TileMainVisualStates.DefaultState, TileSubVisualStates.Unpressed);
            tileTextHandler?.AddText(countdownValue.ToString(), 72, new Color32(61,61,61,255));
            countdownState = CountdownState.Counting;
        }

        public override async Task OnPlayerStep() {
            await base.OnPlayerStep();

            if(countdownState == CountdownState.Collected) return;

            if (countdownState == CountdownState.ReadyToCollect) {
                GoodCollect();
            } else {
                BadCollect();
            }

            await Task.Yield();
        }
    }
}