/*Clase: PlayableCarrier
*Descripción: Representa un portador jugable dentro del juego.
* Hereda de Carrier e incluye recursos adicionales como maná, un conjunto de habilidades
* y un elemento asociado. Permite administrar el uso de maná, habilidades y regeneración.
*Atributos:
*   - element: elemento asociado al portador (Fire, Water, Earth, Air).
*   - Mana: recurso de maná del portador.
*   - Abilities: conjunto de habilidades disponibles.
*/
public class PlayableCarrier : Carrier
{
    private Element element;
    public Mana Mana { get; }
    public AbilitySet Abilities { get; private set; }

    /*Constructor: PlayableCarrier
    *Descripción: Inicializa un portador jugable con id, nombre, descripción, vida, maná y elemento.
    *Parámetros:
    *   - id: identificador único del portador.
    *   - name: nombre del portador.
    *   - description: descripción del portador.
    *   - life: recurso de vida.
    *   - mana: recurso de maná.
    *   - element: elemento asociado al portador.
    */
    public PlayableCarrier(
        int id,
        string name,
        string description,
        Life life,
        Mana mana,
        Element element)
        : base(id, name, description, life)
    {
        Mana = mana;
        this.element = element;
    }

    /*Método: GetElemento
    *Descripción: Devuelve el elemento asociado al portador.
    *Retorna: valor del enum Element.
    */
    public Element GetElemento() => element;

    /*Método: SetAbilities
    *Descripción: Asigna un conjunto de habilidades al portador.
    *Parámetros:
    *   - set: conjunto de habilidades (AbilitySet).
    */
    public void SetAbilities(AbilitySet set) => Abilities = set;

    /*Método: Tick
    *Descripción: Regenera maná en función del tiempo transcurrido.
    *Parámetros:
    *   - dt: tiempo transcurrido en segundos.
    */
    public void Tick(float dt)
    {
        if (dt <= 0f) return;
        Mana.RecoveryPerSecond(dt);
    }

    /*Método: TrySpendMana
    *Descripción: Intenta gastar una cantidad de maná.
    *Parámetros:
    *   - amount: cantidad de maná a gastar.
    *Retorna: true si el gasto fue exitoso, false si no alcanza el maná.
    */
    public bool TrySpendMana(float amount)
    {
        if (amount <= 0f) return true;
        if (Mana.Current < amount) return false;
        Mana.Spend(amount);
        return true;
    }

    /*Método: ManaPercent
    *Descripción: Calcula el porcentaje actual de maná.
    *Retorna: valor flotante entre 0 y 1.
    */
    public float ManaPercent() => Mana.CurrentPercent();

    /*Método: CanUseAttack
    *Descripción: Verifica si la habilidad de ataque está disponible y puede lanzarse.
    *Retorna: true si es posible usar el ataque.
    */
    public bool CanUseAttack() => Abilities != null && Abilities.Attack.canCast(this);

    /*Método: UseAttack
    *Descripción: Ejecuta la habilidad de ataque contra un objetivo.
    *Parámetros:
    *   - target: objetivo que recibirá el ataque.
    */
    public void UseAttack(Carrier target)
    {
        if (Abilities == null) return;
        Abilities.Attack.Cast(this, target);
    }

    /*Método: CanUseHeal
    *Descripción: Verifica si la habilidad de curación está disponible y puede lanzarse.
    *Retorna: true si es posible usar la curación.
    */
    public bool CanUseHeal() => Abilities != null && Abilities.Heal.canCast(this);

    /*Método: UseHeal
    *Descripción: Ejecuta la habilidad de curación sobre un objetivo.
    *Parámetros:
    *   - target: objetivo que recibirá la curación.
    */
    public void UseHeal()
    {
        if (Abilities == null) return;
        Abilities.Heal.Cast(this, this);
    }

    /*Método: CanUseArea
    *Descripción: Verifica si la habilidad de daño en área está disponible y puede lanzarse.
    *Retorna: true si es posible usar la habilidad de área.
    */
    public bool CanUseArea() => Abilities != null && Abilities.Area.canCast(this);

    /*Método: UseArea
    *Descripción: Ejecuta la habilidad de daño en área sobre un objetivo.
    *Parámetros:
    *   - target: objetivo afectado por la habilidad de área.
    */
    public void UseArea(Carrier target)
    {
        if (Abilities == null) return;
        Abilities.Area.Cast(this, target);
    }
}
