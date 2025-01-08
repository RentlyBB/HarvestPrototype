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

        //TODO: Udelat lookupy vsech tilu co budou delat nejakej proces v bulku,
        // Vzdy pri nacteni levelu se udelaji list vsech countdown listeneru
        // Pri kazdem freeznuti se ulozi vsechny freeznute tile
        // A s temito lookupy nebo datami se bude operovat, bude lehci urcit kdy cely tento proccess skonci
        // protoze to bude cele centralizovane a budu o vsech vedet
        // eventy jsou fajn ale na toto to neni funkcni model.
        
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