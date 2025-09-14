using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 movementInput;

    // Este método será llamado automáticamente por el Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        // Movimiento en el plano XZ (sin cámara)
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        transform.Translate(move * speed * Time.deltaTime, Space.World);

        // Opcional: hacer que el GameObject mire hacia la dirección de movimiento
        if (move.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(move);
        }
    }
}