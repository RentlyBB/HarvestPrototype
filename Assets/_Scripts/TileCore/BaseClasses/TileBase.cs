using _Scripts.TileCore.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.TileCore.BaseClasses {
    [RequireComponent(typeof(TileVisualHandler))]
    public abstract class TileBase : MonoBehaviour{

        public bool canMoveOn;
        public Vector2Int gridPosition;
        
        public TileVisualHandler tileVisualHandler; // Handling a visual of the tile

        protected virtual void Awake() {
            TryGetComponent(out tileVisualHandler);
            canMoveOn = true;
        }
        
        public virtual void OnPlayerStep() {
            tileVisualHandler.SetSubState(TileSubVisualStates.Pressed);
        }

        public virtual void OnPlayerStepAfterDecreasing() {
            
        }


        public virtual void OnPlayerLeave() {
            tileVisualHandler.SetSubState(TileSubVisualStates.Unpressed);
        }
    }
}