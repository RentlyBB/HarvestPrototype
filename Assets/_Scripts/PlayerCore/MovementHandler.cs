using System.Collections.Generic;
using _Scripts.GridCore;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.PlayerCore {
    [RequireComponent(typeof(TilePositionValidator))]
    public class MovementHandler : MonoBehaviour {
        
        [Header("Movement Settings")]
        public float maxSpeed = 75; // Maximum speed for movement
        public float movementSmoothTime = 0.05f; // Smoothing time for movement
        public float targetReachThreshold = 0.0005f; // Distance to consider the target reached
        
        private Vector2Int _currentGridPosition = Vector2Int.zero;
        private Vector2 _targetWorldPosition;
        private Vector2 _velocity = Vector2.zero; // Used by SmoothDamp for smooth movement
        
        private Queue<TileGridObject> _targetTilesQueue = new Queue<TileGridObject>();
        
        private TileGridObject _startingTile;

        private TilePositionValidator _tilePositionValidator;

        private GameplayManager _gameplayManagerInstance;

        private void Awake() {
            TryGetComponent(out _tilePositionValidator);
            _gameplayManagerInstance = GameplayManager.Instance;
        }

        private void OnEnable() {
            InputManager.OnClickOnTile += SetTargetTilePosition;
        }
      
        private void OnDisable() {
            InputManager.OnClickOnTile -= SetTargetTilePosition;
        }

        private void Update() {
            MoveToNextTile();
        }
        
        //Teleport player to GridPosition - used on load level
        public void SetStartingTile(TileGridObject tileGridObject) {
            _currentGridPosition = tileGridObject.GetXY();
            Vector2 startingPosition = tileGridObject.GetWorldPositionCellCenter();
            transform.position = new Vector3(startingPosition.x, startingPosition.y, transform.position.z);
        }
        
        private void SetTargetTilePosition(TileGridObject tileGridObject) {
            _targetTilesQueue.Enqueue(tileGridObject);
        }

        private void MoveToNextTile() {
            if(_targetTilesQueue.Count == 0) return;
            
            if (!_tilePositionValidator.ValidateNextTilePosition(_currentGridPosition, _targetTilesQueue.Peek().GetXY())) {
                _targetTilesQueue.Dequeue();
                return;
            }
            
            _targetWorldPosition = _targetTilesQueue.Peek().GetWorldPositionCellCenter();
            
            Vector2 tempDampPosition = Vector2.SmoothDamp(transform.position, _targetWorldPosition, ref _velocity, movementSmoothTime, maxSpeed);
            
            // Apply the new position, keeping the original Z value
            transform.position = new Vector3(tempDampPosition.x, tempDampPosition.y, transform.position.z);
            
            // Check if the object has reached the target position
            if (!(Vector2.Distance(transform.position, _targetWorldPosition) <= targetReachThreshold))
                return;
            
            OnReachedTarget(); 
        }
        
        // Called when the final target position is reached
        private void OnReachedTarget() {
            // When player reached position, we need to update the current position
            _currentGridPosition = _targetTilesQueue.Peek().GetXY(); 
            
            _gameplayManagerInstance?.PhaseHandler(_targetTilesQueue.Peek());
            
            //Now we can remove that target from the List
            _targetTilesQueue.Dequeue();
        }
    }
}