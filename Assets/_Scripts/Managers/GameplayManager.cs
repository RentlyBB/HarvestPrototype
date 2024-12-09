using System;
using _Scripts.GridCore;
using _Scripts.TileCore.BaseClasses;
using UnityEngine.Events;
using UnitySingleton;

namespace _Scripts.Managers {
    public class GameplayManager : MonoSingleton<GameplayManager> {

        public static UnityAction CountdownDecreasing = delegate { };
        
        public static UnityAction UnfreezeTiles = delegate { };
        
        // This method handles the all phases during puzzle solving
        // It know on which tile player end up and know that the player actually stop moving. 
        public void PhaseHandler(TileGridObject pressedTile) {
            pressedTile.GetTile().TryGetComponent(out TileBase tileBase);
            
            //#1
            UnfreezeTiles?.Invoke();
            
            //#2
            tileBase?.OnPlayerStep();
            
            //#3
            CountdownDecreasing?.Invoke();
            
            //#4
            tileBase?.OnPlayerStepAfterDecreasing();
        }
    }
}