using System;
using UnityEditor;
using UnityEngine;

namespace DartsGames.Editors
{
    [CustomPropertyDrawer(typeof(SerializedType<>), true)]
    public class SerializedTypeEditor : PropertyDrawer
    {
        private bool isActive = false;
        private Type t;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect newPos = EditorGUI.PrefixLabel(EditorGUI.IndentedRect(position), label);

            var c = property.FindPropertyRelative("c");
            var val = c.objectReferenceValue;

            newPos.height = 18;
            EditorGUI.PropertyField(newPos, c, GUIContent.none);

            // change occured
            if (val != c.objectReferenceValue && c.objectReferenceValue != null)
            {
                if (!isActive)
                {
                    t = property.TrueValue().GetType().BaseType.GetGenericArguments()[0];

                    isActive = true;
                }

                if (!t.IsAssignableFrom(c.objectReferenceValue.GetType()))
                {
                    var comp = ((Component)c.objectReferenceValue).GetComponent(t);
                    c.objectReferenceValue = comp;
                }
            }
        }
    }
}