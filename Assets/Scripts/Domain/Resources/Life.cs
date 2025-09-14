/*Clase: Life
*Descripción: Representa el recurso de vida (salud) de un personaje en el juego.
* Hereda de Resource y utiliza su funcionalidad base sin añadir nuevos atributos o métodos.
*Atributos:
*   - Hereda: Current (cantidad actual de vida).
*   - Hereda: Max (cantidad máxima de vida).
*/
public class Life : Resource
{
    /*Constructor: Life
    *Descripción: Inicializa la vida con un valor actual y un valor máximo.
    *Parámetros:
    *   - current: cantidad inicial de vida.
    *   - max: cantidad máxima de vida.
    */
    public Life(float current, float max) : base(current, max) { }
}
