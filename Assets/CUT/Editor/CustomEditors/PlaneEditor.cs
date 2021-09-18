using UnityEditor;
using UnityEngine;

namespace DartsGames.Editors
{
    [CustomEditor(typeof(CameraClipPlane), true)]
    public class PlaneEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Match the clip plane"))
            {
                Undo.RecordObject(target, "plane match");
                ((CameraClipPlane)target).MatchClipPlane();
            }
        }
    }
}