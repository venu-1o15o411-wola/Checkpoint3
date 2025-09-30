using System.Collections;
using UnityEngine;

public class UltiMoveBetween : MonoBehaviour
{
    [Header("Animación")]
    [Tooltip("Si > 0 usa duración fija; si 0 usa velocidad constante.")]
    public float duration = 1.0f;
    [Tooltip("Usada solo si duration == 0 (unidades/seg).")]
    public float speed = 3f;
    [Tooltip("Tiempo que permanece activo tras llegar al punto final.")]
    public float holdTime = 0.5f;

    [Header("Offsets en Y (relativos a la base)")]
    [Tooltip("Desfase inicial en Y (ej: -2 para nacer bajo el suelo).")]
    public float startYOffset = -2f;
    [Tooltip("Desfase final en Y (ej: 0 para llegar al nivel de base).")]
    public float endYOffset = 0f;

    [Header("Bloquear movimiento del dueño")]
    public bool lockOwnerMovement = true;

    // runtime
    bool running;
    Coroutine routine;
    GameObject owner;
    MonoBehaviour movementComp;
    Rigidbody ownerRb;

    /// <summary>
    /// Activa la ulti en una posición base (ya calculada por ti delante del muzzle).
    /// Este script solo modifica Y: baseY + startYOffset -> baseY + endYOffset.
    /// </summary>
    public void ActivateAtBasePosition(GameObject ultiOwner, Vector3 basePos)
    {
        if (running) return;

        owner = ultiOwner;

        Vector3 startPos = new Vector3(basePos.x, basePos.y + startYOffset, basePos.z);
        Vector3 endPos = new Vector3(basePos.x, basePos.y + endYOffset, basePos.z);

        gameObject.SetActive(true);

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(RunSequence(startPos, endPos));
    }

    IEnumerator RunSequence(Vector3 startPos, Vector3 endPos)
    {
        running = true;
        transform.position = startPos;

        // bloquear movimiento si aplica
        System.Action unlock = null;
        if (lockOwnerMovement && owner != null)
            unlock = TryLockOwner(owner);

        // mover (por duración o por velocidad)
        if (duration > 0f)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                transform.position = Vector3.Lerp(startPos, endPos, Mathf.Clamp01(t));
                yield return null;
            }
        }
        else
        {
            while ((transform.position - endPos).sqrMagnitude > 0.0001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
                yield return null;
            }
        }

        if (holdTime > 0f)
            yield return new WaitForSeconds(holdTime);

        unlock?.Invoke();
        gameObject.SetActive(false);

        running = false;
        routine = null;
    }

    // Bloqueo simple: deshabilita tu componente de movimiento y frena el RB
    System.Action TryLockOwner(GameObject who)
    {
        movementComp = who.GetComponent<PlayerMovement>();
        if (movementComp != null) movementComp.enabled = false;

        ownerRb = who.GetComponent<Rigidbody>();
        if (ownerRb != null)
        {
            ownerRb.linearVelocity = Vector3.zero;   // Unity 6
            ownerRb.angularVelocity = Vector3.zero;
        }

        // Si usas PlayerInput y quieres bloquear inputs, podrías deshabilitar su mapa aquí.

        return () =>
        {
            if (movementComp != null) movementComp.enabled = true;
            // Rehabilitar input si lo deshabilitaste.
        };
    }
}
