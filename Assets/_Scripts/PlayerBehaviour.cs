using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts {
    public class PlayerBehaviour : MonoBehaviour {

        public float speed; // Speed of the movement
        public float smoothTime = 0.3f; // How smooth the movement is
        public float targetThreshold = 0.1f; // Distance to consider as "reached"

        public bool orthogonalMovement = false;
        private Vector2Int _targetPosition;
        private Vector2 _velocity = Vector2.zero; // Used by SmoothDamp for smooth movement

        public List<Vector2Int> _nextTargetPosition = new List<Vector2Int>();

        public static event UnityAction DegreaseTileValueEvent = delegate { };

        private void OnEnable() {
            Tile.PlayerMove += SetTargetPosition;
        }

        private void OnDisable() {
            Tile.PlayerMove -= SetTargetPosition;
        }

        private void Start() {
        }

        private void Update() {
           
            MoveToTarget();
        }

        

        private void SetTargetPosition(Vector2Int pos) {
            Vector2Int _lastPos;
            if (_nextTargetPosition.Count > 0) {
                _lastPos = _nextTargetPosition[^1];
            } else {
                _lastPos = _targetPosition;
            }

            if (!ValidateNextMove(_lastPos, pos)) return;

            _nextTargetPosition.Add(pos);
        }

        public void SetPosition(Vector2 position) {
            _targetPosition = new Vector2Int((int)position.x, (int)position.y);
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }

        private void MoveToTarget() {
            
            if(_nextTargetPosition.Count == 0) return;
            
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
            //Harvest
            Tile tile = GridManager.Instance.GetTileAtPosition(_targetPosition);

            tile.OnTileStep();
            DegreaseTileValueEvent?.Invoke();
            tile.OnTileStepAfter();
        }


        private bool ValidateNextMove(Vector2Int currentPos, Vector2Int nextPos) {

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
    }
}