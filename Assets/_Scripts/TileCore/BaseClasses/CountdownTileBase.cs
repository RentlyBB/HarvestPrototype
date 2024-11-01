using System;
using _Scripts.CustomTools;
using UnityEngine;

namespace _Scripts.TileCore.BaseClasses {
    public abstract class CountdownTileBase : TileBase {

        public int countdownValue;

        [RequireVariable]
        public GameObject badCollectTile, goodCollectTile;

        protected override void Awake() {
            base.Awake();
            badCollectTile = Resources.Load<GameObject>("TilePrefabs/BadCollectTile");
            goodCollectTile = Resources.Load<GameObject>("TilePrefabs/GoodCollectTile");
        }

        protected abstract void CheckCountdown();

    }
}