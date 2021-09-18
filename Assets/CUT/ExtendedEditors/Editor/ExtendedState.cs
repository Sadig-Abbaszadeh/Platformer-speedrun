using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DartsGames.Editors
{
    public class ExtendedState : MonoscriptDrawState
    {
        #region Static
        private static readonly int boxRightOffset = 25;
        #endregion

        // serialized object
        private SerializedObject so;

        private List<Button> buttonMethods = new List<Button>();

        // reg props
        private List<Property> regularProperties = new List<Property>();

        // boxes
        private Dictionary<string, List<Drawable>> boxedProperties = new Dictionary<string, List<Drawable>>();

        private float width;

        // drawer method
        public override void OnGUI()
        {
            width = Screen.width - boxRightOffset;

            base.OnGUI();

            DrawRegular(width);

            DrawBoxes(width);

            DrawButtons(width);

            so.ApplyModifiedProperties();
        }

        public override void Disable()
        {
            Undo.undoRedoPerformed -= SoUpdate;
        }

        // ctor
        /// <summary>
        /// Dont forget to call ondisable on the object on disable
        /// </summary>
        public ExtendedState(UnityEngine.Object target, SerializedObject obj, Func<FieldInfo, bool> CanSerializeField, Func<MethodInfo, bool> CanSerializeMethod) : base(target)
        {
            PrepareMain(target, obj, CanSerializeField, CanSerializeMethod);
            Undo.undoRedoPerformed += SoUpdate;
        }

        public void Refresh(UnityEngine.Object target, SerializedObject obj, Func<FieldInfo, bool> CanSerializeField, Func<MethodInfo, bool> CanSerializeMethod)
        {
            buttonMethods.Clear();
            regularProperties.Clear();
            boxedProperties.Clear();

            PrepareMain(target, obj, CanSerializeField, CanSerializeMethod);
        }

        #region Draw Stuff
        private void DrawRegular(float widith)
        {
            foreach (var sp in regularProperties)
                sp.Draw(width);
        }

        private void DrawBoxes(float width)
        {
            foreach (var bp in boxedProperties)
            {
                EditorGUILayout.BeginHorizontal();

                var w = width / bp.Value.Count;
                var initialLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = w * .4f;

                foreach (var d in bp.Value)
                {
                    d.Draw(w);
                }

                EditorGUIUtility.labelWidth = initialLabelWidth;

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawButtons(float width)
        {
            foreach (var m in buttonMethods)
                m.Draw(width);
        }
        #endregion

        #region Preparation
        private void PrepareMain(UnityEngine.Object target, SerializedObject obj, Func<FieldInfo, bool> CanSerializeField, Func<MethodInfo, bool> CanSerializeMethod)
        {
            so = obj;
            var targetType = target.GetType();

            var allMethods = targetType.GetAllMethodsInAncestors(new List<Type>() { typeof(MonoBehaviour), typeof(ScriptableObject) });
            var allFields = targetType.GetAllFieldsInAncestors(new List<Type>() { typeof(MonoBehaviour), typeof(ScriptableObject) });

            // this will be subdivided into different collections, such that at the end this collection will have 0 elements because all
            // will be inside their respective collections. 
            var serializableFields = allFields.Where(f => so.FindProperty(f.Name) != null &&
                !Attribute.IsDefined(f, typeof(HideInInspector)) &&
                CanSerializeField(f)).ToList(); // as param

            var usableMethods = allMethods.Where(m => m.GetParameters().Length == 0 &&
                Attribute.IsDefined(m, typeof(InspectorButtonAttribute)) && CanSerializeMethod(m)).ToList();

            // put stuff that should be in the boxes, into the boxes
            PrepareBoxedFields(ref serializableFields, ref usableMethods, target);

            // add the leftover serialized fields to the regular props
            foreach (var sf in serializableFields)
                regularProperties.Add(new Property(so.FindProperty(sf.Name)));

            // make buttons from the leftover methods
            buttonMethods = usableMethods.Select(method => new Button(method, target)).ToList();
        }

        private void SoUpdate()
        {
            so.Update();
        }

        private void PrepareBoxedFields(ref List<FieldInfo> serializableFields, ref List<MethodInfo> buttons, UnityEngine.Object target)
        {
            AddDrawableMember(ref serializableFields, f => new Property(so.FindProperty(f.Name)));

            AddDrawableMember(ref buttons, b => new Button(b, target));
        }

        private void AddDrawableMember<T>(ref List<T> elements, Func<T, Drawable> addingFunction) where T : MemberInfo
        {
            for (int i = 0; i < elements.Count; i++)
            {
                var e = elements[i];

                var box = (HorizontalAreaAttribute)e.GetCustomAttribute(typeof(HorizontalAreaAttribute));

                if (box == null)
                    continue;

                if (!boxedProperties.ContainsKey(box.Name))
                    boxedProperties.Add(box.Name, new List<Drawable>());

                var item = addingFunction(e);
                item.order = box.order;

                AddByOrder(boxedProperties[box.Name], item);

                elements.RemoveAt(i);
                i--;
            }
        }

        private void AddByOrder(List<Drawable> list, Drawable item)
        {
            var index = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (item.order < list[i].order)
                {
                    list.Insert(index, item);
                    return;
                }
            }

            list.Add(item);
        }
        #endregion

        #region Nests
        public abstract class Drawable
        {
            public int order = 0;
            public abstract void Draw(float width);
        }

        public class Button : Drawable
        {
            private Action action;
            private GUIContent name;

            public Button(MethodInfo method, UnityEngine.Object target)
            {
                this.action = () => method.Invoke(target, null);
                name = new GUIContent(method.Name);
            }

            public override void Draw(float width)
            {
                if (GUILayout.Button(name))
                    action();
            }
        }

        public class Property : Drawable
        {
            public SerializedProperty sp;

            public Property(SerializedProperty sp)
            {
                this.sp = sp;
            }

            public override void Draw(float width)
            {
                EditorGUILayout.PropertyField(sp, GUILayout.Width(width));
            }
        }
        #endregion
    }
}