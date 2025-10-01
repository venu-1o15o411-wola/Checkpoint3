using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/*Clase: MenuUI
*Descripción: Controla la lógica del menú principal, incluyendo botones de
* jugar, reiniciar y salir, además de reproducir música y efectos de sonido
* al interactuar con la interfaz.
*Atributos:
*   - playButton / exitButton / restartButton: referencias a los botones de la UI.
*   - musicClip: música de fondo del menú.
*   - hoverSfx: sonido reproducido al pasar el cursor sobre un botón.
*   - clickSfx: sonido reproducido al hacer clic en un botón.
*/
public class MenuUI : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button restartButton;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip hoverSfx;
    [SerializeField] private AudioClip clickSfx;

    /*Método: Start
    *Descripción: Configura la música de fondo y asigna listeners a los botones.
    * También agrega sonido al pasar el cursor sobre los botones.
    */
    private void Start()
    {
        if (AudioManager.Instance != null && musicClip != null)
            AudioManager.Instance?.PlayMusic(musicClip);
        if (playButton != null) playButton.onClick.AddListener(Play);
        if (exitButton != null) exitButton.onClick.AddListener(Exit);

        AddHoverSound(playButton);
        AddHoverSound(exitButton);
    }

    /*Método: Play
    *Descripción: Reproduce un efecto de sonido de clic y carga la escena de selección de jugador.
    */
    public void Play()
    {
        if (AudioManager.Instance != null && clickSfx != null)
            AudioManager.Instance.PlaySFX(clickSfx);
        SceneManager.LoadScene("SelectionPlayer");
    }

    /*Método: Restart
    *Descripción: Reproduce un efecto de sonido de clic y recarga la escena del menú.
    */
    public void Restart()
    {
        if (AudioManager.Instance != null && clickSfx != null)
            AudioManager.Instance.PlaySFX(clickSfx);
        SceneManager.LoadScene("Menu");
    }

    /*Método: Exit
    *Descripción: Reproduce un efecto de sonido de clic y cierra la aplicación.
    */
    public void Exit()
    {
        if (AudioManager.Instance != null && clickSfx != null)
            AudioManager.Instance.PlaySFX(clickSfx);
        Application.Quit();
    }

    /*Método: AddHoverSound
    *Descripción: Añade un EventTrigger a un botón para reproducir un efecto de sonido
    * al pasar el cursor sobre él.
    *Parámetros:
    *   - btn: botón al cual se le agrega el sonido de hover.
    */
    private void AddHoverSound(Button btn)
    {
        if (btn == null) return;
        var go = btn.gameObject;
        var trigger = go.GetComponent<EventTrigger>();
        if (trigger == null) trigger = go.AddComponent<EventTrigger>();

        var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entry.callback.AddListener(_ =>
        {
            if (AudioManager.Instance != null && hoverSfx != null)
                AudioManager.Instance.PlaySFX(hoverSfx);
        });
        trigger.triggers.Add(entry);
    }
}
