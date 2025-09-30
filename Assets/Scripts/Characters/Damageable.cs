using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Header("Health")]
    public float maxLife = 100f;
    public float currentLife = -1f;
    public bool destroyOnDeath = false;

    public event Action<float, float> OnLifeChanged;
    public event Action OnDied;

    void Awake()
    {
        if (currentLife < 0f) currentLife = maxLife;
    }

    public void SetLife(float value)
    {
        float clamped = Mathf.Clamp(value, 0f, maxLife);
        if (Mathf.Approximately(clamped, currentLife)) return;
        currentLife = clamped;
        OnLifeChanged?.Invoke(currentLife, maxLife);
        if (currentLife <= 0f) Die();
    }

    public void ReceiveDamage(float amount)
    {
        if (amount <= 0f) return;
        SetLife(currentLife - amount);
    }

    public void Heal(float amount)
    {
        if (amount <= 0f) return;
        SetLife(currentLife + amount);
    }

    public void ResetLifeFull()
    {
        SetLife(maxLife);
    }

    void Die()
    {
        OnDied?.Invoke();
        if (destroyOnDeath) Destroy(gameObject);
    }
}
