using UnityEngine;

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
        
        
        
    }
}