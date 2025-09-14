using UnityEngine;

// Usa tus namespaces/clases del dominio
// (ajusta los using según dónde tengas tus clases)
public class PlayableCarrierActor : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] private int id = 1;
    [SerializeField] private string displayName = "Hero";
    [TextArea][SerializeField] private string description = "Playable character";

    [Header("Stats")]
    [SerializeField] private float lifeMax = 150f;
    [SerializeField] private float manaMax = 100f;
    [Tooltip("Por segundo. Si tu Mana usa fracción (0.02=2%), pon 0.02; si usa % entero, pon 2.")]
    [SerializeField] private float manaRecoveryRate = 0.02f;

    [Header("Element")]
    [SerializeField] private Element element = Element.Fire;

    // Instancia de dominio (tu modelo POO)
    public PlayableCarrier Carrier { get; private set; }

    // Atajos útiles para UI / depuración
    public Life Life => Carrier?.Life;
    public Mana Mana => Carrier?.Mana;
    public Element Element => Carrier != null ? Carrier.GetElemento() : element;

    private void Awake()
    {
        // Instanciamos tu dominio con vida/mana propios del prefab
        var life = new Life(lifeMax, lifeMax);
        var mana = new Mana(manaMax, manaMax, manaRecoveryRate);

        Carrier = new PlayableCarrier(
            id: id,
            name: displayName,
            description: description,
            life: life,
            mana: mana,
            element: element
        );
    }

    private void Update()
    {
        // Propaga la regeneración de mana (usa tu Mana.RecoveryPerSecond)
        if (Carrier != null)
            Carrier.Tick(Time.deltaTime);
    }

    // Métodos de ayuda (útiles para probar sin UI)
    public void ApplyDamage(float amount) => Carrier?.ApplyDamage(amount);
    public void Heal(float amount) => Carrier?.Heal(amount);

    // Si luego asignas AbilitySet desde otro sistema, expón un setter
    public void SetAbilities(AbilitySet set) => Carrier?.SetAbilities(set);
}
