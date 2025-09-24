using UnityEngine;
using UnityEngine.InputSystem;

/*Clase: PlayerMovement
*Descripción: Gestiona el movimiento del jugador utilizando el Input System.
* Controla la velocidad, animaciones y efectos de partículas al desplazarse.
*Atributos:
*   - speed: velocidad de movimiento del jugador.
*   - particles: sistema de partículas activado al moverse.
*   - movementInput: vector de entrada proveniente del Input System.
*   - anim: referencia al Animator para actualizar las animaciones.
*   - rb: referencia al Rigidbody para manejar la física del movimiento.
*/
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private ParticleSystem particles;
    private Vector2 movementInput;
    private Animator anim;
    private Rigidbody rb;

    /*Método: Awake
    *Descripción: Inicializa las referencias al Rigidbody y al Animator del jugador.
    */
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    /*Método: OnMove
    *Descripción: Captura el valor de entrada del Input System para el movimiento.
    *Parámetros:
    *   - context: contexto de la acción de entrada que provee el vector de movimiento.
    */
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    /*Método: Update
    *Descripción: Actualiza la posición, velocidad, animaciones y rotación del jugador
    * en función de la entrada de movimiento. También activa las partículas al desplazarse.
    */
    void Update()
    {
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        rb.linearVelocity = move * speed;
        transform.Translate(move * speed * Time.deltaTime, Space.World);
        anim.SetFloat("Speed", rb.linearVelocity.magnitude);

        if (move.sqrMagnitude > 0.001f)
        {
            particles.Play();
            transform.rotation = Quaternion.LookRotation(move);
        }
    }
}
