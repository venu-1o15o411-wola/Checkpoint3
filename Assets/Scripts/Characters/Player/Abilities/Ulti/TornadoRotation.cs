using UnityEngine;

/*Clase: TornadoRotation
*Descripción: Controla la rotación continua de un objeto en el eje Y,
* simulando un efecto de torbellino o tornado.
*Atributos:
*   - speed: velocidad de rotación en grados por segundo.
*/
public class TornadoRotation : MonoBehaviour
{
    [SerializeField] private float speed = 100f;

    /*Método: Update
    *Descripción: Aplica la rotación en el eje Y cada frame en función de la velocidad.
    */
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
