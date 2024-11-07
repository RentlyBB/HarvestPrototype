using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.BaseClasses {
    public abstract class TileBase : MonoBehaviour{

        public bool canMoveOn;
        public Vector2Int gridPosition;
        
        protected TileStateHandler TileStateHandler; // Handling a stage of the tile
        protected TileVisualHandler TileVisualHandler; // Handling a visual of the tile
        protected TileSoundHandler TileSoundHandler; // Handling sounds of the tile

        protected virtual void Awake() {
            TryGetComponent(out TileStateHandler);
            TryGetComponent(out TileVisualHandler);
            TryGetComponent(out TileSoundHandler);
            canMoveOn = true;
        }

        public virtual void OnPlayerStep() {
            TileVisualHandler.SetSubState(TileSubVisualStates.Pressed);
        }

        public virtual void OnPlayerLeave() {
            TileVisualHandler.SetSubState(TileSubVisualStates.Unpressed);
        }
    }
}