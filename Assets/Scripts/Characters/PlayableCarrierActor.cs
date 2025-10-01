using UnityEngine;

/*Clase: PlayableCarrierActor
*Descripción: Actor de Unity que representa a un portador jugable (PlayableCarrier)
* en la escena. Se encarga de inicializar y sincronizar la instancia de dominio
* con parámetros configurados en el prefab, como vida, maná, elemento y habilidades.
*Atributos:
*   - id: identificador único del personaje.
*   - displayName: nombre visible del personaje.
*   - description: descripción del personaje.
*   - lifeMax: vida máxima inicial.
*   - manaMax: maná máximo inicial.
*   - manaRecoveryRate: tasa de regeneración de maná por segundo.
*   - element: elemento asociado (ej. Fire, Water).
*   - Carrier: instancia del modelo de dominio PlayableCarrier.
*   - Life / Mana / Element: atajos para exponer propiedades de Carrier.
*/
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

    public PlayableCarrier Carrier { get; private set; }

    public Life Life => Carrier?.Life;
    public Mana Mana => Carrier?.Mana;
    public Element Element => Carrier != null ? Carrier.GetElemento() : element;

    /*Método: Awake
    *Descripción: Inicializa la instancia del portador jugable con vida, maná y
    * demás parámetros configurados en el inspector del prefab.
    */
    private void Awake()
    {
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

    /*Método: Update
    *Descripción: Propaga la regeneración de maná cada frame usando el método Tick del Carrier.
    */
    private void Update()
    {
        if (Carrier != null)
            Carrier.Tick(Time.deltaTime);
    }

    /*Método: ApplyDamage
    *Descripción: Aplica daño al portador jugable.
    *Parámetros:
    *   - amount: cantidad de daño a aplicar.
    */
    public void ApplyDamage(float amount) => Carrier?.ApplyDamage(amount);

    /*Método: Heal
    *Descripción: Aplica curación al portador jugable.
    *Parámetros:
    *   - amount: cantidad de curación a aplicar.
    */
    public void Heal(float amount) => Carrier?.Heal(amount);

    /*Método: SetAbilities
    *Descripción: Asigna un conjunto de habilidades al portador jugable.
    *Parámetros:
    *   - set: conjunto de habilidades (AbilitySet).
    */
    public void SetAbilities(AbilitySet set) => Carrier?.SetAbilities(set);
}
