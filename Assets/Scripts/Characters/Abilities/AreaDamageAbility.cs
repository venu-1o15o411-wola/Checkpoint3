/*Clase: AreaDamageAbility
*Descripción: Representa una habilidad de daño en área que inflige daño a múltiples objetivos.
* Hereda de Ability e implementa la lógica de gasto de recursos y aplicación de daño.
*Atributos:
*   - baseDamage: cantidad base de daño que inflige la habilidad.
*/
public class AreaDamageAbility : Ability
{
    private readonly float baseDamage;

    /*Constructor: AreaDamageAbility
    *Descripción: Inicializa la habilidad de daño en área con sus valores de configuración.
    *Parámetros:
    *   - name: nombre de la habilidad.
    *   - description: descripción de la habilidad.
    *   - cd: tiempo de enfriamiento en segundos.
    *   - resourceType: tipo de recurso requerido (Mana o Vida).
    *   - costValue: cantidad de recurso que cuesta usar la habilidad.
    *   - baseDamage: cantidad base de daño infligido.
    */
    public AreaDamageAbility(
        string name,
        string description,
        float cd,
        ResourceType resourceType,
        float costValue,
        float baseDamage)
        : base(name, description, cd, resourceType, costValue)
    {
        this.baseDamage = baseDamage;
    }

    /*Método: Cast
    *Descripción: Ejecuta la habilidad de daño en área, verificando que pueda lanzarse,
    * gastando los recursos correspondientes y aplicando el daño base al objetivo.
    *Parámetros:
    *   - caster: entidad que lanza la habilidad.
    *   - target: objetivo que recibe el daño (actualmente se aplica a un único objetivo).
    */
    public override void Cast(PlayableCarrier caster, Carrier target)
    {
        if (!canCast(caster)) return;
        if (target == null) return;

        if (ResourceType == ResourceType.Mana)
            caster.Mana.Spend(CostValue);
        else
            caster.Life.Spend(CostValue);

        target.ApplyDamage(baseDamage);
        startCooldown();
    }
}
