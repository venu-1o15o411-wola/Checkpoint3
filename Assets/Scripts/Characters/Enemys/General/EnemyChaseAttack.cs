using UnityEngine;
using UnityEngine.AI;

/*Clase: EnemyChaseAttack
*Descripción: Controla el comportamiento de persecución y ataque de un enemigo usando NavMesh.
* Alterna entre patrullar y perseguir según la detección del jugador, y ejecuta ataques
* cuando el objetivo está dentro del rango de disparo.
*Atributos:
*   - detector: componente que detecta al jugador dentro de un área (trigger).
*   - patrol: patrulla opcional que se activa si no hay objetivo.
*   - chaseSpeed: velocidad de persecución.
*   - rotationSpeed: velocidad de rotación en grados por segundo.
*   - shootRange: rango de disparo.
*   - faceTarget: indica si el enemigo debe mirar hacia el objetivo.
*   - agent: componente NavMeshAgent para navegación.
*   - abilities: referencia a las habilidades del enemigo.
*   - shooter: referencia al componente Shooter para disparar.
*/
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAbilities))]
[RequireComponent(typeof(Shooter))]
public class EnemyChaseAttack : MonoBehaviour
{
    [Header("Refs")]
    public EnemyDetector detector;
    public EnemyPatrol patrol;

    [Header("Movimiento")]
    public float chaseSpeed = 3.5f;
    public float rotationSpeed = 720f;

    [Header("Combate")]
    public float shootRange = 12f;
    public bool faceTarget = true;

    private NavMeshAgent agent;
    private EnemyAbilities abilities;
    private Shooter shooter;

    /*Método: Awake
    *Descripción: Inicializa las referencias a NavMeshAgent, EnemyAbilities, Shooter,
    * así como detector y patrulla si no fueron asignados manualmente.
    */
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        abilities = GetComponent<EnemyAbilities>();
        shooter = GetComponent<Shooter>();
        detector = GetComponentInChildren<EnemyDetector>(true);
        patrol = GetComponent<EnemyPatrol>();
    }

    /*Método: Start
    *Descripción: Configura parámetros iniciales del NavMeshAgent para movimiento y rotación.
    */
    void Start()
    {
        agent.updateRotation = true;
        agent.autoBraking = true;
        agent.stoppingDistance = Mathf.Max(shootRange * 0.25f, 0.5f);
    }

    /*Método: Update
    *Descripción: Determina el comportamiento del enemigo en cada frame.
    * Si no hay objetivo: activa patrulla. Si hay objetivo: persigue o ataca según la distancia.
    */
    void Update()
    {
        var target = detector ? detector.currentTarget : null;
        if (!target)
        {
            if (patrol) patrol.enabled = true;
            if (agent)
            {
                agent.updateRotation = true;
                agent.isStopped = false;
            }
            return;
        }

        if (patrol) patrol.enabled = false;
        float dist = Vector3.Distance(transform.position, target.position);
        if (dist > shootRange)
        {
            agent.updateRotation = true;
            agent.isStopped = false;
            if (NavMesh.SamplePosition(target.position, out var hit, 5f, agent.areaMask))
                agent.SetDestination(hit.position);
        }
        else
        {
            agent.isStopped = true;
            agent.updateRotation = false;
            if (faceTarget)
            {
                Vector3 dir = target.position - transform.position;
                dir.y = 0f;
                if (dir.sqrMagnitude > 0.0001f)
                {
                    var targetRot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
                }
            }
            if (shooter && shooter.muzzle)
            {
                Vector3 aim = target.position - shooter.muzzle.position;
                aim.y = 0f;
                if (aim.sqrMagnitude > 0.0001f)
                    shooter.muzzle.forward = aim.normalized;
                Debug.DrawRay(shooter.muzzle.position, shooter.muzzle.forward * 3f, Color.red);
            }
            abilities.TryAttack();
        }
    }
}
