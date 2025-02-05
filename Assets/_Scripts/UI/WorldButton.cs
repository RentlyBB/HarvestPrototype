using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.UI {
    public class WorldButton : MonoBehaviour {
        public int btnWorldID;
        
        public static event UnityAction<int> OnWorldSelected = delegate { };

        public void BtnClicked() {
            OnWorldSelected?.Invoke(btnWorldID);
        }
    }
}