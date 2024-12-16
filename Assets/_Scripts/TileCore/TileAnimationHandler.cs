using System;
using System.Collections;
using DG.Tweening;
using QFSW.QC;
using UnityEngine;
using VInspector;
using Random = UnityEngine.Random;

namespace _Scripts.TileCore {
    public class TileAnimationHandler : MonoBehaviour {
        
        private Vector3 _originalScale;

        private void Awake() {
            _originalScale = transform.localScale;
        }

        [Button]
        public Tween SpawnTileAnimation() {
            transform.localScale = Vector3.zero;
            var delay = Random.Range(0.1f, 0.5f); // Each tile gets a unique delay
            return transform.DOScale(_originalScale, 0.4f)
                .SetEase(Ease.OutBack)
                .SetDelay(delay); // Apply delay to this specific tile's Tween
        }
        
        [Button]
        public Tween DespawnTileAnimation() {
            transform.localScale = _originalScale;
            var delay = Random.Range(0.1f, 0.5f);
            return transform.DOScale(Vector3.zero, 0.5f)
                .SetEase(Ease.InBack)
                .SetDelay(delay);
        }
        
        public Tween CountdownAnimation() {
            return transform.DOShakeScale(0.3f, 0.05f, 10, 0, false);
        }

        public Tween FreezeAnimation() {
            return transform.DOShakePosition(0.3f, 0.05f, 10, 0);
        }
    }
}