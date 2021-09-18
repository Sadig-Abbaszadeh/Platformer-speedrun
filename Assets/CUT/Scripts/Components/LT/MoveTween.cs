using System;
using UnityEngine;

namespace DartsGames
{
    public class MoveTween : LT_Base
    {
        [SerializeField]
        private Transform origin, destination;

        public void MoveToDestination() => MoveTo(destination);
        public void MoveToOrigin() => MoveTo(origin);
        public void MoveToDestination(Action onComplete) => MoveTo(destination, onComplete);
        public void MoveToOrigin(Action onComplete) => MoveTo(origin, onComplete);

        // main move function
        private LTDescr MoveTo(Transform target)
        {
            StopAnimation();
            return LeanTween.move(gameObject, target, base.SpeedTimeRelation(transform.localPosition, target.localPosition)).setIgnoreTimeScale(base.useUnscaledTime);
        }

        private LTDescr MoveTo(Transform target, Action afterReached) => MoveTo(target).setOnComplete(afterReached);
    }
}