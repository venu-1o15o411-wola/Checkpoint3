using UnityEngine;
using UnityEngine.AI;

/*Clase: EnemyPatrol
*Descripción: Controla la patrulla de un enemigo a través de una lista ordenada de waypoints.
* Permite esperar en cada punto un tiempo configurable y continuar en bucle si se desea.
*Atributos:
*   - waypoints: puntos de patrulla en orden.
*   - waitAtPoint: tiempo de espera en cada waypoint antes de continuar.
*   - loop: indica si, al finalizar la lista de waypoints, vuelve al primero.
*   - agent: referencia al NavMeshAgent para moverse entre puntos.
*   - index: índice del waypoint actual.
*   - waitTimer: temporizador interno de espera en un waypoint.
*/
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAbilities))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Waypoints (en orden)")]
    public Transform[] waypoints;
    [Header("Config")]
    public float waitAtPoint = 1.0f;
    public bool loop = true;
    private NavMeshAgent agent;
    private int index;
    private float waitTimer;

    /*Método: Awake
    *Descripción: Obtiene la referencia al componente NavMeshAgent.
    */
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    /*Método: Start
    *Descripción: Valida la lista de waypoints e inicia la patrulla en el primer punto.
    */
    void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            enabled = false;
            return;
        }
        index = 0;
        GoTo(index);
    }

    /*Método: Update
    *Descripción: Gestiona el movimiento entre waypoints, incluyendo la espera en cada punto
    * y el avance al siguiente. Si no hay bucle y se alcanza el final, deshabilita el componente.
    */
    void Update()
    {
        if (agent.pathPending) return;
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (waitTimer <= 0f)
            {
                waitTimer = waitAtPoint;
            }
            else
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    index++;
                    if (index >= waypoints.Length)
                    {
                        if (loop) index = 0;
                        else { enabled = false; return; }
                    }
                    GoTo(index);
                }
            }
        }
    }

    /*Método: GoTo
    *Descripción: Ordena al NavMeshAgent desplazarse al waypoint indicado por índice.
    *Parámetros:
    *   - i: índice del waypoint de destino.
    */
    private void GoTo(int i)
    {
        if (waypoints[i] != null)
        {
            agent.SetDestination(waypoints[i].position);
        }
    }

    /*Método: OnDrawGizmosSelected
    *Descripción: Dibuja líneas en la escena entre los waypoints para visualizar la ruta
    * cuando el objeto está seleccionado. Si loop es true, cierra el circuito.
    */
    void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Length < 2) return;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (!waypoints[i] || !waypoints[i + 1]) continue;
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
        if (loop && waypoints[0] && waypoints[^1])
        {
            Gizmos.DrawLine(waypoints[^1].position, waypoints[0].position);
        }
    }
}
