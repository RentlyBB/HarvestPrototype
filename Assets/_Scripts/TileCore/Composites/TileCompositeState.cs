using System;
using _Scripts.TileCore.Enums;

namespace _Scripts.TileCore.Composites {
    [Serializable]
    public struct TileCompositeState {
        public TileMainVisualStates mainState;
        public TileSubVisualStates subState;

        public TileCompositeState(TileMainVisualStates mainState, TileSubVisualStates subState) {
            this.mainState = mainState;
            this.subState = subState;
        }

        // Override Equals and GetHashCode for dictionary key compatibility
        public override bool Equals(object obj) {
            if (obj is TileCompositeState other) {
                return mainState == other.mainState && subState == other.subState;
            }
            return false;
        }

        public override int GetHashCode() {
            return (int)mainState * 397 ^ (int)subState;
        }
    }
}