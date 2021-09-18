using UnityEngine;

namespace DartsGames
{
    [DisallowMultipleComponent]
    public class MoveObjectsOnScreen : MonoBehaviour
    {
        [SerializeField, Tooltip("The transform that will be moved")]
        protected Transform mainTransform;

        protected Camera cam;

        private float widthFactor, heightFactor, heightBound, widthBound, z;

        protected Transform parent;

        protected virtual void Start()
        {
            cam = Camera.main;

            if (mainTransform == null)
                mainTransform = transform;
        }

        protected virtual void OnMouseDown()
        {
            parent = mainTransform.parent;
            mainTransform.SetParent(cam);

            heightBound = 2 * mainTransform.localPosition.z * Mathf.Tan(cam.fieldOfView / 2 * Mathf.Deg2Rad);
            widthBound = heightBound * cam.pixelWidth / cam.pixelHeight;

            widthFactor = widthBound / cam.pixelWidth;
            heightFactor = heightBound / cam.pixelHeight;

            z = mainTransform.localPosition.z;
        }

        protected virtual void OnMouseUp()
        {
            mainTransform.SetParent(parent);
        }

        protected virtual void OnMouseDrag()
        {
            UpdatePosition(Input.mousePosition);

            UpdateRotation();
        }

        protected virtual void Reset()
        {
            mainTransform = transform;
        }

        protected void UpdateRotation()
        {
            mainTransform.rotation = Quaternion.LookRotation(mainTransform.position - cam.transform.position);
        }

        protected void UpdatePosition(Vector3 position) => UpdatePosition(position.x, position.y);

        protected void UpdatePosition(float x, float y)
        {
            mainTransform.localPosition = new Vector3(x * widthFactor - widthBound / 2,
                y * heightFactor - heightBound / 2, z);
        }
    }
}