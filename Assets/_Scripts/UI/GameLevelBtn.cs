using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.UI {
    public class GameLevelBtn : MonoBehaviour {

        public int btnControlId;
        
        public static event UnityAction<int> GameControlEvent = delegate { };


        public void BtnClicked() {
            GameControlEvent?.Invoke(btnControlId);
        }
    }
}