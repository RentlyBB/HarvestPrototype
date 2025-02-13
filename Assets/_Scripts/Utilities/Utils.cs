﻿using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Utilities {
    public static class Utils {
        
        public static TextMeshPro CreateTextWorld(string text, Vector3 position, int fontSize = 32, Transform parent = null, Color? color = null) {
            if (color == null) color = Color.white;

            // Create a new GameObject to hold the TextMeshPro
            GameObject textObject = new GameObject("WorldText");
            Transform transform = textObject.transform;

            // Set parent
            transform.SetParent(parent);

            // Add a TextMeshPro component to the GameObject
            TextMeshPro textMeshPro = textObject.AddComponent<TextMeshPro>();

            TMP_FontAsset font = Resources.Load<TMP_FontAsset>("Fonts/CHUNKY BAR SDF");
            
            // Set the text, position, and other properties
            textMeshPro.text = text;
            textMeshPro.fontSize = fontSize;
            textMeshPro.font = font;
            textMeshPro.color = color.Value;
            textObject.transform.position = position;

            // Configure TextMeshPro to behave as world-space text
            textMeshPro.alignment = TextAlignmentOptions.Center;

            // Adjust the scale if necessary to make the text fit well in the world
            textMeshPro.rectTransform.localScale = Vector3.one * 0.1f; // Adjust to fit your scene's scale

            // Return the created TextMeshPro component
            return textMeshPro;
        }
        
        public static SpriteRenderer CreateSpriteWorld(Sprite sprite, Vector3 position, Vector2 size, Transform parent = null, Color? color = null)
        {
            // If color is not provided, default to white
            if (color == null) color = Color.white;

            // Create a new GameObject to hold the SpriteRenderer
            GameObject spriteObject = new GameObject("WorldSprite");
            Transform transform = spriteObject.transform;

            // Add a SpriteRenderer component to the GameObject
            SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();

            // Set the parent transform if provided
            transform.SetParent(parent);

            // Set the position of the sprite in world space
            transform.position = position;

            // Set the sprite for the SpriteRenderer
            spriteRenderer.sprite = sprite;

            // Set the size of the sprite (by adjusting the scale)
            //transform.localScale = new Vector3(size.x / spriteRenderer.sprite.bounds.size.x, size.y / spriteRenderer.sprite.bounds.size.y, 1);
            transform.localScale = new Vector3(size.x , size.y, 1);

            // Set the color of the sprite
            spriteRenderer.color = color ?? Color.white; // Use the provided color or default to white

            // Return the created SpriteRenderer
            return spriteRenderer;
        }
        
        public static Vector3 GetMousePosition3D() {
            if(Camera.main == null) return new Vector3(1000, 1000, 1000);
            
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue)) {
                return raycastHit.point;
            }
            
            return new Vector3(1000, 1000, 1000);
        }
        
        public static Vector2 GetMousePosition2D() {
            if (Camera.main != null) {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue)) {
                    return raycastHit.point;
                }
            }

            return new Vector2(1000, 1000);
        }
        
        public static Vector2 GetMouseWorldPosition2D() {
            Vector2 screenPosition = Mouse.current.position.ReadValue();

            Camera cam = Camera.main != null ? Camera.main : (Camera.allCameras.Length > 0 ? Camera.allCameras[0] : null);
            if (cam != null) {
                Vector3 worldPosition = cam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, cam.nearClipPlane));
                return new Vector2(worldPosition.x, worldPosition.y);
            }

            return new Vector2(1000, 1000); // Default if no camera is found
        }
    }
}