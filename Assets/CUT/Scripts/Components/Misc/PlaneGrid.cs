using System.Collections.Generic;
using UnityEngine;

namespace DartsGames
{
    /// <summary>
    /// Match the camera plane and create a 2D grid on the resulting plane
    /// </summary>
    public class PlaneGrid : CameraClipPlane, IRandomPositioner
    {
        [SerializeField]
        protected float cellWidth, cellHeight;
        [SerializeField]
        protected AnchorOptions anchorOptions;

        protected List<Vector3> points;

        protected override void Start()
        {
            base.Start();

            SetUpGrid();
        }

        protected void SetUpGrid()
        {
            int xCount = (int)(transform.lossyScale.x / cellWidth);
            int yCount = (int)(transform.lossyScale.y / cellHeight);
            points = new List<Vector3>(xCount * yCount);

            float xOffset = 0, yOffset = 0;

            switch (anchorOptions)
            {
                case AnchorOptions.LeftBottomStack:
                    xOffset = .5f;
                    yOffset = .5f;
                    break;
                case AnchorOptions.LeftBottomSnap:
                    break;
                default:
                    break;
            }

            var _up = up.normalized;
            var _right = right.normalized;

            for (int y = 0; y < yCount; y++)
            {
                for (int x = 0; x < xCount; x++)
                {
                    points.Add(vert[0] + _right * (x + xOffset) * cellWidth +
                        _up * (y + yOffset) * cellHeight);
                }
            }
        }

        public Vector3 GetRandomPosition() => points.RandomElement();

        public enum AnchorOptions
        {
            LeftBottomStack,
            LeftBottomSnap,
        }
    }
}