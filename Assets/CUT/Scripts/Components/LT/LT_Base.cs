using UnityEngine;

namespace DartsGames
{
    [ExtendEditor]
    public class LT_Base : MonoBehaviour
    {
        [SerializeField]
        protected bool useUnscaledTime = true;

        public bool useSpeed = false;

        [SerializeField, InspectCondition("useSpeed", false)]
        protected float animTime;
        [SerializeField, InspectCondition("useSpeed", true)]
        protected float animSpeed;

        public void StopAnimation() => LeanTween.cancel(gameObject);

        protected float SpeedTimeRelation(Vector3 begin, Vector3 end) => SpeedTimeRelation(Vector3.Distance(begin, end));
        protected float SpeedTimeRelation(float begin, float end) => SpeedTimeRelation(Mathf.Abs(end - begin));

        private float SpeedTimeRelation(float distance) => useSpeed ? distance / animSpeed : animTime;
    }
}