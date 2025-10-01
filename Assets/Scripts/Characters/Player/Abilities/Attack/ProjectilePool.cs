using System.Collections.Generic;
using UnityEngine;

/*Clase: ProjectilePool
*Descripción: Implementa un sistema de pooling para proyectiles. Permite
* instanciar un conjunto inicial de objetos, reutilizarlos para mejorar
* el rendimiento y devolverlos al pool al finalizar su uso.
*Atributos:
*   - projectilePrefab: prefab base del proyectil a instanciar.
*   - initialSize: número inicial de proyectiles a crear en el pool.
*   - prewarmOnAwake: indica si se debe precalentar el pool en Awake.
*   - pool: cola de proyectiles disponibles para usar.
*/
public class ProjectilePool : MonoBehaviour
{
    [Header("Optional default (can be null)")]
    public Projectile projectilePrefab;
    public int initialSize = 20;
    public bool prewarmOnAwake = true;

    private readonly Queue<Projectile> pool = new();

    /*Método: Awake
    *Descripción: Precalienta el pool en la inicialización si está habilitado y hay prefab asignado.
    */
    void Awake()
    {
        if (prewarmOnAwake && projectilePrefab != null)
            Prewarm();
    }

    /*Método: Prewarm
    *Descripción: Crea una cantidad inicial de proyectiles inactivos y los almacena en el pool.
    */
    private void Prewarm()
    {
        for (int i = 0; i < Mathf.Max(1, initialSize); i++)
        {
            var p = Instantiate(projectilePrefab, transform.position, transform.rotation);
            p.gameObject.SetActive(false);
            pool.Enqueue(p);
        }
    }

    /*Método: SetPrefab
    *Descripción: Establece un nuevo prefab para el pool y lo (re)precalienta.
    *Parámetros:
    *   - newPrefab: nuevo prefab de proyectil.
    *   - size: tamaño inicial opcional del pool.
    */
    public void SetPrefab(Projectile newPrefab, int size = -1)
    {
        projectilePrefab = newPrefab;
        if (size > 0) initialSize = size;
        while (pool.Count > 0) pool.Dequeue();
        if (projectilePrefab != null)
            Prewarm();
        else
            Debug.LogWarning("[ProjectilePool] SetPrefab(null) — no se pre-calienta.");
    }

    /*Método: Get
    *Descripción: Obtiene un proyectil del pool. Si está vacío, instancia uno nuevo.
    *Retorna: proyectil listo para ser utilizado.
    */
    public Projectile Get()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("[ProjectilePool] No prefab asignado. Llama SetPrefab() primero.");
            return null;
        }
        if (pool.Count == 0)
        {
            var p = Instantiate(projectilePrefab, transform);
            p.gameObject.SetActive(false);
            pool.Enqueue(p);
        }
        var proj = pool.Dequeue();
        proj.transform.SetParent(null);
        return proj;
    }

    /*Método: ReturnToPool
    *Descripción: Devuelve un proyectil al pool para ser reutilizado.
    *Parámetros:
    *   - p: proyectil a retornar.
    */
    public void ReturnToPool(Projectile p)
    {
        if (p == null) return;
        p.gameObject.SetActive(false);
        p.transform.SetParent(transform);
        pool.Enqueue(p);
    }
}
