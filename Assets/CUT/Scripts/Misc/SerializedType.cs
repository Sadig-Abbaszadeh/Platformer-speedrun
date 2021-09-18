using UnityEngine;

namespace DartsGames
{
    /// <summary>
    /// A wrapper class to serialize interfaces (mainly). Inherit using ur interface as T. You can also make that class inherit
    /// from that interface, and make the methods call the actual methods from the interface for ease of use of the wrapper class
    /// </summary>
    [System.Serializable]
    public class SerializedType<T> : ISerializationCallbackReceiver
    {
        [SerializeField]
        public Component c;

        public T value;

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            if (c is T t)
                value = t;
        }
    }
}