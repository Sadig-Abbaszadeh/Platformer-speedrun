using System.Reflection;
using UnityEditor;

namespace DartsGames.Editors
{
    public static class EditorExtensions
    {
        public static object TrueValue(this SerializedProperty serP)
        {
            var t = serP.serializedObject.targetObject;

            return t.GetType()
                .GetField(serP.propertyPath, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(t);
        }
    }
}