using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    private void Start()
    {
        if (AudioManager.Instance != null && musicClip != null)
            AudioManager.Instance.PlayMusic(musicClip);

        if (playButton != null) playButton.onClick.AddListener(Play);
        if (exitButton != null) exitButton.onClick.AddListener(Exit);

        AddHoverSound(playButton);
        AddHoverSound(exitButton);
    }

    public void Play()
    {
        if (AudioManager.Instance != null && clickSfx != null)
            AudioManager.Instance.PlaySFX(clickSfx);
        SceneManager.LoadScene("SelectionPlayer");
    }
    public void Restart()
    {
        if (AudioManager.Instance != null && clickSfx != null)
            AudioManager.Instance.PlaySFX(clickSfx);
        SceneManager.LoadScene("Menu");
        Debug.Log("Restarting Game...");
    }
    public void Exit()
    {
        if (AudioManager.Instance != null && clickSfx != null)
            AudioManager.Instance.PlaySFX(clickSfx);

        Application.Quit();
    }

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
