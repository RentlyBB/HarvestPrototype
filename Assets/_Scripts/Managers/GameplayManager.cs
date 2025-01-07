using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.GridCore;
using _Scripts.PlayerCore;
using _Scripts.UnitySingleton;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Managers {
    public class GameplayManager : MonoSingleton<GameplayManager> {

        public static event UnityAction<TileGridObject> OnPlayerMovement = delegate { };
        public static event UnityAction OnCountdownDecreasing = delegate { };
        public static event UnityAction OnUnfreezeTiles = delegate { };
        
        private void OnEnable() {
            InputManager.OnClickOnTile += MovementPhase;
        }
      
        private void OnDisable() {
            InputManager.OnClickOnTile -= MovementPhase;
        }
        
        private void MovementPhase(TileGridObject pressedTile) {
            Debug.Log("Player Move phase started.");
            OnPlayerMovement?.Invoke(pressedTile);
            Debug.Log("Player Move phase completed.");
        }
       
    }
}