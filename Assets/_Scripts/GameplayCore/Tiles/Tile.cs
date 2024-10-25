using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.GameplayCore.Tiles {
    public abstract class Tile : MonoBehaviour{

        protected SpriteRenderer Renderer;
        protected Sprite DefaultSprite, FreezeSprite;
        
        public Vector2Int gridPosition;
        public bool moveable;

        protected Tile() {
            
        }

        protected virtual void Awake() {
            TryGetComponent<SpriteRenderer>(out Renderer);
        }

        public abstract void OnStep();

        public abstract void DuringStep();

        public abstract void AfterStep();
    }
}