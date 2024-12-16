using _Scripts.TileCore.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.TileCore.BaseClasses {
    [RequireComponent(typeof(TileVisualHandler), typeof(TileAnimationHandler))]
    public abstract class TileBase : MonoBehaviour{

        public bool canMoveOn;
        public Vector2Int gridPosition;
        
        public TileVisualHandler tileVisualHandler; // Handling a visual of the tile
        public TileAnimationHandler tileAnimationHandler;

        protected virtual void Awake() {
            TryGetComponent(out tileVisualHandler);
            TryGetComponent(out tileAnimationHandler);
            canMoveOn = true;
        }

        
        /// <summary>
        /// This is a setup method that is called when the tile spawns into the grid. Use this method instead of Unity’s Start() method.
        /// This method is called only in GridManager script.
        /// </summary>
        public abstract void SetupTile();
            
        public virtual void OnPlayerStep() {
            tileVisualHandler.QueueVisualChange(null, TileSubVisualStates.Pressed);
        }

        public virtual void OnPlayerStepAfterDecreasing() {
            
        }
        
        public virtual void OnPlayerLeave() {
            tileVisualHandler.QueueVisualChange(null, TileSubVisualStates.Unpressed);
        }
        
        
    }
}