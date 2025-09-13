public abstract class Ability
{
    // --- Datos base ---
    public string Name { get; }
    public string Description { get; }
    public float Cooldown { get; }              // en segundos
    public ResourceType ResourceType { get; }   // Vida o Mana
    public CostAmountType CostAmountType { get; }
    public float CostValue { get; }             // si PERCENT_MAX, usar 0.02 = 2%

    // --- Estado de runtime ---
    private float _lastCastAt = float.NegativeInfinity;

    protected Ability(
        string name,
        string description,
        float cooldown,
        ResourceType resourceType,
        CostAmountType costAmountType,
        float costValue)
    {
        Name = name;
        Description = description;
        Cooldown = Math.Max(0f, cooldown);
        ResourceType = resourceType;
        CostAmountType = costAmountType;
        CostValue = Math.Max(0f, costValue);
    }

    // ---------- API pública ----------
    public bool IsOnCooldown(float now) => (now - _lastCastAt) < Cooldown;

    public float RemainingCooldown(float now)
    {
        float remaining = Cooldown - (now - _lastCastAt);
        return remaining > 0f ? remaining : 0f;
    }

    public virtual bool CanCast(PlayableCarrier owner, float now)
    {
        if (owner == null) return false;
        if (IsOnCooldown(now)) return false;

        float cost = ComputeCost(owner);
        return HasEnoughResource(owner, cost);
    }

    /// Plantilla: valida, cobra costo, aplica efecto y arranca cooldown.
    public void Cast(PlayableCarrier owner, Carrier target, float now)
    {
        if (!CanCast(owner, now))
            throw new InvalidOperationException($"Ability '{Name}' cannot be cast now.");

        float cost = ComputeCost(owner);
        ChargeCost(owner, cost);
        ApplyEffect(owner, target);
        StartCooldown(now);
    }

    // ---------- Gancho de efecto (cada subclase implementa) ----------
    protected abstract void ApplyEffect(PlayableCarrier owner, Carrier target);

    // ---------- Costo ----------
    protected virtual float ComputeCost(PlayableCarrier owner)
    {
        if (CostAmountType == CostAmountType.Flat) return CostValue;

        // Percent del Max del recurso correspondiente
        float max = ResourceType == ResourceType.Life ? owner.Vida.Max : owner.Mana.Max;
        return max * CostValue;
    }

    protected virtual bool HasEnoughResource(PlayableCarrier owner, float cost)
    {
        if (ResourceType == ResourceType.Life)
            return owner.Vida.Current >= cost;
        return owner.Mana.Current >= cost;
    }

    protected virtual void ChargeCost(PlayableCarrier owner, float cost)
    {
        if (ResourceType == ResourceType.Life)
            owner.Vida.Spend(cost);
        else
            owner.Mana.Spend(cost);
    }

    protected virtual void StartCooldown(float now) => _lastCastAt = now;
}
