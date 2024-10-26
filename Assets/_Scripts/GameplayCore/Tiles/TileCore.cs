using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.GameplayCore.Tiles {
    public abstract class TileCore : MonoBehaviour{

        protected SpriteRenderer Renderer;
        protected Sprite DefaultSprite, FreezeSprite;
        
        public Vector2 gridPosition;
        public bool moveable;

        protected virtual void Awake() {
            TryGetComponent<SpriteRenderer>(out Renderer);
        }

        public abstract void OnStep();

        public abstract void DuringStep();

        public abstract void AfterStep();
    }
}