/*Clase: ProjectileAbility
*Descripción: Representa una habilidad de proyectil que inflige daño directo a un objetivo.
* Hereda de Ability e implementa la lógica específica para gastar recursos y aplicar daño.
*Atributos:
*   - baseDamage: cantidad de daño que inflige el proyectil al objetivo.
*/
public class ProjectileAbility : Ability
{
    private readonly float baseDamage;

    /*Constructor: ProjectileAbility
    *Descripción: Inicializa la habilidad de proyectil con sus valores de configuración.
    *Parámetros:
    *   - name: nombre de la habilidad.
    *   - description: descripción de la habilidad.
    *   - cd: tiempo de enfriamiento en segundos.
    *   - resourceType: tipo de recurso requerido (Mana o Vida).
    *   - costValue: cantidad de recurso que cuesta usar la habilidad.
    *   - baseDamage: daño base infligido por el proyectil.
    */
    public ProjectileAbility(
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
    *Descripción: Ejecuta la habilidad de proyectil, verificando que pueda lanzarse,
    * gastando los recursos correspondientes y aplicando el daño base al objetivo.
    *Parámetros:
    *   - caster: entidad que lanza la habilidad.
    *   - target: objetivo que recibe el daño.
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
