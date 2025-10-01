using UnityEngine;

/*Clase: Shooter
*Descripción: Controla la lógica de disparo de una entidad (jugador o enemigo).
* Utiliza un pool de proyectiles para instanciarlos, asignarles dirección, velocidad,
* daño y tiempo de vida antes de devolverlos al pool.
*Atributos:
*   - pool: referencia al pool de proyectiles.
*   - muzzle: punto de salida del proyectil.
*   - projectileSpeed: velocidad inicial del proyectil.
*   - projectileLifetime: tiempo de vida del proyectil antes de ser retornado.
*   - ownerTag: etiqueta del dueño del disparo (Player o Enemy) usada para definir la capa del proyectil.
*/
public class Shooter : MonoBehaviour
{
    [Header("Pool & Spawn")]
    public ProjectilePool pool;
    public Transform muzzle;
    public float projectileSpeed = 18f;
    public float projectileLifetime = 3f;

    [Header("Ownership")]
    public string ownerTag = "Player";

    /*Método: Fire
    *Descripción: Lanza un proyectil desde el punto de salida definido, configurando
    * su capa, dirección, velocidad, daño y tiempo de vida.
    *Parámetros:
    *   - damage: cantidad de daño que inflige el proyectil.
    */
    public void Fire(float damage)
    {
        if (!pool || !muzzle) return;

        var proj = pool.Get();
        int layer = LayerMask.NameToLayer(ownerTag == "Enemy" ? "EnemyProjectile" : "PlayerProjectile");
        proj.gameObject.layer = layer;
        proj.Launch(
            muzzle.position,
            muzzle.forward,
            projectileSpeed,
            damage,
            projectileLifetime,
            returnToPool: pool.ReturnToPool
        );
    }
}
