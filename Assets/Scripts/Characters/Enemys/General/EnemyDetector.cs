using UnityEngine;

/*Clase: EnemyDetector
*Descripción: Detecta al jugador u objetivos válidos dentro de un área de colisión.
* Utiliza un LayerMask para filtrar qué capas se consideran objetivos, guarda
* la referencia al objetivo detectado y orienta el transform hacia el jugador.
*Atributos:
*   - targetLayers: capas que se consideran objetivos válidos (ej. Player).
*   - player: referencia al transform del jugador encontrado en la escena.
*   - currentTarget: objetivo actual detectado por el trigger.
*/
public class EnemyDetector : MonoBehaviour
{
    [Header("Detección por capas")]
    [Tooltip("Capas que se consideran 'jugador' o objetivos válidos (p. ej. Player).")]
    [SerializeField] private LayerMask targetLayers;
    private Transform player;
    [HideInInspector] public Transform currentTarget;

    /*Método: Start
    *Descripción: Inicializa el detector (actualmente sin lógica adicional).
    */
    void Start()
    {
    }

    /*Método: Update
    *Descripción: Busca el objeto con tag "Player" si no se tiene referencia
    * y orienta el transform hacia el jugador si existe.
    */
    void Update()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
        if (player != null)
        {
            transform.LookAt(player);
        }
    }

    /*Método: OnTriggerEnter
    *Descripción: Detecta cuando un objeto entra al trigger. Si pertenece a las capas válidas,
    * se establece como objetivo actual.
    *Parámetros:
    *   - other: collider del objeto que entra en el área de detección.
    */
    private void OnTriggerEnter(Collider other)
    {
        if (IsInTargetLayers(other.gameObject.layer))
        {
            var t = other.attachedRigidbody ? other.attachedRigidbody.transform : other.transform;
            currentTarget = t.root;
        }
    }

    /*Método: OnTriggerExit
    *Descripción: Detecta cuando un objeto sale del trigger. Si es el objetivo actual,
    * lo elimina de la referencia.
    *Parámetros:
    *   - other: collider del objeto que sale del área de detección.
    */
    private void OnTriggerExit(Collider other)
    {
        if (IsInTargetLayers(other.gameObject.layer))
        {
            var t = other.attachedRigidbody ? other.attachedRigidbody.transform : other.transform;
            if (currentTarget == t.root) currentTarget = null;
        }
    }

    /*Método: IsInTargetLayers
    *Descripción: Verifica si una capa dada pertenece al LayerMask configurado.
    *Parámetros:
    *   - layer: número de capa a verificar.
    *Retorna: true si la capa está dentro de targetLayers, false en caso contrario.
    */
    private bool IsInTargetLayers(int layer) => (targetLayers.value & (1 << layer)) != 0;
}
