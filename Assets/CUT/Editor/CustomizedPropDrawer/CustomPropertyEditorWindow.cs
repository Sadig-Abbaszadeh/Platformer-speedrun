using UnityEditor;
using UnityEngine;

namespace DartsGames.Editors
{
    public class CustomPropertyEditorWindow : EditorWindow
    {
        protected CustomGeneralPropertyDrawer<ICustomPropertyDrawable> drawer;
        private Vector2 scrollPos;

        public virtual void Init(ICustomPropertyDrawable cpd)
        {
            titleContent.text = cpd.windowName;
            drawer = new CustomGeneralPropertyDrawer<ICustomPropertyDrawable>(cpd);
            Show();
        }

        private void OnEnable()
        {
            AssemblyReloadEvents.afterAssemblyReload += Close;
        }

        private void OnDisable()
        {
            AssemblyReloadEvents.afterAssemblyReload -= Close;
        }

        protected virtual void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            drawer.DrawProperties();
            EditorGUILayout.EndScrollView();
        }
    }
}