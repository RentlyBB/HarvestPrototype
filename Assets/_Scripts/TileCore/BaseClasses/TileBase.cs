using System.Threading.Tasks;
using _Scripts.TileCore.Enums;
using DG.Tweening;
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
            
        public virtual async Task OnPlayerStep() {
            tileVisualHandler?.ProcessVisualChange(null, TileSubVisualStates.Pressed);
            await Task.Yield();
        }

        public virtual async Task OnPlayerStepAfterDecreasing() {
            
            
            await Task.Yield();
        }
        
        public virtual async Task OnPlayerLeave() {
            await Task.Delay(230);
            tileVisualHandler?.ProcessVisualChange(null, TileSubVisualStates.Unpressed);
        }
        
        
    }
}