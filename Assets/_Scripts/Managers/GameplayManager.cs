using System;
using _Scripts.GridCore;
using _Scripts.TileCore.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnitySingleton;

namespace _Scripts.Managers {
    public class GameplayManager : PersistentMonoSingleton<GameplayManager> {

        public static UnityAction CountdownDecreasing = delegate { };

        // This method handles the all phases during puzzle solving
        // It know on which tile player end up and know that the player actually stop moving. 
        public void PhaseHandler(TileGridObject activatedTile) {

            activatedTile.GetTile().TryGetComponent(out IInteractableTile interactableTile);
            interactableTile?.OnPlayerStep();
            
            CountdownDecreasing?.Invoke();
        }
    }
}