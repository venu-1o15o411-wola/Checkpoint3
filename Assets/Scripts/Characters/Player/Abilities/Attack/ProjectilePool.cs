using System.Collections.Generic;
using UnityEngine;
public class ProjectilePool : MonoBehaviour
{
    [Header("Optional default (can be null)")]
    public Projectile projectilePrefab;
    public int initialSize = 20;
    public bool prewarmOnAwake = true;

    private readonly Queue<Projectile> pool = new();

    void Awake()
    {
        if (prewarmOnAwake && projectilePrefab != null)
            Prewarm();
    }

    private void Prewarm()
    {
        for (int i = 0; i < Mathf.Max(1, initialSize); i++)
        {
            var p = Instantiate(projectilePrefab, transform);
            p.gameObject.SetActive(false);
            pool.Enqueue(p);
        }
    }

    // Llama esto al elegir personaje para fijar el prefab y (re)precalentar
    public void SetPrefab(Projectile newPrefab, int size = -1)
    {
        projectilePrefab = newPrefab;
        if (size > 0) initialSize = size;

        // limpiar pool anterior
        while (pool.Count > 0) pool.Dequeue();

        if (projectilePrefab != null)
            Prewarm();
        else
            Debug.LogWarning("[ProjectilePool] SetPrefab(null) — no se pre-calienta.");
    }

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

    public void ReturnToPool(Projectile p)
    {
        if (p == null) return;
        p.gameObject.SetActive(false);
        p.transform.SetParent(transform);
        pool.Enqueue(p);
    }
}
