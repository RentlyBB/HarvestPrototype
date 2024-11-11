using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.GridCore;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.PlayerCore {
    [RequireComponent(typeof(TilePositionValidator))]
    public class MovementHandler : MonoBehaviour {
        
        [Header("Movement Settings")]
        public float maxSpeed = 75; // Maximum speed for movement
        public float movementSmoothTime = 0.05f; // Smoothing time for movement
        public float targetReachThreshold = 0.0005f; // Distance to consider the target reached

        public float movementDelay;

        public bool startMoving = false;

        private TileGridObject _currentTile;
        private Vector2 _targetWorldPosition;
        private Vector2 _velocity = Vector2.zero; // Used by SmoothDamp for smooth movement

        private TileGridObject _startingTile;
        private TilePositionValidator _tilePositionValidator;
        private GameplayManager _gameplayManagerInstance;
        
        private readonly Queue<TileGridObject> _targetTilesQueue = new Queue<TileGridObject>();

        private void Awake() {
            TryGetComponent(out _tilePositionValidator);
            _gameplayManagerInstance = GameplayManager.Instance;
        }

        private void OnEnable() {
            InputManager.OnClickOnTile += AddTargetTile;
        }
      
        private void OnDisable() {
            InputManager.OnClickOnTile -= AddTargetTile;
        }

        private void Update() {
            Moving();
        }
        
        //Teleport player to GridPosition - used on load level
        public void SetStartingTile(TileGridObject tileGridObject) {
            _currentTile = tileGridObject;
            Vector2 startingPosition = tileGridObject.GetWorldPositionCellCenter();
            transform.position = new Vector3(startingPosition.x, startingPosition.y, transform.position.z);
        }
        
        private void AddTargetTile(TileGridObject tileGridObject) {
            _targetTilesQueue.Enqueue(tileGridObject);

            if (_targetTilesQueue.Count == 1) {
                ValidatingNextTile();
            }
        }

        private void ValidatingNextTile() {
            if (!_tilePositionValidator.ValidateNextTilePosition(_currentTile.GetXY(), _targetTilesQueue.Peek().GetXY())) {
                _targetTilesQueue.Dequeue();
                return;
            }

            // Next move has to be delayed because we want to see sprite change
            // Without the delay it is not visible that the tile was pressed and player
            // character just floating around the grid
            StartCoroutine(DelayedNextMove());
        }

        private IEnumerator DelayedNextMove() {
            yield return new WaitForSeconds(movementDelay);
            _targetWorldPosition = _targetTilesQueue.Peek().GetWorldPositionCellCenter();

            _currentTile?.GetTile().OnPlayerLeave();

            startMoving = true;
        }

        private void Moving() {
            if(!startMoving) return;
            
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
            _currentTile = _targetTilesQueue.Peek(); 
            
            _gameplayManagerInstance?.PhaseHandler(_targetTilesQueue.Peek());
            
            //Now we can remove that target from the List
            _targetTilesQueue.Dequeue();
            
            startMoving = false;
            
            if(_targetTilesQueue.Count != 0) ValidatingNextTile();
        }
    }
}