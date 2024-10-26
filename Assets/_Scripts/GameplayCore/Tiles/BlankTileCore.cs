using System;
using UnityEngine;

namespace _Scripts.GameplayCore.Tiles {
    public class BlankTileCore : TileCore {
        
        protected override void Awake() {
            base.Awake();
            moveable = true;
        }

        public override void OnStep() {
            throw new NotImplementedException();
        }

        public override void DuringStep() {
            throw new NotImplementedException();
        }

        public override void AfterStep() {
            throw new NotImplementedException();
        }
    }
}