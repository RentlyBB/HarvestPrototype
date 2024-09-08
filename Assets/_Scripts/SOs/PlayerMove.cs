using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.SOs {
    [CreateAssetMenu(fileName = "MoveEvent", menuName = "Custom/MoveEvent", order = 0)]
    public class PlayerMoveEvent : ScriptableObject {
        
        public event UnityAction<Vector2> PlayerMove = delegate { };
        
        public void RaiseEvent(Vector2 pos) {
            PlayerMove?.Invoke(pos);
        }
        
    }
}