using UnityEditor;
using UnityEngine;

namespace _Scripts.CustomTools {
    [CustomPropertyDrawer(typeof(RequireVariable))]
    public class RequireVariableDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.objectReferenceValue == null) {
                // Define two separate areas: one for the field and one for the warning label
                Rect fieldRect = new Rect(position.x, position.y, position.width * 0.7f, position.height);
                Rect warningRect = new Rect(position.x + position.width * 0.72f, position.y, position.width * 0.28f, position.height);

                // Draw red background to indicate the error
                EditorGUI.DrawRect(position, new Color(1, 0.5f, 0.5f, 0.3f));

                // Draw the field in the fieldRect
                EditorGUI.PropertyField(fieldRect, property, label);

                // Draw the warning label in the warningRect
                EditorGUI.LabelField(warningRect, "Required!", EditorStyles.boldLabel);
            } else {
                // Draw the field normally if no error
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}