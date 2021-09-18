using UnityEngine;

namespace DartsGames
{
    public class PlaneRandom : CameraClipPlane, IRandomPositioner
    {
        public virtual Vector3 GetPositionFromFractions(float xFrac, float yFrac) => vert[0] + right * xFrac + up * yFrac;

        public virtual Vector3 GetRandomPosition() => GetPositionFromFractions(Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}