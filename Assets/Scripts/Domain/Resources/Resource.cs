/*Clase: Resource
*Descripción: Clase abstracta que representa un recurso genérico (vida o maná) 
* en un sistema de juego. Define propiedades y métodos comunes para manejar la cantidad actual, 
* el máximo permitido y operaciones de recuperación o gasto del recurso.
*Atributos:
*   - Current: cantidad actual del recurso.
*   - Max: cantidad máxima que puede alcanzar el recurso.
*/
public abstract class Resource
{
    /*Constructor: Resource
    *Descripción: Inicializa el recurso con una cantidad actual (Current) y un máximo (Max).
    *Parámetros:
    *   - Current: valor inicial del recurso.
    *   - Max: valor máximo del recurso.
    */
    public float Current { get; protected set; }
    public float Max { get; protected set; }
    protected Resource(float Current, float Max)
    {
        this.Current = Current;
        this.Max = Max;
    }

    /*Método: Recover
    *Descripción: Recupera (suma) una cantidad al recurso actual sin superar el máximo.
    *Parámetros:
    *   - amount: cantidad a recuperar.
    */
    public virtual void Recover(float amount)
    {
        if (amount <= 0) return;
        Current += amount;
        if (Current > Max) Current = Max;
    }

    /*Método: Spend
    *Descripción: Gasta (resta) una cantidad del recurso actual si hay suficiente disponible.
    *Parámetros:
    *   - amount: cantidad a gastar.
    */
    public virtual void Spend(float amount)
    {
        if (amount <= 0 || amount > Current) return;
        Current -= amount;
        if (Current < 0) Current = 0;
    }

    /*Método: IsEmpty
    *Descripción: Verifica si el recurso está vacío.
    *Retorna: true si Current es menor o igual a 0, de lo contrario false.
    */
    public virtual bool IsEmpty() => Current <= 0;

    /*Método: CurrentPercent
    *Descripción: Calcula el porcentaje del recurso actual en relación al máximo.
    *Retorna: valor flotante entre 0 y 1 representando el porcentaje.
    */
    public virtual float CurrentPercent() => (float)Current / Max;
}
