/*Clase: Ability
*Descripción: Clase abstracta que representa una habilidad genérica dentro del juego.
* Define propiedades comunes como nombre, descripción, enfriamiento, tipo de recurso y costo.
* Además, maneja el estado de enfriamiento (cooldown) y provee validaciones para su uso.
*Atributos:
*   - Name: nombre de la habilidad.
*   - Description: descripción de la habilidad.
*   - Cd: tiempo de enfriamiento de la habilidad en segundos.
*   - ResourceType: tipo de recurso requerido para castear la habilidad (ej. Mana, Vida).
*   - CostValue: costo en recursos necesario para usar la habilidad.
*   - OnCd: indica si la habilidad está actualmente en enfriamiento.
*/
public abstract class Ability
{
    protected string Name { get; }
    protected string Description { get; }
    protected float Cd { get; }
    protected ResourceType ResourceType { get; }
    protected float CostValue { get; }
    protected bool OnCd { get; set; } = false;

    /*Constructor: Ability
    *Descripción: Inicializa una habilidad con nombre, descripción, enfriamiento,
    * tipo de recurso y costo.
    *Parámetros:
    *   - name: nombre de la habilidad.
    *   - description: descripción de la habilidad.
    *   - cd: tiempo de enfriamiento en segundos.
    *   - resourceType: tipo de recurso necesario (Mana o Vida).
    *   - costValue: valor del recurso consumido al castear.
    */
    protected Ability(
        string name,
        string description,
        float cd,
        ResourceType resourceType,
        float costValue
    )
    {
        Name = name;
        Description = description;
        Cd = cd;
        ResourceType = resourceType;
        CostValue = costValue;
    }

    /*Método: Cast
    *Descripción: Método abstracto que define cómo se ejecuta la habilidad.
    *Debe ser implementado por cada clase derivada.
    *Parámetros:
    *   - caster: entidad que lanza la habilidad.
    *   - target: objetivo de la habilidad.
    */
    public abstract void Cast(PlayableCarrier caster, Carrier target);

    /*Método: startCooldown
    *Descripción: Inicia el enfriamiento de la habilidad y ejecuta una corrutina
    * para esperar el tiempo definido en Cd antes de habilitarla nuevamente.
    */
    public void startCooldown()
    {
        OnCd = true;

        UnityEngine.MonoBehaviour runner =
            UnityEngine.Object.FindObjectOfType<UnityEngine.MonoBehaviour>();
        if (runner != null)
        {
            runner.StartCoroutine(CooldownCoroutine());
        }
        else
        {
            UnityEngine.Debug.LogWarning("No MonoBehaviour found to run the cooldown coroutine.");
        }
    }

    /*Método privado: CooldownCoroutine
    *Descripción: Corrutina que espera el tiempo de enfriamiento (Cd)
    * y después restablece la disponibilidad de la habilidad.
    */
    private System.Collections.IEnumerator CooldownCoroutine()
    {
        yield return new UnityEngine.WaitForSeconds(Cd);
        OnCd = false;
    }

    /*Método: canCast
    *Descripción: Verifica si la habilidad puede lanzarse. Revisa que no esté en enfriamiento
    * y que el lanzador tenga suficientes recursos (Mana o Vida).
    *Parámetros:
    *   - caster: entidad que intenta lanzar la habilidad.
    *Retorna: true si la habilidad puede lanzarse, false en caso contrario.
    */
    public bool canCast(PlayableCarrier caster)
    {
        if (OnCd == false)
        {
            if (ResourceType == ResourceType.Mana)
                return caster.Mana.Current >= CostValue;
            else if (ResourceType == ResourceType.Health)
                return caster.Life.Current > CostValue;
            return true;
        }
        else
            return false;
    }
}
