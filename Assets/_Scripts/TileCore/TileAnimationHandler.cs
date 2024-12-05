using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.TileCore {
    public class TileAnimationHandler : MonoBehaviour {
        
        private Vector3 _originalScale; 

        IEnumerator Anim() {
            var i = Random.Range(0.1f, 0.7f);
            
            yield return new WaitForSeconds(i);
            
            // Out
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);

            // In
            //transform.DOScale(_originalScale, 0.5f).SetEase(Ease.OutBack);
            
        }

        private void Awake() {
            _originalScale = transform.localScale;
        }

        private void Start() {
            StartCoroutine(Anim());
        }
    }
}