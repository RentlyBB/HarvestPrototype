using System;
using System.Collections.Generic;
using Enums;
using QFSW.QC;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts {
    public class GhostPlayerBehaviour : MonoBehaviour {
        public float speed; // Speed of the movement
        public float smoothTime = 0.3f; // How smooth the movement is
        public float targetThreshold = 0.1f; // Distance to consider as "reached"

        public bool orthogonalMovement = false;
        public Vector2Int _targetPosition;
        private Vector2 _velocity = Vector2.zero; // Used by SmoothDamp for smooth movement

        public List<Vector2Int> _nextTargetPosition = new List<Vector2Int>();
        public List<Vector2Int> _waitingTargetPosition = new List<Vector2Int>();

        public bool isBeingPushed = false;
        public bool reverseMovement = false;

        private void Update() {
            MoveToTarget();
        }
        
        private void SetTargetPosition(Vector2Int pos) {
            
            var nextPos = _targetPosition + pos;
            
            if (!ValidateNextMove(_targetPosition, nextPos)) {
                return;
            }
            _nextTargetPosition.Add(nextPos);
            
        }

        public void SetPosition(Vector2 position) {
            _targetPosition = new Vector2Int((int)position.x, (int)position.y);
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }

        private void MoveToTarget() {
            if (_nextTargetPosition.Count == 0) return;

            // Get the current position in Vector2 form (ignore Z)
            Vector2 currentPos2D = new Vector2(transform.position.x, transform.position.y);

            _targetPosition = _nextTargetPosition[0];

            // SmoothDamp only the X and Y axis
            Vector2 newPos2D = Vector2.SmoothDamp(currentPos2D, _targetPosition, ref _velocity, smoothTime, speed);

            // Apply the new position, keeping the original Z value
            transform.position = new Vector3(newPos2D.x, newPos2D.y, transform.position.z);

            // Check if the object has reached the target position
            if (Vector2.Distance(transform.position, _targetPosition) <= targetThreshold) {
                _nextTargetPosition.RemoveAt(0);
                OnReachedTarget(); // Call the method when the target is reached
            }
        }

        private void OnReachedTarget() {
            
        }

        private bool ValidateNextMove(Vector2Int currentPos, Vector2Int nextPos) {

            if (nextPos.x < 0 || nextPos.y < 0) return false;
            if (nextPos.x > (GridManager.Instance._width - 1) || nextPos.y > (GridManager.Instance._height - 1)) return false;
            
            //Check if we are already on tile
            if (currentPos.x == nextPos.x && currentPos.y == nextPos.y) return false;

            // Check horizontals and verticals
            if (Math.Abs(currentPos.x - nextPos.x) > 1) return false;
            if (Math.Abs(currentPos.y - nextPos.y) > 1) return false;

            // Check diagonals
            if (orthogonalMovement) {
                if (nextPos.x > currentPos.x && nextPos.y > currentPos.y) return false;
                if (nextPos.x < currentPos.x && nextPos.y < currentPos.y) return false;
                if (nextPos.x > currentPos.x && nextPos.y < currentPos.y) return false;
                if (nextPos.x < currentPos.x && nextPos.y > currentPos.y) return false;
            }

            return true;
        }

        [Command]
        private void ValidateWaitingMove() {
            if (_waitingTargetPosition.Count == 0) return;

            for (int i = 0; i < _waitingTargetPosition.Count; i++) {
                if (!ValidateNextMove(_targetPosition, _waitingTargetPosition[i])) {
                    //_nextTargetPosition.Add(_waitingTargetPosition[i]);
                    _waitingTargetPosition.RemoveAt(i);
                    Debug.Log("Validating with: " + _targetPosition);
                    i = -1;
                }
            }

            _nextTargetPosition = _waitingTargetPosition;
            _waitingTargetPosition = new List<Vector2Int>();
        }


        //Used when player step on the pushing tile
        public void ForceMove(Tile tile) {
            Vector2Int pushDirection = new Vector2Int();
            
            if (tile._pushDirection.Contains("U") && tile._pushDirection.Contains("R")) {
                // Move Up Right
                pushDirection = new Vector2Int(1,1);
            }else if (tile._pushDirection.Contains("U") && tile._pushDirection.Contains("L")) {
                // Move Up Left
                pushDirection = new Vector2Int(-1,1);
            } else if (tile._pushDirection.Contains("D") && tile._pushDirection.Contains("R")) {
                // Move Down Right
                pushDirection = new Vector2Int(1,-1);
            } else if (tile._pushDirection.Contains("D") && tile._pushDirection.Contains("L")) {
                // Move Down Left
                pushDirection = new Vector2Int(-1,-1);
            } else if (tile._pushDirection.Contains("U")) {
                // Move Up
                pushDirection = new Vector2Int(0,1);
            } else if (tile._pushDirection.Contains("D")) {
                // Move Down
                pushDirection = new Vector2Int(0,-1);
            } else if (tile._pushDirection.Contains("L")) {
                // Move L
                pushDirection = new Vector2Int(-1,0);
            } else if (tile._pushDirection.Contains("R")) {
                // Move R
                pushDirection = new Vector2Int(1,0);
            }
            
            _nextTargetPosition.Add( _targetPosition + pushDirection);
        }

        public void MoveByDirection(Directions direction) {
            Vector2Int nextMove = Vector2Int.zero;
            switch (direction) {
                case Directions.NONE:
                    nextMove = Vector2Int.zero;
                    break;
                case Directions.UP:
                    nextMove = new Vector2Int(0,-1);
                    break;
                case Directions.DOWN:
                    nextMove = new Vector2Int(0,1);
                    break;
                case Directions.LEFT:
                    nextMove = new Vector2Int(1,0);
                    break;
                case Directions.RIGHT:
                    nextMove = new Vector2Int(-1,0);
                    break;
                case Directions.UPLEFT:
                    nextMove = new Vector2Int(1,-1);
                    break;
                case Directions.UPRIGHT:
                    nextMove = new Vector2Int(-1,-1);
                    break;
                case Directions.DOWNLEFT:
                    nextMove = new Vector2Int(1,1);
                    break;
                case Directions.DOWNRIGHT:
                    nextMove = new Vector2Int(-1,1);
                    break;
                default:
                    nextMove = Vector2Int.zero;
                    break;
            }
            SetTargetPosition(nextMove);
        }
    }
}