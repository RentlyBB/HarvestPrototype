using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.BaseClasses {
    [RequireComponent(typeof(TileVisualHandler))]
    public abstract class TileBase : MonoBehaviour{

        public bool canMoveOn;
        public Vector2Int gridPosition;
        
        protected TileVisualHandler TileVisualHandler; // Handling a visual of the tile

        protected virtual void Awake() {
            TryGetComponent(out TileVisualHandler);
            canMoveOn = true;
        }
        
        public virtual void OnPlayerStep() {
            TileVisualHandler.SetSubState(TileSubVisualStates.Pressed);
        }

        public virtual void OnPlayerStepAfterDecreasing() {
            
        }


        public virtual void OnPlayerLeave() {
            TileVisualHandler.SetSubState(TileSubVisualStates.Unpressed);
        }
    }
}