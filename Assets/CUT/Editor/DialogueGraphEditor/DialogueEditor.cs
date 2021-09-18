using System;
using UnityEngine;
using UnityEditor;
using DartsGames.DialogueGraph;

namespace DartsGames.Editors.DialogueGraph
{
    [CustomEditor(typeof(Dialogue))]
    public class DialogueEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Edit"))
            {
                DialogueGraphWindow.loadingDialogue = target as Dialogue;
                DialogueGraphWindow.ShowWindow();
            }
            //base.OnInspectorGUI();
        }

        [UnityEditor.Callbacks.OnOpenAsset(2)]
        private static bool OnTryOpen(int instanceID, int line)
        {
            if (Selection.activeObject is Dialogue d)
            {
                DialogueGraphWindow.loadingDialogue = d;
                DialogueGraphWindow.ShowWindow();

                return true;
            }

            return false;
        }
    }
}