using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

namespace DartsGames.Editors
{
    [CustomPropertyDrawer(typeof(InspectConditionAttribute))]
    public class InspectConditionDrawer : PropertyDrawer
    {
        private MethodInfo method = null;
        private FieldInfo field = null;
        private bool initialized = false;

        private bool skipSerialization = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = (InspectConditionAttribute)attribute;
            var obj = property.serializedObject.targetObject;

            // initialize here
            if (!initialized)
            {
                initialized = true;
                var t = obj.GetType();
                
                // check eligibility (also check if object contains ExtendEditor Attribute
                if (string.IsNullOrEmpty(attr.MemberName) || 
                    !Attribute.IsDefined(t, typeof(ExtendEditorAttribute), false)) goto ENDIF;

                var _met = t.GetMethodInAncestors(attr.MemberName);

                if (_met != null && _met.GetParameters().Length == 0)
                {
                    method = _met;
                }
                else // no eligible method, look for field
                {
                    field = t.GetFieldInAncestors(attr.MemberName);
                }
            }

        ENDIF:

            // has method but the value isnt correct || has field but not correct value
            skipSerialization = (method != null && !method.Invoke(obj, null).Equals(attr.Value)) ||
                (field != null && !field.GetValue(obj).Equals(attr.Value));

            if (!skipSerialization)
                EditorGUI.PropertyField(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => skipSerialization ? 0 : base.GetPropertyHeight(property, label);
    }
}