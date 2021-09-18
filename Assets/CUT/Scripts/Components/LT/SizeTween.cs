using System;
using UnityEngine;

namespace DartsGames
{
    [ExtendEditor]
    public class SizeTween : LT_Base
    {
        [SerializeField]
        private Vector3 origin, destination;

        public void SizeInAndBack(Action onComplete) => ResizeTo(destination, () => ResizeTo(origin, onComplete));
        public void SizeInAndBack() => ResizeTo(destination, () => ResizeTo(origin));

        public void SizeIn() => ResizeTo(destination);
        public void SizeOut() => ResizeTo(origin);

        private LTDescr ResizeTo(Vector3 target)
        {
            base.StopAnimation();
            return LeanTween.scale(gameObject, target, SpeedTimeRelation(transform.localScale, target)).setIgnoreTimeScale(useUnscaledTime);
        }

        public float AnimTime() => SpeedTimeRelation(origin, destination);

        private LTDescr ResizeTo(Vector3 target, Action onComplete) => ResizeTo(target).setOnComplete(onComplete);
    }
}