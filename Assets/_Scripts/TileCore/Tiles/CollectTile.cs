using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using _Scripts.TileCore.Interfaces;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class CollectTile : TileBase {

        private void OnEnable() {
            GameplayManager.CountdownDecreasing += NotCollect;
        }

        private void OnDisable() {
            GameplayManager.CountdownDecreasing -= NotCollect;
        }
        
        public override void OnPlayerStep() {
            base.OnPlayerStep();
            //Collected 
            // TODO: Inform LevelManager about it – because we have to check if level is done or not
            // LevelManager do not exist yet
            GridManager.Instance.ReplaceAndDestroyTileWith(gridPosition, TileType.DefaultTile);
        }
        
        
        private void NotCollect() {
            GridManager.Instance.ReplaceAndDestroyTileWith(gridPosition, TileType.BadCollectTile);
        }
        
    }
}