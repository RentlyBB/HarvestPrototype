using System.Threading.Tasks;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class RepeatCountdownTile : CountdownTileBase {

        private bool _repeated = false;
        private int _originalCountdownValue;

        public override void SetupTile() {
            _repeated = false;
            _originalCountdownValue = countdownValue;
            tileVisualHandler?.ProcessVisualChange(TileMainVisualStates.DefaultState, TileSubVisualStates.Unpressed);
            tileTextHandler?.AddText(countdownValue.ToString(), 72, new Color32(215,195,29, 255));
            countdownState = CountdownState.Counting;
        }

        public override async Task OnPlayerStep() {
            await base.OnPlayerStep();

            if (countdownState == CountdownState.Collected) return;

            if (!_repeated) {
                RepeatCountdown();
                return;
            }
            
            if (countdownState == CountdownState.ReadyToCollect) {
                GoodCollect();
            } else {
                BadCollect();
            }

            await Task.Yield();
        }
        
        private void RepeatCountdown() {
            countdownState = CountdownState.Counting;
            countdownValue = _originalCountdownValue;
            tileTextHandler?.RemoveText();
            tileTextHandler?.AddText(countdownValue.ToString(), 72, new Color32(61,61,61,255));
            tileVisualHandler?.ProcessVisualChange(TileMainVisualStates.DefaultState2, null);
            _repeated = true;
            SkipNextDecrease();

            Debug.Log("GOOD Collect and REPEAT!");
        }
    }
}