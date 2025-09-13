
public class PlayableCarrier : Carrier
{
    private Element element;
    public Mana Mana { get; }

    public PlayableCarrier(
        int id,
        string name,
        string description,
        Life vida,
        Mana mana,
        Element element)
        : base(id, name, description, vida)
    {
        Mana = mana;
        this.element = element;
    }

    public Element GetElemento() => element;

    public void Tick(float dt)
    {
        if (dt <= 0f) return;
        Mana.RecoveryPerSecond(dt);
    }

    public bool TrySpendMana(float amount)
    {
        if (amount <= 0f) return true;
        if (Mana.Current < amount) return false;
        Mana.Spend(amount);
        return true;
    }

    /// <summary>Porcentaje de mana actual [0..1].</summary>
    public float ManaPercent() => Mana.CurrentPercent();

}
