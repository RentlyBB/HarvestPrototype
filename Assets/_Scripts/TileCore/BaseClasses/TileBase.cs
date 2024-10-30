using UnityEngine;

namespace _Scripts.TileCore.BaseClasses {
    public abstract class TileBase : MonoBehaviour{

        public bool canMoveOn;
        
        protected TileStateHandler TileStateHandler; // Handling a stage of the tile
        protected TileVisualHandler TileVisualHandler; // Handling a visual of the tile
        protected TileSoundHandler TileSoundHandler; // Handling sounds of the tile

        protected virtual void Awake() {
            TryGetComponent(out TileStateHandler);
            TryGetComponent(out TileVisualHandler);
            TryGetComponent(out TileSoundHandler);
            canMoveOn = true;
        }
    }
}