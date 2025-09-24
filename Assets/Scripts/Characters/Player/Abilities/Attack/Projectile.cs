using UnityEngine;

/*Clase: Projectile
*Descripción: Representa un proyectil disparado por un jugador u otra entidad.
* Se encarga de controlar su movimiento, tiempo de vida y colisiones,
* retornando al pool cuando expira o impacta contra un objeto.
*Atributos:
*   - rb: componente Rigidbody usado para el movimiento físico del proyectil.
*   - returnToPool: acción que devuelve el proyectil al pool.
*   - lifeTimer: tiempo transcurrido desde que fue lanzado.
*   - maxLife: tiempo máximo de vida del proyectil.
*   - damage: cantidad de daño que inflige el proyectil al impactar.
*/
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private System.Action<Projectile> returnToPool;
    private float lifeTimer;
    private float maxLife;
    private float damage;

    /*Método: Awake
    *Descripción: Inicializa el Rigidbody del proyectil y configura
    * la detección de colisiones y gravedad.
    */
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    /*Método: Launch
    *Descripción: Lanza el proyectil desde una posición y dirección determinadas,
    * asignándole velocidad, daño, tiempo de vida y acción de retorno al pool.
    *Parámetros:
    *   - position: posición inicial del proyectil.
    *   - direction: dirección en la que se mueve el proyectil.
    *   - speed: velocidad inicial del proyectil.
    *   - damage: daño que inflige al impactar.
    *   - lifetime: duración máxima antes de desaparecer.
    *   - returnToPool: acción que se ejecuta al terminar la vida útil.
    */
    public void Launch(Vector3 position, Vector3 direction, float speed, float damage, float lifetime, System.Action<Projectile> returnToPool)
    {
        transform.position = position;
        transform.forward = direction.sqrMagnitude > 0.0001f ? direction.normalized : transform.forward;

        this.damage = damage;
        this.maxLife = Mathf.Max(0.1f, lifetime);
        this.returnToPool = returnToPool;

        lifeTimer = 0f;
        rb.linearVelocity = transform.forward * speed;
        gameObject.SetActive(true);
    }

    /*Método: Update
    *Descripción: Incrementa el temporizador de vida y elimina el proyectil
    * cuando se supera su tiempo máximo.
    */
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= maxLife)
        {
            Despawn();
        }
    }

    /*Método: OnCollisionEnter
    *Descripción: Maneja la colisión del proyectil con otro objeto.
    * Actualmente finaliza el proyectil, pendiente de aplicar daño real.
    *Parámetros:
    *   - other: información de la colisión detectada.
    */
    void OnCollisionEnter(Collision other)
    {
        // TODO: aplicar daño real: other.collider.GetComponent<Health>()?.TakeDamage(damage);
        Despawn();
    }

    /*Método: Despawn
    *Descripción: Detiene el movimiento del proyectil y lo retorna al pool.
    */
    private void Despawn()
    {
        rb.linearVelocity = Vector3.zero;
        returnToPool?.Invoke(this);
    }
}
