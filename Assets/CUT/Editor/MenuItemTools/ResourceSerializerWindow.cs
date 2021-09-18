using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DartsGames.Editors
{
    /// <summary>
    /// Override this to make a resource serializer button with menu item attr. 
    /// Create a static initializer method with MenuItem attr and inside call the protected Initialize method.
    /// </summary>
    public class ResourceSerializerWindow : EditorWindow
    {
        #region Static
        /// <summary>
        /// Call to make resource serializer window with name windowName that will serialize resources with resource names which inherit (scriptable objects) or 
        /// have components (gameobject prefab) that inherit the IResourceSerializer interface
        /// </summary>
        protected static void Initialize(string windowName, string[] resourceNames)
        {
            var w = CreateWindow<ResourceSerializerWindow>(windowName);
            w.Init(resourceNames);
            w.Show();
        }
        #endregion

        private Vector2 scrollPos;
        private List<Drawer> drawers = new List<Drawer>();

        protected virtual void Init(string[] resoureNames)
        {
            foreach (var str in resoureNames)
            {
                // get resource
                var obj = Resources.Load(str);
                // null check
                if (obj is null) continue;

                PrepareResources(obj, str);
            }
        }

        private void OnEnable()
        {
            // prepare to be closed
            AssemblyReloadEvents.afterAssemblyReload += Close;
        }

        private void OnDisable()
        {
            // unsubscribe from event 
            AssemblyReloadEvents.afterAssemblyReload -= Close;
        }

        protected virtual void OnGUI()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);

            foreach (var drawer in drawers)
                drawer.Draw();

            GUILayout.EndScrollView();
        }

        /// <summary>
        /// Get serialized object / objects
        /// </summary>
        private void PrepareResources(UnityEngine.Object obj, string soName)
        {
            // if is scriptable object
            if (obj is IResourceSerializer serializer)
            {
                PrepareDrawers(serializer, soName);
            }
            // else if is game obejct
            else if (obj is GameObject gameObject)
            {
                var serializers = gameObject.GetComponents<IResourceSerializer>();

                foreach (var ser in serializers)
                    PrepareDrawers(ser, $"{soName}.{ser.GetType().ToString()}");
            }
        }

        /// <summary>
        /// Get serialized properties and fill drawers collection
        /// </summary>
        private void PrepareDrawers(IResourceSerializer serializer, string soName)
        {
            var props = serializer.SerializableProperties;
            var so = new SerializedObject(serializer as UnityEngine.Object);
            var drawer = new Drawer(so, soName);

            foreach (var p in props)
            {
                if (string.IsNullOrEmpty(p)) continue;

                var paths = p.Split('.');
                SerializedProperty sp;

                // if split has results
                if (paths.Length > 0)
                {
                    // get main property
                    sp = so.FindProperty(paths[0]);

                    // find relative property hierarchyially 
                    for (int i = 1; i < paths.Length; i++)
                    {
                        // if property is not found break
                        if (sp is null) break;

                        // get next
                        sp = sp.FindPropertyRelative(paths[i]);
                    }

                    // ultimately if a property is found add it to the drawer object
                    if (!(sp is null))
                    {
                        drawer.AddSerializedProperty(sp, p);
                    }
                }
            }

            // if new drawer has any properties add it to the list
            if (drawer.HasProperties)
                drawers.Add(drawer);
        }

        public class Drawer
        {
            private static readonly float indentWidth = 12,
                spaceBetweenDrawers = 7;

            private bool isOpen = false;
            private GUIContent label;

            private SerializedObject so;
            private List<Property> properties = new List<Property>();

            public bool HasProperties => properties.Count > 0;

            public Drawer(SerializedObject so, string name)
            {
                this.so = so;
                this.label = new GUIContent(name);
            }

            public void Draw()
            {
                isOpen = EditorGUILayout.Foldout(isOpen, label);

                if (isOpen)
                {

                    GUILayout.BeginHorizontal();
                    // add space
                    GUILayout.Space(indentWidth);

                    DrawProperties();

                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(spaceBetweenDrawers);

                so.ApplyModifiedProperties();
            }

            private void DrawProperties()
            {
                GUILayout.BeginVertical();

                foreach (var prop in properties)
                {
                    prop.Draw();
                }

                GUILayout.EndVertical();
            }

            public void AddSerializedProperty(SerializedProperty property) => AddSerializedProperty(property, property.displayName);

            public void AddSerializedProperty(SerializedProperty property, string displayName)
            {
                properties.Add(new Property(displayName, property));
            }
        }

        public class Property
        {
            private GUIContent label;
            private SerializedProperty prop;

            public Property(string label, SerializedProperty prop)
            {
                this.prop = prop;
                this.label = new GUIContent(label);
            }

            public void Draw()
            {
                EditorGUILayout.PropertyField(prop, label);
            }
        }
    }
}