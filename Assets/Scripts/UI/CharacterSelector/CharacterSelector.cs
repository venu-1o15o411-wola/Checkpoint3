using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*Clase: CharacterSelector
*Descripción: Permite al jugador navegar entre los personajes disponibles,
* visualizar su información y seleccionar uno para jugar. La selección se guarda
* en PlayerPrefs y luego se carga la escena correspondiente.
*Atributos:
*   - characters: lista de personajes disponibles (ScriptableObjects).
*   - characterImage: imagen en la UI que muestra el sprite del personaje.
*   - nameText: texto con el nombre del personaje.
*   - descriptionText: texto con la descripción del personaje.
*   - leftButton / rightButton: botones para navegar entre personajes.
*   - selectButton: botón para confirmar la selección del personaje.
*   - index: índice actual del personaje seleccionado.
*/
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

    /*Método: Start
    *Descripción: Verifica la lista de personajes, configura los listeners de los botones
    * y actualiza la UI con la información del personaje inicial.
    */
    void Start()
    {
        if (leftButton != null)
            leftButton.onClick.AddListener(Previous);
        if (rightButton != null)
            rightButton.onClick.AddListener(Next);
        if (selectButton != null)
            selectButton.onClick.AddListener(Select);

        UpdateUI();
    }

    /*Método: Previous
    *Descripción: Cambia al personaje anterior en la lista y actualiza la UI.
    */
    public void Previous()
    {
        index = (index - 1 + characters.Length) % characters.Length;
        UpdateUI();
    }

    /*Método: Next
    *Descripción: Cambia al siguiente personaje en la lista y actualiza la UI.
    */
    public void Next()
    {
        index = (index + 1) % characters.Length;
        UpdateUI();
    }

    /*Método: Select
    *Descripción: Guarda el personaje seleccionado en PlayerPrefs y carga la escena de juego.
    */
    public void Select()
    {
        PlayerPrefs.SetString("SelectedCharacter", characters[index].name);
        PlayerPrefs.Save();

        Debug.Log($"Character selected: {characters[index]} ");
        SceneManager.LoadScene("Sanbox");
    }

    /*Método: UpdateUI
    *Descripción: Actualiza los elementos de la UI (imagen, nombre y descripción)
    * según el personaje actualmente seleccionado.
    */
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
