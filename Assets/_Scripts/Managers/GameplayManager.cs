using System;
using _Scripts.GridCore;
using UnityEngine;
using UnitySingleton;

namespace _Scripts.Managers {
    public class GameplayManager : PersistentMonoSingleton<GameplayManager> {

        public void PhaseHandler(TileGridObject tile) {
            Debug.Log("Zachyceno: " + tile.GetXY());
        }
    }
}