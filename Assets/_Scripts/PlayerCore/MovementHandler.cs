using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.GridCore;
using _Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using VInspector;

namespace _Scripts.PlayerCore {
    [RequireComponent(typeof(TilePositionValidator))]
    public class MovementHandler : MonoBehaviour {
        
        [Header("Movement Settings")]
        public float timeToReachTarget = 0.4f; // Maximum speed for movement

        private TileGridObject _currentTile;
        private Vector2 _targetWorldPosition;

        private TileGridObject _startingTile;
        private TilePositionValidator _tilePositionValidator;
        private GameplayManager _gameplayManagerInstance;
        
        private readonly Queue<TileGridObject> _targetTilesQueue = new Queue<TileGridObject>();

        private void Awake() {
            TryGetComponent(out _tilePositionValidator);
            _gameplayManagerInstance = GameplayManager.Instance;
        }

        private void OnEnable() {
            GameplayManager.MovePlayer += AddTargetTile;
        }
      
        private void OnDisable() {
            GameplayManager.MovePlayer -= AddTargetTile;
        }
        
        //Teleport player to GridPosition - used on load level
        public void SetStartingTile(TileGridObject tileGridObject) {
            _currentTile = tileGridObject; // Set current tile
            Vector2 startingPosition = tileGridObject.GetWorldPositionCellCenter();
            transform.position = new Vector3(startingPosition.x, startingPosition.y, transform.position.z);
            tileGridObject.GetTile().OnPlayerStep();
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

            MoveToNextPosition();
        }

        
        private async Task MoveToNextPosition() {
            _targetWorldPosition = _targetTilesQueue.Peek().GetWorldPositionCellCenter();

            _currentTile?.GetTile().OnPlayerLeave();
            
            var targetPosition = new Vector3(_targetWorldPosition.x, _targetWorldPosition.y, transform.position.z);
            transform.DOMove(targetPosition, timeToReachTarget).OnComplete(OnReachedTarget);
            
        }

     

        // Called when the final target position is reached
        private void OnReachedTarget() {
            // When player reached position, we need to update the current position
            _currentTile = _targetTilesQueue.Peek(); 
            
            //_gameplayManagerInstance?.PhaseHandler(_targetTilesQueue.Peek());
            
            //Now we can remove that target from the List
            _targetTilesQueue.Dequeue();
            
            if(_targetTilesQueue.Count != 0) ValidatingNextTile();
        }
    }
}