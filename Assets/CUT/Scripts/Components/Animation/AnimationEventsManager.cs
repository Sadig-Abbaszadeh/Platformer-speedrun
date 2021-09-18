using System;
using UnityEngine.Events;
using UnityEngine;
using System.Collections.Generic;

namespace DartsGames
{
    [RequireComponent(typeof(Animator)), DisallowMultipleComponent]
    public class AnimationEventsManager : MonoBehaviour
    {
        [SerializeField]
        private List<UnityEvent> events = new List<UnityEvent>();

        public void InvokeEvent(int index) => events[index].Invoke();

        public void Sub(int index, UnityAction action) => events[index].AddListener(action);
        public void UnSub(int index, UnityAction action) => events[index].RemoveListener(action);
    }
}