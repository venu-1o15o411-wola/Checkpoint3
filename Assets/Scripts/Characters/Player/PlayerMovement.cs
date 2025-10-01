using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
/*Clase: PlayerMovement
*Descripción: Gestiona el movimiento del jugador orientado por la cámara y la rotación
* suavizada del personaje. Integra animaciones, partículas y control físico con Rigidbody.
*Atributos:
*   - moveSpeed: velocidad de desplazamiento.
*   - acceleration: factor de aceleración aplicado al interpolar la velocidad.
*   - rotationSmoothTime: tiempo de suavizado para la rotación hacia la dirección objetivo.
*   - strafeTurnInfluence: influencia lateral al rotar cuando solo hay input horizontal.
*   - particles: sistema de partículas que se activa según la velocidad.
*   - cameraTransform: referencia a la cámara para orientar el movimiento.
*   - moveInput: entrada de movimiento (x: lateral, y: adelante/atrás).
*   - anim: referencia al Animator para actualizar el parámetro "Speed".
*   - rb: Rigidbody usado para el movimiento físico.
*   - currentYaw / yawVelocity: control interno para suavizar la rotación (SmoothDampAngle).
*/
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 12f;

    [SerializeField] private float acceleration = 12f;
    [SerializeField] private float rotationSmoothTime = 0.08f;
    [SerializeField] private float strafeTurnInfluence = 0.35f;

    [Header("Referencias")]
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Transform cameraTransform;

    private Vector2 moveInput;
    private Animator anim;
    private Rigidbody rb;
    private float currentYaw;
    private float yawVelocity;

    /*Método: Awake
    *Descripción: Inicializa referencias (Rigidbody, Animator, cámara) y configura las
    * restricciones físicas para evitar giros no deseados. Prepara la rotación suave.
    */
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.angularDamping = 999f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        currentYaw = transform.eulerAngles.y;
    }

    /*Método: OnMove
    *Descripción: Lee el vector de entrada del sistema de input (WASD/flechas/stick) para el movimiento.
    *Parámetros:
    *   - ctx: contexto del Input System con el valor Vector2 del movimiento.
    */
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    /*Método: Update
    *Descripción: Actualiza animaciones y efectos visuales en función de la velocidad plana real.
    */
    private void Update()
    {
        float planarSpeed = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude;
        anim?.SetFloat("Speed", planarSpeed);
        if (particles != null)
        {
            if (planarSpeed > 0.05f)
            {
                if (!particles.isPlaying) particles.Play();
            }
            else
            {
                if (particles.isPlaying) particles.Stop();
            }
        }
    }

    /*Método: FixedUpdate
    *Descripción: Aplica la lógica de movimiento y rotación suave usando físicas.
    * Calcula la dirección deseada en el plano según la cámara, interpola la velocidad
    * con aceleración y orienta el personaje con suavizado. Anula giros físicos residuales.
    */
    private void FixedUpdate()
    {
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f; camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f; camRight.Normalize();

        Vector3 desiredMoveDir = camRight * moveInput.x + camForward * moveInput.y;
        if (desiredMoveDir.sqrMagnitude > 1f) desiredMoveDir.Normalize();
        Vector3 targetVelocity = desiredMoveDir * moveSpeed;
        Vector3 blended = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
        float lerp = 1f - Mathf.Exp(-acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, blended, lerp);
        bool hasForward = Mathf.Abs(moveInput.y) > 0.001f;
        bool hasLateral = Mathf.Abs(moveInput.x) > 0.001f;

        Vector3 faceDir;
        if (hasForward)
        {
            faceDir = desiredMoveDir.sqrMagnitude > 0.0001f ? desiredMoveDir : transform.forward;
        }
        else if (hasLateral)
        {
            Vector3 biased = camForward * (1f - strafeTurnInfluence)
                           + camRight * Mathf.Sign(moveInput.x) * strafeTurnInfluence;
            biased.y = 0f;
            faceDir = biased.sqrMagnitude > 0.0001f ? biased.normalized : transform.forward;
        }
        else
        {
            faceDir = transform.forward;
        }

        if (faceDir.sqrMagnitude > 0.0001f)
        {
            float targetYaw = Mathf.Atan2(faceDir.x, faceDir.z) * Mathf.Rad2Deg;
            currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVelocity, rotationSmoothTime);
            rb.MoveRotation(Quaternion.Euler(0f, currentYaw, 0f));
        }
        rb.angularVelocity = Vector3.zero;
    }
}
