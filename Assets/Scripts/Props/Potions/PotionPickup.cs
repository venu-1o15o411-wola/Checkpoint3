using UnityEngine;

/*Enum: PotionType
*Descripción: Define los tipos de pociones disponibles.
*Valores:
*   - Health: poción que restaura vida.
*   - Mana: poción que restaura maná.
*/
public enum PotionType
{
    Health,
    Mana,
}

/*Clase: PotionPickup
*Descripción: Representa una poción en el escenario que puede ser recogida por el jugador.
* Al entrar en contacto con el jugador, restaura vida o maná según el tipo de poción y
* reproduce un efecto de sonido.
*Atributos:
*   - type: tipo de poción (vida o maná).
*   - amount: cantidad de recurso a restaurar.
*   - playerTag: etiqueta del jugador que puede recoger la poción.
*   - pickupSfx: sonido reproducido al recoger la poción.
*/
public class PotionPickup : MonoBehaviour, IPickup
{
    [Header("Setup")]
    [SerializeField] private PotionType type = PotionType.Health;
    [SerializeField] private float amount = 50f;
    [SerializeField] private string playerTag = "Player";

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSfx;

    /*Método: OnTriggerEnter
    *Descripción: Detecta la colisión con el jugador. Si coincide la etiqueta,
    * aplica el efecto de la poción y destruye el objeto en la escena.
    *Parámetros:
    *   - other: collider del objeto que entró en el trigger.
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            TakeIt(amount, other.gameObject);
            Destroy(gameObject);
        }
    }

    /*Método: TakeIt
    *Descripción: Aplica el efecto de la poción al jugador que la recoge,
    * restaurando vida o maná según corresponda, y reproduce el sonido de recogida.
    *Parámetros:
    *   - amount: cantidad de recurso a restaurar.
    *   - gameObject: referencia al objeto que recogió la poción.
    */
    public void TakeIt(float amount, GameObject gameObject)
    {
        var abilities = gameObject.GetComponent<PlayerAbilities>();
        if (abilities == null) return;

        switch (type)
        {
            case PotionType.Health:
                abilities.SetLife(abilities.currentLife + amount);
                break;
            case PotionType.Mana:
                abilities.SetMana(abilities.currentMana + amount);
                break;
        }

        AudioManager.Instance.PlaySFX(pickupSfx);
    }
}
