using UnityEngine;

/*Interfaz: IPickup
*Descripción: Define el contrato para objetos que pueden ser recogidos en el juego
* (ej. pociones, cofres, ítems). Obliga a implementar la lógica al ser tomados por un GameObject.
*Métodos:
*   - TakeIt(float amount, GameObject gameObject): ejecuta la acción de recoger el objeto.
*/
public interface IPickup
{
    /*Método: TakeIt
    *Descripción: Aplica el efecto del objeto recogido por el jugador u otra entidad.
    *Parámetros:
    *   - amount: valor asociado al pickup (ej. cantidad de curación o maná).
    *   - gameObject: referencia al GameObject que recogió el objeto.
    */
    void TakeIt(float amount, GameObject gameObject);
}
