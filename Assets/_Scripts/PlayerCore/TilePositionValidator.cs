using System;
using UnityEngine;

namespace _Scripts.PlayerCore {
    public class TilePositionValidator : MonoBehaviour {
        
        public bool lockOrthogonalMovement = false;

        public bool ValidateNextTilePosition(Vector2Int currentPos, Vector2Int nextPos) {
            
            if (currentPos.x == nextPos.x && currentPos.y == nextPos.y) return false;

            // Check horizontals and verticals
            if (Math.Abs(currentPos.x - nextPos.x) > 1) return false;
            if (Math.Abs(currentPos.y - nextPos.y) > 1) return false;

            // Check diagonals
            if (lockOrthogonalMovement) {
                if (nextPos.x > currentPos.x && nextPos.y > currentPos.y) return false;
                if (nextPos.x < currentPos.x && nextPos.y < currentPos.y) return false;
                if (nextPos.x > currentPos.x && nextPos.y < currentPos.y) return false;
                if (nextPos.x < currentPos.x && nextPos.y > currentPos.y) return false;
            }
            
            return true;
        }
        
    }
}