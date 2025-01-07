using System.Collections;

namespace _Scripts.GamePhaseSystem {
    public abstract class GamePhase {
        public enum GamePhaseState {
            Waiting,
            InProgress,
            Completed
        }

        protected GamePhaseState currentState = GamePhaseState.Waiting;
        public GamePhaseState State => currentState;

        public virtual IEnumerator Execute() {
            currentState = GamePhaseState.InProgress;
            yield return RunGamePhase();
            currentState = GamePhaseState.Completed;
        }

        /// <summary>
        /// The actual command logic. 
        /// Override this in your subclass.
        /// </summary>
        protected abstract IEnumerator RunGamePhase();
    }
}