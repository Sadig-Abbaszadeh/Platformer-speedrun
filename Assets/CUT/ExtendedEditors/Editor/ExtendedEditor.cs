using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DartsGames.Editors
{
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class ExtendedEditor : Editor, IEditorState
    {
        protected IEditorState state;

        protected virtual void OnEnable()
        {
            var t = target.GetType();

            // is extended editor
            if (t.GetCustomAttribute(typeof(ExtendEditorAttribute)) != null)
            {
                state = new ExtendedState(target, t, serializedObject, CanSerializeField, CanSerializeMethod);
            }
            // is custom drawable 
            else if(target is ICustomPropertyDrawable cpd)
            {
                state = new CGD_ButtonState(cpd);
            }
            // is normal editor
            else
            {
                state = this;
            }
        }

        private void OnDisable() => state.Disable();

        public sealed override void OnInspectorGUI() => state.OnGUI();

        #region Default inspector
        /// <summary>
        /// Override this instead of OnInspectorGUI if you want to override the default inspector gui
        /// </summary>
        public virtual void OnGUI()
        {
            base.OnInspectorGUI();
        }

        /// <summary>
        /// If state is regular state, this is called on disable, else the state.disable will be called on the state. Thus only override this. OnEnable is not changed for now
        /// </summary>
        public virtual void Disable() { }
        #endregion

        protected virtual bool CanSerializeField(FieldInfo fieldInfo) => true;
        protected virtual bool CanSerializeMethod(MethodInfo methodInfo) => true;
    }
}