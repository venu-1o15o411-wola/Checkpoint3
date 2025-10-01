using UnityEngine;

/*Clase: AudioManager
*Descripción: Gestiona la reproducción de música y efectos de sonido en el juego.
* Implementa un patrón Singleton para acceso global y persiste entre escenas.
*Atributos:
*   - Instance: instancia única del AudioManager.
*   - musicSource: fuente de audio dedicada a música de fondo.
*   - sfxSource: fuente de audio dedicada a efectos de sonido (SFX).
*/
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    /*Método: Awake
    *Descripción: Inicializa el Singleton, evita duplicados y configura las
    * fuentes de audio para música y efectos de sonido.
    */
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    /*Método: PlayMusic
    *Descripción: Reproduce un clip de audio como música de fondo.
    *Parámetros:
    *   - clip: AudioClip a reproducir.
    */
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    /*Método: PlaySFX
    *Descripción: Reproduce un clip de audio como efecto de sonido sin interrumpir
    * la música de fondo.
    *Parámetros:
    *   - clip: AudioClip a reproducir como SFX.
    */
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    /*Método: MuteMusic
    *Descripción: Activa o desactiva el sonido de la música de fondo.
    *Parámetros:
    *   - mute: true para silenciar, false para activar.
    */
    public void MuteMusic(bool mute)
    {
        musicSource.mute = mute;
    }

    /*Método: MuteSFX
    *Descripción: Activa o desactiva el sonido de los efectos de sonido.
    *Parámetros:
    *   - mute: true para silenciar, false para activar.
    */
    public void MuteSFX(bool mute)
    {
        sfxSource.mute = mute;
    }

    /*Método: SetMusicVolume
    *Descripción: Ajusta el volumen de la música de fondo.
    *Parámetros:
    *   - v: valor entre 0 y 1.
    */
    public void SetMusicVolume(float v)
    {
        musicSource.volume = Mathf.Clamp01(v);
    }

    /*Método: SetSfxVolume
    *Descripción: Ajusta el volumen de los efectos de sonido.
    *Parámetros:
    *   - v: valor entre 0 y 1.
    */
    public void SetSfxVolume(float v)
    {
        sfxSource.volume = Mathf.Clamp01(v);
    }
}
