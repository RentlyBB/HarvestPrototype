using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts {
    public static class Utils {
        /// <summary>
        /// Creates a TextMesh in world space at the specified position with the given text, font size, and color.
        /// </summary>
        /// <param name="text">The text to display in the TextMesh.</param>
        /// <param name="position">The world space position for the TextMesh.</param>
        /// <param name="fontSize">The size of the text.</param>
        /// <param name="color">The color of the text.</param>
        /// <returns>Returns the created GameObject with the TextMesh component.</returns>
        /// 
        public static TextMesh CreateTextWorld(string text, Vector3 position, int fontSize = 32, Transform parent = null, Color? color = null) {

            if (color == null) color = Color.white;
            
            // Create a new GameObject to hold the TextMesh
            GameObject textObject = new GameObject("WorldText");
            Transform transform = textObject.transform;
            // Add a TextMesh component to the GameObject
            TextMesh textMesh = textObject.AddComponent<TextMesh>();
            
            //Set paretn
            transform.SetParent(parent);

            // Set the text for the TextMesh
            textMesh.text = text;

            // Set the position of the TextMesh in world space
            textObject.transform.position = position;

            // Set font size and other properties
            textMesh.fontSize = fontSize;
            textMesh.color = color ?? Color.white; // Use the provided color or default to white

            // Ensure it's rendering in world space
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.characterSize = 0.1f; // Adjust this to control scaling in the world
            textMesh.offsetZ = -0.1f;

            // Optionally set any other properties of the TextMesh here (font, scale, etc.)
            
            return textMesh; // Return the created GameObject
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