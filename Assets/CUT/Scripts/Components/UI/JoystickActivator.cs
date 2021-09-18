using UnityEngine;
using UnityEngine.EventSystems;

namespace DartsGames
{
    [DisallowMultipleComponent]
    public class JoystickActivator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField]
        private Joystick joystick;

        public void OnDrag(PointerEventData eventData)
        {
            joystick.SetPosition(eventData.position);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            joystick.Activate(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            joystick.Deactivate();
        }
    }
}