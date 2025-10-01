/*Clase: Mana
*Descripción: Representa un recurso de maná en el juego. Hereda de Resource e incorpora
* un sistema de regeneración automática por segundo basado en una tasa de recuperación.
*Atributos:
*   - RecoveryRate: tasa de recuperación de maná expresada como un valor porcentual.
*/
public class Mana : Resource
{
    public float RecoveryRate { get; protected set; }

    /*Constructor: Mana
    *Descripción: Inicializa el recurso de maná con su valor actual, máximo y tasa de recuperación.
    *Parámetros:
    *   - current: cantidad inicial de maná.
    *   - max: cantidad máxima de maná.
    *   - recoveryRate: valor de recuperación de maná por segundo.
    */
    public Mana(float current, float max, float recoveryRate)
        : base(current, max)
    {
        RecoveryRate = recoveryRate;
    }

    /*Método: RecoveryPerSecond
    *Descripción: Recupera maná cada segundo en función de la tasa de recuperación establecida.
    *Observación: el valor recuperado es RecoveryRate * 0.01.
    */
    public void RecoveryPerSecond(float deltaTime)
    {
        if (deltaTime <= 0f) return;

        float recoveryAmount = Max * 0.02f * deltaTime; // 2% del Max por segundo
        Recover(recoveryAmount);
    }
}
