/*Clase: AbilitySet
*Descripción: Representa un conjunto de habilidades que un portador o entidad puede utilizar.
* Agrupa diferentes tipos de habilidades ofensivas y de soporte en una sola estructura.
*Atributos:
*   - Attack: habilidad de proyectil usada para ataques directos.
*   - Heal: habilidad de curación usada para restaurar vida.
*   - Area: habilidad de daño en área que afecta múltiples objetivos.
*/
public class AbilitySet
{
    public ProjectileAbility Attack { get; }
    public HealAbility Heal { get; }
    public AreaDamageAbility Area { get; }

    /*Constructor: AbilitySet
    *Descripción: Inicializa un conjunto de habilidades con ataque, curación y daño en área.
    *Parámetros:
    *   - attack: habilidad de proyectil.
    *   - heal: habilidad de curación.
    *   - area: habilidad de daño en área.
    */
    public AbilitySet(ProjectileAbility attack, HealAbility heal, AreaDamageAbility area)
    {
        Attack = attack;
        Heal = heal;
        Area = area;
    }
}
