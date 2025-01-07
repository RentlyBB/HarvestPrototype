using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Scripts.GamePhaseSystem {
    public class GamePhaseHandler : MonoBehaviour {
        private List<GamePhase> _phases = new List<GamePhase>();
        private bool _isRunning = false;


        public void AddGamePhase(GamePhase gamePhase) {
            _phases.Add(gamePhase);
        }

        public void RunPhases() {
            if (!_isRunning && _phases.Count > 0) {
                StartCoroutine(ProcessPhases());
            }
        }
        
        private IEnumerator ProcessPhases() {
            _isRunning = true;

            // Process each command in sequence
            foreach (var gamePhase in _phases)
            {
                yield return gamePhase.Execute();
            }

            // Done. Clear if you want to reuse the list.
            _phases.Clear();
            _isRunning = false;
        }

    }
}