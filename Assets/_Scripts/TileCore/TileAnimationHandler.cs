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

        private IEnumerator SpawnTileAnimation() {
            var i = Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(i);
            // In
            transform.DOScale(_originalScale, 0.4f).SetEase(Ease.OutBack);
        }
        
        private IEnumerator DespawnTileAnimation() {
            var i = Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(i);
            // Out
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        }
        
        [Button]
        public void DespawnTile() {
             transform.localScale = _originalScale;
             StartCoroutine(DespawnTileAnimation());
        }

        [Button]
        public void SpawnTile() {
            transform.localScale = Vector3.zero;
            StartCoroutine(SpawnTileAnimation());
        }

        [Button]
        public void CountdownAnimation() {
            transform.DOShakeScale(0.3f, 0.05f, 10, 0, false);
        }

    }
}