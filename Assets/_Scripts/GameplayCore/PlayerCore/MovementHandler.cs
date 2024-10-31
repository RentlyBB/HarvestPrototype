using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.GameplayCore.PlayerCore {

    public enum MovementState {
        Waiting,
        Moving,
    }

    public class MovementHandler : MonoBehaviour {
        public float speed; // Speed of the movement
        public float smoothTime = 0.3f; // How smooth the movement is
        public float targetThreshold = 0.1f; // Distance to consider as "reached"
        public bool orthogonalMovement = false;
        
        private Vector2 _targetPosition;
        private Vector2 _velocity = Vector2.zero; // Used by SmoothDamp for smooth movement
        private List<Vector2> _nextTargetPosition = new List<Vector2>();
        private bool _getNewTarget = true;
        
        private MovementState _movementState = MovementState.Waiting;

        private void Update() {
            //TODO: Toto chci predelat, tak aby kdyz hrac klikna na tile,
            // tak se tily prida do listu pres metodu SetTargetPosition a v ten moment se tam pusti script MoveToTarget()
            // ktery pokazde kdyz dojede, tedy hrac reachne target, tak zkontroluje jestli neni v listu dalsi target
            // pokud bude tak se bude pokracovat v movementu
            // pokud ne tak se ukonci a prepne se state z Moving na Wainting
            // muzes pouzit while-do
            MoveToTarget();
        }

        public void SetTargetPosition(Vector2 pos) {
            _nextTargetPosition.Add(pos);
        }
    
        public void SetPosition(Vector2 position) {
            _targetPosition = new Vector2(position.x, position.y);
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }

        private void MoveToTarget() {
            if (_nextTargetPosition.Count == 0) return;

            // Get the current position in Vector2 form (ignore Z)
            Vector2 currentPos2D = new Vector2(transform.position.x, transform.position.y);

            if (_getNewTarget) {
                if (!ValidateNextMove(_targetPosition, _nextTargetPosition[0])) {
                    _nextTargetPosition.RemoveAt(0);
                } else {
                    _getNewTarget = false;
                }
            }

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
            _getNewTarget = true;
        }
        
        private bool ValidateNextMove(Vector2 currentPos, Vector2 nextPos) {
            //Check if we are already on tile
            if (Mathf.Approximately(currentPos.x, nextPos.x) && Mathf.Approximately(currentPos.y, nextPos.y)) return false;

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