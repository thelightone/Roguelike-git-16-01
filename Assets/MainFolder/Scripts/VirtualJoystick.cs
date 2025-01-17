
using UnityEngine;
using UnityEngine.EventSystems;

    public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        public RectTransform joystickBackground;
        public RectTransform joystickHandle;

        public Transform characterTransform; // Assign your character's transform in the Inspector

        private bool isJoystickPressed;
        private Vector2 joystickDirection;

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 direction = (eventData.position - (Vector2)joystickBackground.position).normalized;
            joystickDirection = direction;

            // Move the handle based on the input direction
            joystickHandle.anchoredPosition = new Vector2(direction.x * joystickBackground.rect.width / 2f, direction.y * joystickBackground.rect.height / 2f);

            // Rotate the handle based on the input direction
            float angle = Mathf.Atan2(joystickDirection.y, joystickDirection.x) * Mathf.Rad2Deg;
            joystickHandle.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isJoystickPressed = true;
            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isJoystickPressed = false;

            // Reset the handle position and rotation when the user releases the touch
            joystickHandle.anchoredPosition = Vector2.zero;
            joystickHandle.rotation = Quaternion.identity;
        }

        void Update()
        {
            // Optionally, you can use the joystickDirection for character movement
            if (isJoystickPressed && characterTransform != null)
            {
                // Example: Move and rotate the character based on joystick direction
                
                float rotateSpeed = 180f;

                Vector3 movement = new Vector3(joystickDirection.x, 0f, joystickDirection.y) * Time.deltaTime;
                characterTransform.Translate(movement);

                // Rotate the character based on joystick direction
                float targetAngle = Mathf.Atan2(joystickDirection.y, joystickDirection.x) * Mathf.Rad2Deg;
                characterTransform.rotation = Quaternion.RotateTowards(characterTransform.rotation, Quaternion.Euler(0f, targetAngle, 0f), rotateSpeed * Time.deltaTime);
            }
        }
    }




