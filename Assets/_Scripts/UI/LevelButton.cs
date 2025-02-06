using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Scripts.UI {
    public class LevelButton : MonoBehaviour {

        [FormerlySerializedAs("btnControlId")]
        public int btnLevelID;
        
        public static event UnityAction<int> OnLevelSelected = delegate { };

        public void BtnClicked() {
            OnLevelSelected?.Invoke(btnLevelID);
        }
    }
}