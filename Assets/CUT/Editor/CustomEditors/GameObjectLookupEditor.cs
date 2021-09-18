using UnityEditor;

namespace DartsGames.Editors
{
    [CustomEditor(typeof(GameObjectLookup))]
    public class GameObjectLookupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.HelpBox("This game object can be looked up with its name", MessageType.Info);
        }
    }
}