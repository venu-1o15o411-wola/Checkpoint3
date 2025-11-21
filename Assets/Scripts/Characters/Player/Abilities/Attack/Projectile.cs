using System;
using UnityEngine;

/*Clase: Projectile
*Descripción: Gestiona el comportamiento de un proyectil: lanzamiento, movimiento,
* vida útil, detección de impactos y retorno al pool o desactivación.
*Atributos:
*   - damage / lifetime / speed: valores de daño, duración y velocidad del proyectil (runtime).
*   - rb: referencia al Rigidbody para mover el proyectil.
*   - lifeTimer: temporizador interno de vida restante.
*   - returnToPool: acción para retornar el proyectil a un pool si está disponible.
*/
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Header("Runtime (read-only)")]
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    [SerializeField] private float speed;

    private Rigidbody rb;
    private float lifeTimer;
    private Action<Projectile> returnToPool;

    /*Método: Awake
    *Descripción: Obtiene y almacena la referencia al componente Rigidbody.
    */
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /*Método: OnEnable
    *Descripción: Reinicia el temporizador de vida cuando el proyectil se activa.
    */
    void OnEnable()
    {
        lifeTimer = lifetime;
    }

    /*Método: Update
    *Descripción: Decrementa el temporizador de vida y elimina/despawns el proyectil
    * cuando esta llega a cero.
    */
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Despawn();
        }
    }

    /*Método: Launch
    *Descripción: Inicializa y lanza el proyectil desde una posición y dirección dadas,
    * configurando su velocidad, daño, vida útil y callback de retorno al pool.
    *Parámetros:
    *   - startPosition: posición inicial del proyectil.
    *   - direction: dirección de lanzamiento.
    *   - projectileSpeed: velocidad del proyectil.
    *   - damage: daño a aplicar en impacto.
    *   - lifetime: duración máxima antes de desaparecer.
    *   - returnToPool: acción para retornar el proyectil a un pool.
    */
    public void Launch(
        Vector3 startPosition,
        Vector3 direction,
        float projectileSpeed,
        float damage,
        float lifetime,
        Action<Projectile> returnToPool
    )
    {
        transform.position = startPosition;
        var euler = transform.eulerAngles;
        euler.x = 90f;
        transform.eulerAngles = euler;
        transform.forward = direction.sqrMagnitude > 0.0001f ? direction.normalized : transform.forward;

        this.speed = projectileSpeed;
        this.damage = damage;
        this.lifetime = lifetime;
        this.returnToPool = returnToPool;

        lifeTimer = lifetime;

        rb.linearVelocity = transform.forward * speed;
        gameObject.SetActive(true);
    }

    /*Método: OnCollisionEnter
    *Descripción: Gestiona impactos usando colisiones físicas (Collider no trigger).
    *Parámetros:
    *   - other: información de la colisión.
    */
    private void OnCollisionEnter(Collision other)
    {
        HandleHit(other.collider);
    }

    /*Método: OnTriggerEnter
    *Descripción: Gestiona impactos usando colisiones con triggers.
    *Parámetros:
    *   - other: collider del objeto con el que se solapa.
    */
    private void OnTriggerEnter(Collider other)
    {
        HandleHit(other);
    }

    /*Método: HandleHit
    *Descripción: Aplica daño al objeto impactado si implementa IDamageable y
    * finaliza el proyectil.
    *Parámetros:
    *   - col: collider del objeto impactado.
    */
    private void HandleHit(Collider col)
    {
        var damageable = col.GetComponentInParent<IDamageable>();
        if (damageable != null && damage > 0f)
        {
            damageable.ReceiveDamage(damage);
        }
        Despawn();
    }

    /*Método: Despawn
    *Descripción: Detiene el movimiento y devuelve el proyectil al pool si hay callback,
    * o lo desactiva en caso contrario.
    */
    private void Despawn()
    {
        rb.linearVelocity = Vector3.zero;

        if (returnToPool != null)
        {
            returnToPool(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
