using System;
using UnityEngine;
using UnityEngine.Events;

namespace DartsGames
{
    [DisallowMultipleComponent]
    public class ScriptableEventListener : MonoBehaviour
    {
        [SerializeField]
        private ScriptableEvent sEvent;

        public UnityEvent responseEvent;

        private void OnEnable() => sEvent += this;
        private void OnDisable() => sEvent -= this;
        public void RaiseResponse() => responseEvent.Invoke();
    }
}