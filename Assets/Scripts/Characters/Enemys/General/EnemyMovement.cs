using UnityEngine;
using UnityEngine.AI;

/*Clase: EnemyMovement
*Descripción: Controla la animación de movimiento del enemigo en función de la velocidad
* obtenida del NavMeshAgent. Actualiza el parámetro "Speed" del Animator.
*Atributos:
*   - agent: referencia al NavMeshAgent para obtener la velocidad.
*   - animator: referencia al Animator para actualizar las animaciones.
*/
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    /*Método: Awake
    *Descripción: Inicializa referencias a NavMeshAgent y Animator.
    */
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    /*Método: Update
    *Descripción: Actualiza el parámetro "Speed" del Animator según la velocidad del agente.
    */
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
