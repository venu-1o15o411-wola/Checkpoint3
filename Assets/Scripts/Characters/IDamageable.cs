/*Interfaz: IDamageable
*Descripción: Define el contrato para cualquier objeto que pueda recibir daño.
* Obliga a implementar un método para procesar la cantidad de daño recibida.
*Métodos:
*   - ReceiveDamage(float amount): aplica la cantidad de daño especificada.
*/
public interface IDamageable
{
    /*Método: ReceiveDamage
    *Descripción: Aplica una cantidad de daño al objeto.
    *Parámetros:
    *   - amount: cantidad de daño a recibir.
    */
    void ReceiveDamage(float amount);
}
