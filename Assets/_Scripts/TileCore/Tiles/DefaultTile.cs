using System;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class DefaultTile : TileBase {
        protected override void Awake() {
            base.Awake();
        }

        private void Start() {
            TileVisualHandler.SetMainAndSubState(TileMainVisualStates.Default, TileSubVisualStates.Unpressed);
        }

        public override void OnPlayerStep() {
            base.OnPlayerStep();
            
        }

        public override void OnPlayerLeave() {
            base.OnPlayerLeave();
        }
    }
}