/*Clase: HealAbility
*Descripción: Representa una habilidad de curación que restaura la vida del lanzador.
* Hereda de Ability e implementa la lógica específica para gastar recursos y recuperar vida.
*Atributos:
*   - baseHeal: cantidad base de vida que restaura la habilidad.
*/
public class HealAbility : Ability
{
    private readonly float baseHeal;

    /*Constructor: HealAbility
    *Descripción: Inicializa la habilidad de curación con sus valores de configuración.
    *Parámetros:
    *   - name: nombre de la habilidad.
    *   - description: descripción de la habilidad.
    *   - cd: tiempo de enfriamiento en segundos.
    *   - resourceType: tipo de recurso requerido (Mana o Vida).
    *   - costValue: cantidad de recurso que cuesta usar la habilidad.
    *   - baseHeal: cantidad de vida que restaura al lanzador.
    */
    public HealAbility(
        string name,
        string description,
        float cd,
        ResourceType resourceType,
        float costValue,
        float baseHeal)
        : base(name, description, cd, resourceType, costValue)
    {
        this.baseHeal = baseHeal;
    }

    /*Método: Cast
    *Descripción: Ejecuta la habilidad de curación, verificando que pueda lanzarse,
    * gastando los recursos correspondientes y aplicando la curación al lanzador.
    *Parámetros:
    *   - caster: entidad que lanza la habilidad (recibe la curación).
    *   - target: parámetro requerido por la abstracción, pero no utilizado aquí.
    */
    public override void Cast(PlayableCarrier caster, Carrier target)
    {
        if (!canCast(caster)) return;

        if (ResourceType == ResourceType.Mana)
            caster.Mana.Spend(CostValue);
        else
            caster.Life.Spend(CostValue);

        caster.Heal(baseHeal);
        startCooldown();
    }
}
