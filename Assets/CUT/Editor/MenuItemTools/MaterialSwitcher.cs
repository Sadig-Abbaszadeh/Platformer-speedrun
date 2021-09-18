using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DartsGames.Editors
{
    public class MaterialSwitcher : EditorWindow
    {
        private Material mat1, mat2;

        [MenuItem("DartsGames/ChangeMaterial")]
        public static void ShowWindow()
        {
            var ms = (MaterialSwitcher)EditorWindow.GetWindow(typeof(MaterialSwitcher));
            ms.Show();
        }

        private void OnGUI()
        {
            mat1 = (Material)EditorGUILayout.ObjectField("Old material", mat1, typeof(Material), true);
            mat2 = (Material)EditorGUILayout.ObjectField("New material", mat2, typeof(Material), true);

            if (GUILayout.Button("Switch all scene materials"))
            {
                if (mat1 == null || mat2 == null)
                {
                    Debug.Log("Material options can not be null");
                    return;
                }

                SwitchMaterials();
            }
        }

        private void SwitchMaterials()
        {
            var R = FindObjectsOfType<Renderer>();
            int total = 0;

            foreach (var r in R)
            {
                var m = r.sharedMaterials;

                int i = m.ToList().FindIndex(_m => _m == mat1);

                if (i >= 0)
                {
                    total++;

                    m[i] = mat2;
                    r.sharedMaterials = m;
                }
            }

            Debug.Log((total > 0) ? $"Switched {total} materials" : "No materials switched");
        }
    }
}