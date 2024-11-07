using System.Collections.Generic;
using _Scripts.TileCore.Composites;
using _Scripts.TileCore.Enums;
using _Scripts.TileCore.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(TileVisualData))]
    public class TileVisualDataEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            TileVisualData tileVisualData = (TileVisualData)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("State Sprites", EditorStyles.boldLabel);

            foreach (var pair in tileVisualData.stateSpritesList) {
                EditorGUILayout.BeginHorizontal();
                pair.compositeState.mainState = (TileMainVisualStates)EditorGUILayout.EnumPopup(pair.compositeState.mainState, GUILayout.Width(100));
                pair.compositeState.subState = (TileSubVisualStates)EditorGUILayout.EnumPopup(pair.compositeState.subState, GUILayout.Width(100));
                pair.sprite = (Sprite)EditorGUILayout.ObjectField(pair.sprite, typeof(Sprite), false);
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add New State")) {
                tileVisualData.stateSpritesList.Add(new TileVisualData.StateSpritePair());
            }

            if (GUI.changed) {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
