using System;
using System.Collections.Generic;
using UnityEngine;

namespace DartsGames
{
    [CreateAssetMenu(menuName = "DartsGames/ScriptableEvent")]
    public class ScriptableEvent : ScriptableObject
    {
        private List<ScriptableEventListener> listeners = new List<ScriptableEventListener>();

        public void RaiseEvent()
        {
            for (int i = 0; i < listeners.Count; i++)
                listeners[i].RaiseResponse();
        }

        public static ScriptableEvent operator +(ScriptableEvent sEvent, ScriptableEventListener listener)
        {
            sEvent.listeners.Add(listener);
            return sEvent;
        }

        public static ScriptableEvent operator -(ScriptableEvent sEvent, ScriptableEventListener listener)
        {
            sEvent.listeners.Remove(listener);
            return sEvent;
        }
    }
}