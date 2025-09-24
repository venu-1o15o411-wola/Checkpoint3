using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*Clase: PlayerUIManager
*Descripción: Administra la interfaz de usuario del jugador, incluyendo los iconos de habilidades con
* sus cooldowns y las barras de vida y maná. Implementa un patrón singleton para acceso global.
*Atributos:
*   - Instance: instancia única del gestor de UI.
*   - attackIcon / healIcon / ultiIcon: imágenes de UI para los iconos de habilidades.
*   - lifeSlider / manaSlider: imágenes que representan las barras de vida y maná.
*/
public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance { get; private set; }

    [Header("Ability Icons")]
    [SerializeField]
    private Image attackIcon;
    [SerializeField]
    private Image healIcon;
    [SerializeField]
    private Image ultiIcon;
    [Header("Life / Mana Bars")]
    [SerializeField]
    private Image lifeSlider;
    [SerializeField]
    private Image manaSlider;

    /*Método: Awake
    *Descripción: Inicializa el singleton. Si ya existe una instancia diferente, destruye este objeto.
    */
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /*Método: StartCoolDown
    *Descripción: Inicia la animación visual de cooldown para el icono de habilidad indicado.
    *Parámetros:
    *   - type: tipo de habilidad (1 = attack, 2 = heal, 3 = ulti).
    *   - cd: duración del cooldown en segundos.
    */
    public void StartCoolDown(int type, float cd)
    {
        Image target = null;
        Color baseColor = Color.white;

        switch (type)
        {
            case 1:
                target = attackIcon;
                break;
            case 2:
                target = healIcon;
                break;
            case 3:
                target = ultiIcon;
                break;
        }

        if (target != null)
        {
            StartCoroutine(CooldownRoutine(target, baseColor, cd));
        }
    }

    /*Método: CooldownRoutine
    *Descripción: Corrutina que cambia temporalmente el color del icono para indicar cooldown
    * y lo restaura al finalizar el tiempo indicado.
    *Parámetros:
    *   - icon: imagen del icono a modificar.
    *   - baseColor: color original del icono para restaurar al final.
    *   - cd: duración del cooldown en segundos.
    */
    private IEnumerator CooldownRoutine(Image icon, Color baseColor, float cd)
    {
        icon.color = Color.green;
        yield return new WaitForSeconds(cd);
        icon.color = baseColor;
    }

    /*Método: UpdateLife
    *Descripción: Actualiza visualmente la barra de vida escalando su RectTransform según el porcentaje.
    *Parámetros:
    *   - current: valor actual de vida.
    *   - max: valor máximo de vida.
    */
    public void UpdateLife(float current, float max)
    {
        if (lifeSlider != null)
        {
            float clamped = Mathf.Clamp(current, 0f, max);
            float pct = clamped / max;
            var rt = lifeSlider.rectTransform;
            rt.localScale = new Vector3(pct, 1f, 1f);
        }
    }

    /*Método: UpdateMana
    *Descripción: Actualiza visualmente la barra de maná escalando su RectTransform según el porcentaje.
    *Parámetros:
    *   - current: valor actual de maná.
    *   - max: valor máximo de maná.
    */
    public void UpdateMana(float current, float max)
    {
        if (manaSlider != null)
        {
            float clamped = Mathf.Clamp(current, 0f, max);
            float pct = clamped / max;
            var rt = manaSlider.rectTransform;
            rt.localScale = new Vector3(pct, 1f, 1f);
        }
    }
}
