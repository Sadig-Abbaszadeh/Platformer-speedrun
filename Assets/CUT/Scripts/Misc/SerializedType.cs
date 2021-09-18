using UnityEngine;

namespace DartsGames
{
    /// <summary>
    /// A wrapper class to serialize interfaces (mainly). Inherit using ur interface as T.
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
            else
                Debug.Log("Error: serialized component is not of type " + typeof(T));
        }
    }
}