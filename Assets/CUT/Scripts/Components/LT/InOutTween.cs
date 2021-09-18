using UnityEngine;

namespace DartsGames
{
    public class InOutTween : LT_Base
    {
        [SerializeField]
        private Vector3 inScale, outScale;
        [SerializeField, Tooltip("true if the animation cycle will zoom in first, else false")]
        private bool scaleInFirst = true;

        private void Start()
        {
            transform.localScale = scaleInFirst ? outScale : inScale;
            Zoom(scaleInFirst);
        }

        public void Zoom(bool scaleIn)
        {
            Vector3 scale = scaleIn ? inScale : outScale;
            LeanTween.scale(gameObject, scale, base.SpeedTimeRelation(transform.localScale, scale)).setIgnoreTimeScale(useUnscaledTime).setOnComplete(() => Zoom(!scaleIn));
        }
    }
}