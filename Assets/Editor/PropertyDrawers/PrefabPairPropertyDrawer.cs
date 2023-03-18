using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace bpdev
{
    // [CustomPropertyDrawer(typeof(PrefabPair))]
    public class PrefabPairPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int _padding = 2;

            float _prefab = 0.6f;
            float _type = 0.4f;

            EditorGUI.BeginProperty(position, label, property);

            int _indentCache = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect _prefabRect = new Rect(position.x, position.y, position.width * _prefab - _padding, position.height);
            Rect _typeRect = new Rect(position.x + position.width * _prefab + _padding, position.y, position.width * _type - _padding, position.height);
            
            SerializedProperty _prefabProperty = property.FindPropertyRelative("Prefab");
            SerializedProperty _typeProperty = property.FindPropertyRelative("Type");

            EditorGUI.PropertyField(_prefabRect, _prefabProperty, GUIContent.none);
            EditorGUI.PropertyField(_typeRect, _typeProperty, GUIContent.none);

            EditorGUI.indentLevel = _indentCache;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}