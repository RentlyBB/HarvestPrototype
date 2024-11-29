using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class RepeatCountdownTile : CountdownTileBase {

        private bool _repeated = false;
        private int _originalCountdownValue;

        private void Start() {
            _repeated = false;
            _originalCountdownValue = countdownValue;
            tileVisualHandler.SetMainAndSubState(TileMainVisualStates.ReadyToCollect, TileSubVisualStates.Unpressed);
            tileTextHandler.AddText(countdownValue.ToString(), 72, Color.green);
            countdownState = CountdownState.Counting;
        }

        protected override void UpdateCountdownValue() {
            if (countdownState is not (CountdownState.Counting or CountdownState.ReadyToCollect))
                return;

            countdownValue--;
            tileTextHandler.UpdateText(countdownValue.ToString());
        }

        public override void OnPlayerStep() {
            base.OnPlayerStep();

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
        }
        
        public override void OnPlayerStepAfterDecreasing() {
        }

        private void RepeatCountdown() {
            countdownState = CountdownState.Counting;
            countdownValue = _originalCountdownValue;
            tileTextHandler.RemoveText();
            tileTextHandler.AddText(countdownValue.ToString(), 72, Color.green);
            tileVisualHandler.SetMainState(TileMainVisualStates.DefaultState2);
            _repeated = true;
            SkipNextDecrease();

            Debug.Log("GOOD Collect and REPEAT!");
        }
    }
}