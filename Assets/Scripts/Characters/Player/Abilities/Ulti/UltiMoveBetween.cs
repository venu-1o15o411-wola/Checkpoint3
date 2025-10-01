using System.Collections;
using UnityEngine;

/*Clase: UltiMoveBetween
*Descripción: Controla el comportamiento de una habilidad definitiva de área
* que se mueve verticalmente entre dos posiciones, inflige daño en un área
* al llegar al punto máximo y luego regresa a su posición inicial.
*Atributos:
*   - startYOffset: desplazamiento inicial en Y desde la posición base.
*   - endYOffset: desplazamiento final en Y desde la posición base.
*   - travelDuration: tiempo que tarda en alcanzar la posición final.
*   - holdTime: tiempo que permanece en la posición final antes de regresar.
*   - returnDuration: tiempo que tarda en volver a la posición inicial.
*   - hitRadius: radio del área en la que inflige daño.
*   - isActive: indica si la habilidad está activa.
*   - owner: transform del objeto que lanzó la habilidad.
*   - basePos: posición base desde donde se origina la habilidad.
*   - damage: cantidad de daño que inflige.
*   - ownerTag: etiqueta del dueño (Player/Enemy) para distinguir objetivos.
*   - hasDealtDamage: controla si ya se aplicó el daño.
*/
public class UltiMoveBetween : MonoBehaviour
{
    [Header("Motion")]
    [SerializeField] private float startYOffset = 0f;
    [SerializeField] private float endYOffset = 5f;
    [SerializeField] private float travelDuration = 0.35f;
    [SerializeField] private float holdTime = 0.25f;
    [SerializeField] private float returnDuration = 0.35f;
    [Header("Hit")]
    [SerializeField] private float hitRadius = 3f;
    private bool isActive;
    private Transform owner;
    private Vector3 basePos;
    private float damage;
    private string ownerTag;
    private bool hasDealtDamage;

    /*Método: ActivateAtBasePosition
    *Descripción: Activa la habilidad en una posición base, configurando daño,
    * dueño y etiqueta del objetivo. Inicia la secuencia de movimiento y daño.
    *Parámetros:
    *   - ownerGO: objeto que lanza la habilidad.
    *   - basePosition: posición base donde se activa.
    *   - damage: daño a infligir.
    *   - ownerTag: etiqueta del dueño (Player o Enemy).
    */
    public void ActivateAtBasePosition(GameObject ownerGO, Vector3 basePosition, float damage, string ownerTag)
    {
        if (isActive) return;

        this.owner = ownerGO ? ownerGO.transform : null;
        this.basePos = basePosition;
        this.damage = Mathf.Max(0f, damage);
        this.ownerTag = ownerTag;
        this.hasDealtDamage = false;
        gameObject.SetActive(true);
        StartCoroutine(RunSequence());
    }

    /*Método: RunSequence
    *Descripción: Corrutina que controla el movimiento de la habilidad.
    * Sube desde la posición inicial hasta la final, aplica daño en área,
    * espera y regresa a la posición inicial.
    */
    private IEnumerator RunSequence()
    {
        isActive = true;
        Vector3 start = basePos + Vector3.up * startYOffset;
        Vector3 end = basePos + Vector3.up * endYOffset;
        float t = 0f;
        while (t < travelDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / travelDuration);
            transform.position = Vector3.Lerp(start, end, a);
            yield return null;
        }
        if (!hasDealtDamage && damage > 0f)
        {
            DealOneShotDamageAt(transform.position);
            hasDealtDamage = true;
        }
        yield return new WaitForSeconds(holdTime);
        t = 0f;
        while (t < returnDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / returnDuration);
            transform.position = Vector3.Lerp(end, start, a);
            yield return null;
        }
        isActive = false;
        gameObject.SetActive(false);
    }

    /*Método: DealOneShotDamageAt
    *Descripción: Aplica daño a todos los objetivos dentro del radio especificado,
    * filtrando por capas y etiqueta contraria al dueño.
    *Parámetros:
    *   - center: posición central donde se aplica el daño en área.
    */
    private void DealOneShotDamageAt(Vector3 center)
    {
        int targetLayer = LayerMask.NameToLayer(ownerTag == "Enemy" ? "Player" : "Enemy");
        int layerMask = 1 << targetLayer;

        Collider[] hits = Physics.OverlapSphere(center, hitRadius, layerMask, QueryTriggerInteraction.Collide);
        if (hits == null || hits.Length == 0) return;

        for (int i = 0; i < hits.Length; i++)
        {
            var col = hits[i];
            if (!col) continue;

            var damageable = col.GetComponentInParent<IDamageable>();
            if (damageable == null) continue;

            var comp = damageable as Component;
            if (!comp) continue;

            var targetRoot = comp.gameObject;
            if (!targetRoot) continue;

            string targetTag = ownerTag == "Enemy" ? "Player" : "Enemy";
            if (!targetRoot.CompareTag(targetTag)) continue;

            damageable.ReceiveDamage(damage);
        }
    }

#if UNITY_EDITOR
    /*Método: OnDrawGizmosSelected
    *Descripción: Dibuja en la vista de escena una esfera representando el área
    * de impacto de la habilidad cuando el objeto está seleccionado.
    */
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position == Vector3.zero
                ? transform.position + Vector3.up * endYOffset
                : transform.position,
            hitRadius
        );
    }
#endif
}
