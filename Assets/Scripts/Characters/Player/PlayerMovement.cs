using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 movementInput;
    public Transform cameraTransform;

    void Update()
    {
        if (cameraTransform == null)
            return;

        // Movimiento relativo a la cámara
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0;
        right.Normalize();

        Vector3 move = forward * movementInput.y + right * movementInput.x;
        transform.Translate(move * speed * Time.deltaTime, Space.World);

        // Hacer que el GameObject mire hacia donde mira la cámara (solo en el eje Y)
        Vector3 lookDirection = cameraTransform.forward;
        lookDirection.y = 0;
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
}