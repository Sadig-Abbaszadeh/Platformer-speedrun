using System.Collections.Generic;
using UnityEngine;

namespace DartsGames
{
    [DisallowMultipleComponent]
    public class GameObjectLookup : MonoBehaviour
    {
        private static Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();

        public static bool LookUpGameObject(string name, out GameObject obj) => objects.TryGetValue(name, out obj);

        private void Awake()
        {
            objects.Add(gameObject.name, gameObject);
        }

        private void OnDestroy()
        {
            objects.Remove(gameObject.name);
        }
    }
}