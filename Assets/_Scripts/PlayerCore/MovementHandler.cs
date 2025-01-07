using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.GridCore;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using VInspector;

namespace _Scripts.PlayerCore {
    [RequireComponent(typeof(TilePositionValidator))]
    public class MovementHandler : MonoBehaviour {
        
        public static event UnityAction<TileGridObject> OnPlayerReachedTarget = delegate { };
        
        [Header("Movement Settings")]
        public float timeToReachTarget = 0.4f; // Maximum speed for movement

        private TileGridObject _currentTile;
        private Vector2 _targetWorldPosition;

        private TileGridObject _startingTile;
        private TilePositionValidator _tilePositionValidator;
        
        private readonly Queue<TileGridObject> _targetTilesQueue = new Queue<TileGridObject>();

        private void Awake() {
            TryGetComponent(out _tilePositionValidator);
        }

        private void OnEnable() {
            GameplayManager.OnPlayerMovement += AddTargetTile;
        }
      
        private void OnDisable() {
            GameplayManager.OnPlayerMovement += AddTargetTile;
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
        
        private void MoveToNextPosition() {
            _targetWorldPosition = _targetTilesQueue.Peek().GetWorldPositionCellCenter();

            _currentTile?.GetTile().OnPlayerLeave();
            
            var targetPosition = new Vector3(_targetWorldPosition.x, _targetWorldPosition.y, transform.position.z);
            transform.DOMove(targetPosition, timeToReachTarget).OnComplete(OnReachedTarget);
        }

        // Called when the final target position is reached
        private void OnReachedTarget() {
            // When player reached position, we need to update the current position
            _currentTile = _targetTilesQueue.Peek(); 
            
            // _currentTile.GetTile().TryGetComponent(out TileBase tileBase);
            // tileBase?.OnPlayerStep();
            
            OnPlayerReachedTarget?.Invoke(_currentTile);
            
            //Now we can remove that target from the List
            _targetTilesQueue.Dequeue();
            
            if(_targetTilesQueue.Count != 0) ValidatingNextTile();
        }
    }
}