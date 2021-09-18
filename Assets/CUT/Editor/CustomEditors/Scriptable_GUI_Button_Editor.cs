using UnityEditor;
using UnityEngine;

namespace DartsGames.Editors
{
    [CustomEditor(typeof(Scriptable_GUI_Button), true)]
    public class Scriptable_GUI_Button_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Scriptable_GUI_Button t = target as Scriptable_GUI_Button;

            if (GUILayout.Button(t.ButtonName()))
                t.OnGUIButtonPressed();
        }
    }
}