using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace DartsGames.Editors
{
    public class CustomGeneralPropertyDrawer<T> where T : ICustomPropertyDrawable
    {
        protected SerializedObject serializedObject;

        protected List<SerializedProperty> properties = new List<SerializedProperty>();

        private Dictionary<string, int> propertyIndices = new Dictionary<string, int>();

        // ctor
        /// <summary>
        /// T needs to derive from UnityEngine.Object!
        /// </summary>
        public CustomGeneralPropertyDrawer(T serializedObject)
        {
            if (serializedObject is UnityEngine.Object obj)
                this.serializedObject = new SerializedObject(obj);
            else
                throw new ArgumentException("Provided object does not inherit UnityEngine.Object"); 

            var props = serializedObject.DrawableProperties;

            foreach (var p in props)
            {
                var prop = this.serializedObject.FindProperty(p);

                if (prop != null)
                    properties.Add(prop);
            }
        }

        private int GetIndex(string p)
        {
            int index = 0;

            if (!propertyIndices.TryGetValue(p, out index))
            {
                propertyIndices.Add(p, 0);
                index = 0;
            }

            return index;
        }
        private void SetIndex(string p, int value)
        {
            if (!propertyIndices.ContainsKey(p))
            {
                propertyIndices.Add(p, value);
            }
            else
            {
                propertyIndices[p] = value;
            }
        }

        public void DrawProperties()
        {
            foreach (var p in properties)
                Draw(p);

            serializedObject.ApplyModifiedProperties();
        }

        protected void Draw(SerializedProperty p)
        {
            var t = p.propertyType;

            if (p.isArray && t != SerializedPropertyType.String)
            {
                DrawArray(p);
            }
            else if (t != SerializedPropertyType.Generic)
            {
                EditorGUILayout.PropertyField(p);
            }
            else
            {
                p.NextVisible(true);
                EditorGUILayout.PropertyField(p);
            }
        }

        protected void DrawArray(SerializedProperty prop)
        {
            int size = prop.arraySize;

            EditorGUILayout.BeginHorizontal(GUILayout.Height((size) * 21 + 50), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

            GUILayout.Label(prop.displayName);

            var btnInd = GetIndex(prop.propertyPath);

            for (int i = 0; i < size; i++)
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(i == btnInd ? ">>>" : $"El. {i}"))
                {
                    SetIndex(prop.propertyPath, i);
                }

                if (GUILayout.Button("Remove"))
                {
                    prop.DeleteArrayElementAtIndex(i);
                    SetIndex(prop.propertyPath, 0);
                    return;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add"))
            {
                prop.InsertArrayElementAtIndex(size);
                return;
            }

            EditorGUILayout.EndVertical();

            if (size > 0)
            {
                var _p = prop.GetArrayElementAtIndex(GetIndex(prop.propertyPath));
                var name = _p.displayName;

                if (_p.propertyType == SerializedPropertyType.Generic)
                    _p.NextVisible(true);

                EditorGUILayout.BeginVertical();

                if (_p.isArray)
                {
                    Draw(_p);
                }
                else
                {
                    DrawNonArrayRecursive(_p);
                }

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawNonArrayRecursive(SerializedProperty p)
        {
            do
            {
                Draw(p);
            } while (p.NextVisible(false) && !ContainsOneOfTwo(p.displayName, "Element", "Size"));
        }

        private bool ContainsOneOfTwo(string name, string a, string b) => name.Contains(a) || name.Contains(b);
    }
}