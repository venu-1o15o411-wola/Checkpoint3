using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [Header("Data (drag your ScriptableObjects here)")]
    public CharacterData[] characters;

    [Header("UI References")]
    public Image characterImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [Header("Buttons")]
    public Button leftButton;
    public Button rightButton;
    public Button selectButton;

    private int index = 0;

    void Start()
    {
        if (characters == null || characters.Length == 0)
        {
            Debug.LogError("No characters assigned to CharacterSelector.");
            return;
        }

        // Hook up button listeners
        if (leftButton != null)
            leftButton.onClick.AddListener(Previous);
        if (rightButton != null)
            rightButton.onClick.AddListener(Next);
        if (selectButton != null)
            selectButton.onClick.AddListener(Select);

        UpdateUI();
    }

    public void Previous()
    {
        index = (index - 1 + characters.Length) % characters.Length;
        UpdateUI();
    }

    public void Next()
    {
        index = (index + 1) % characters.Length;
        UpdateUI();
    }

    public void Select()
    {
        PlayerPrefs.SetString("SelectedCharacter", characters[index].name);
        PlayerPrefs.Save();

        Debug.Log($"Character selected: {characters[index]} ");
        SceneManager.LoadScene("Sanbox");

    }

    private void UpdateUI()
    {
        var c = characters[index];
        if (characterImage)
            characterImage.sprite = c.sprite;
        if (nameText)
            nameText.text = c.displayName;
        if (descriptionText)
            descriptionText.text = c.description;
    }
}
