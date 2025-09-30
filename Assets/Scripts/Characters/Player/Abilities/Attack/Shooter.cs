using UnityEngine;

/*Clase: Shooter
*Descripción: Controla la lógica de disparo del jugador.
* Se encarga de instanciar proyectiles desde un pool, asignarles dirección,
* velocidad, daño y tiempo de vida antes de retornar al pool.
*Atributos:
*   - pool: referencia al pool de proyectiles.
*   - muzzle: punto de salida del proyectil.
*   - projectileSpeed: velocidad inicial del proyectil.
*   - projectileLifetime: tiempo de vida del proyectil antes de ser destruido o retornado.
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
    *Descripción: Lanza un proyectil desde el punto de salida definido.
    *Parámetros:
    *   - damage: cantidad de daño que inflige el proyectil.
    */
    public void Fire(float damage)
    {
        if (!pool || !muzzle) return;

        var proj = pool.Get();
        proj.ownerTag = ownerTag;
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
