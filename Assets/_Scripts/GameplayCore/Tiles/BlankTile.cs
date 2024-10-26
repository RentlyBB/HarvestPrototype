using System;
using UnityEngine;

namespace _Scripts.GameplayCore.Tiles {
    public class BlankTile : TileCore {

        public BlankTile(int x, int y) {
            gridPosition = new Vector2(x, y);
            moveable = true;
        }
        

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