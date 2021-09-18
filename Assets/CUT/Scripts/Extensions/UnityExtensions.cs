using System;
using System.Collections;
using UnityEngine;

namespace DartsGames
{
    public static class UnityExtensions
    {
        public static void DoAtTheEndOfFrame(this MonoBehaviour mb, Action action) => mb.StartCoroutine(EndOfFrameAction(action));

        public static void DoAfterOneFrame(this MonoBehaviour mb, Action action) => mb.StartCoroutine(ActionAfterFrame(action));
        public static Coroutine DoAfterTime(this MonoBehaviour mb, float time, Action action) => mb.StartCoroutine(ActionAfterTime(time, action));
        public static Coroutine DoAfterUnscaledTime(this MonoBehaviour mb, float time, Action action) => mb.StartCoroutine(ActionAfterUnscaledTime(time, action));
        public static Coroutine DoWhile(this MonoBehaviour mb, Func<bool> waitCondition, Action action) => mb.StartCoroutine(DoWhileTrue(waitCondition, action));
        public static Coroutine DoWhile(this MonoBehaviour mb, Func<bool> waitCondition, Action action, Action afterwards) => mb.StartCoroutine(DoWhileTrue(waitCondition, action, afterwards));
        public static Coroutine DoAfterConditionIsTrue(this MonoBehaviour mb, Func<bool> condition, Action action) => mb.StartCoroutine(DoAfterTrue(condition, action));
        public static Coroutine DoEvery(this MonoBehaviour mb, float waitTime, Action action) => mb.StartCoroutine(DoEvery(waitTime, waitTime, action));
        public static Coroutine DoEvery(this MonoBehaviour mb, float waitTime, float initialWaitTime, Action action) => mb.StartCoroutine(DoEvery(waitTime, initialWaitTime, action));

        public static Coroutine DoAfterRoutine(this MonoBehaviour mb, Coroutine routine, Action action) => mb.StartCoroutine(DoAfterOtherRoutine(routine, action));

        public static T Debug<T>(this T t)
        {
            UnityEngine.Debug.Log(t);
            return t;
        }

        public static T Debug<T>(this T t, UnityEngine.Object context)
        {
            UnityEngine.Debug.Log(t, context);
            return t;
        }

        public static bool ContainsLayer(this LayerMask layerMask, int layer) => layerMask == (layerMask | (1 << layer));

        public static void SetParent(this Transform t, Component parent) => t.SetParent(parent.transform);

        public static Transform GetRandomChild(this Transform t) => t.GetChild(UnityEngine.Random.Range(0, t.childCount));

        public static Vector2 XZ(this Vector3 v) => new Vector2(v.x, v.z);

        #region Behind the scenes
        private static IEnumerator ActionAfterFrame(Action action)
        {
            yield return null;
            action();
        }

        private static IEnumerator ActionAfterTime(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action();
        }

        private static IEnumerator ActionAfterUnscaledTime(float time, Action action)
        {
            yield return new WaitForSecondsRealtime(time);
            action();
        }

        private static IEnumerator DoWhileTrue(Func<bool> condition, Action action)
        {
            while (condition())
            {
                action();
                yield return null;
            }
        }

        private static IEnumerator DoWhileTrue(Func<bool> condition, Action action, Action afterwards)
        {
            while (condition())
            {
                action();
                yield return null;
            }
            afterwards();
        }

        private static IEnumerator DoAfterTrue(Func<bool> condition, Action action)
        {
            yield return new WaitUntil(condition);
            action();
        }

        private static IEnumerator DoEvery(float time, float beginTime, Action action)
        {
            yield return new WaitForSeconds(beginTime);
            action();

            while(true)
            {
                yield return new WaitForSeconds(time);
                action();
            }
        }

        private static IEnumerator EndOfFrameAction(Action action)
        {
            yield return new WaitForEndOfFrame();
            action();
        }

        private static IEnumerator DoAfterOtherRoutine(Coroutine routine, Action action)
        {
            yield return routine;

            action();
        }
        #endregion
    }
}