using UnityEngine;

/*Clase: BreakableBox
*Descripción: Representa una caja destructible que requiere cierto número de impactos
* para romperse. Al destruirse puede soltar prefabs (vida, maná, etc.) y reproducir sonidos.
*Atributos:
*   - hitsToDestroy: número de impactos necesarios para destruir la caja.
*   - dropPrefabs: lista de prefabs que pueden ser instanciados al destruirse.
*   - destroySFX1: sonido reproducido al recibir un impacto.
*   - destroySFX2: sonido reproducido al destruirse por completo.
*   - projectileLayer: nombre de la capa usada para proyectiles válidos.
*   - currentHits: contador de impactos recibidos.
*/
public class BreakableBox : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int hitsToDestroy = 2;
    [SerializeField] private GameObject[] dropPrefabs;
    [SerializeField] private AudioClip destroySFX1;
    [SerializeField] private AudioClip destroySFX2;
    [SerializeField] private string projectileLayer = "PlayerProjectile";

    private int currentHits = 0;

    /*Método: Start
    *Descripción: Inicialización de la caja (actualmente vacío).
    */
    void Start()
    {
    }

    /*Método: OnCollisionEnter
    *Descripción: Detecta colisiones con otros objetos. Si el objeto pertenece
    * a la capa de proyectiles, incrementa los impactos y destruye la caja si se supera el límite.
    *Parámetros:
    *   - collision: información de la colisión detectada.
    */
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisión con: " + collision.gameObject.layer);
        if (collision.gameObject.layer == 8)
        {
            AudioManager.Instance.PlaySFX(destroySFX1);
            currentHits++;
            if (currentHits >= hitsToDestroy)
            {
                DestroyBox();
            }
        }
    }

    /*Método: DestroyBox
    *Descripción: Maneja la destrucción de la caja. Reproduce sonido, instancia
    * un prefab aleatorio si está configurado y destruye el objeto.
    */
    private void DestroyBox()
    {
        AudioManager.Instance.PlaySFX(destroySFX2);
        if (dropPrefabs != null && dropPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, dropPrefabs.Length);
            Instantiate(dropPrefabs[randomIndex], transform.position, dropPrefabs[randomIndex].transform.rotation);
        }
        Destroy(gameObject);
    }
}
