using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DartsGames.Editors
{
    public class CGD_ButtonState : MonoscriptDrawState
    {
        private ICustomPropertyDrawable cpd;

        public CGD_ButtonState(ICustomPropertyDrawable cpd) : base(cpd)
        {
            this.cpd = cpd;
        }

        public override void OnGUI()
        {
            base.OnGUI();

            if(GUILayout.Button("Open Editor"))
            {
                EditorWindow.CreateWindow<CustomPropertyEditorWindow>().Init(cpd);
            }
        }
    }
}