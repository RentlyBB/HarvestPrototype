using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.UI {
    public class LevelContainer : MonoBehaviour {
        private void OnEnable() {
            
            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                var originalScale = child.localScale;
                
                child.localScale = Vector3.zero;
                var delay = Random.Range(0.1f, 0.5f); // Each tile gets a unique delay
                child.DOScale(originalScale, 0.4f)
                    .SetEase(Ease.OutBack)
                    .SetDelay(delay); // Apply delay to this specific tile's Tween
            }
        }
    }
}