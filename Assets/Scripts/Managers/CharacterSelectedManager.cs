using UnityEngine;
using UnityEngine.UI;

/*Clase: CharacterSelectedManager
*Descripción: Gestiona la selección, aparición (spawn) del personaje elegido y la
* configuración de la UI asociada (iconos de habilidades y barras de vida/maná).
*Atributos:
*   - characters: lista de personajes disponibles para selección.
*   - spawnPoint: punto de aparición del personaje seleccionado.
*   - attackImage / healImage / areaImage: imágenes para iconos de habilidades.
*   - lifeBarImage / manaBarImage: imágenes de barras de vida y maná.
*/
public class CharacterSelectedManager : MonoBehaviour
{
    [Header("Available characters")]
    public CharacterData[] characters;

    [Header("Spawn")]
    public Transform spawnPoint;

    [Header("Ability UI")]
    public Image attackImage;
    public Image healImage;
    public Image areaImage;

    [Header("Bars")]
    public Image lifeBarImage;
    public Image manaBarImage;

    /*Método: Start
    *Descripción: Carga el personaje seleccionado desde PlayerPrefs, lo instancia en la escena,
    * configura el pool de proyectiles según la habilidad de ataque y asigna los iconos y barras en la UI.
    */
    void Start()
    {
        string selectedCharacterName = PlayerPrefs.GetString("SelectedCharacter");

        CharacterData selectedData = null;
        GameObject selectedCharacterPrefab = null;
        foreach (var characterData in characters)
        {
            if (characterData != null && characterData.name == selectedCharacterName)
            {
                selectedData = characterData;
                selectedCharacterPrefab = characterData.prefab;
                break;
            }
        }

        if (selectedData == null || selectedCharacterPrefab == null)
        {
            Debug.LogError("Character not found in list!");
            return;
        }

        GameObject parent = GameObject.Find("PLAYER-----------");
        GameObject instance = Instantiate(
            selectedCharacterPrefab,
            spawnPoint != null ? spawnPoint.position : Vector3.zero,
            Quaternion.identity,
            parent != null ? parent.transform : null
        );
        instance.name = "Player";
        var shooter = instance.GetComponent<Shooter>();
        if (shooter != null)
        {
            var attack = selectedData.abilitySet?.attack;
            if (attack != null && attack.projectilePrefab != null && shooter.pool != null)
            {
                shooter.pool.SetPrefab(attack.projectilePrefab);
            }
        }
        if (selectedData.abilitySet != null)
        {
            SetIcon(attackImage, selectedData.abilitySet.attack ? selectedData.abilitySet.attack.icon : null, 64, 64);
            SetIcon(healImage, selectedData.abilitySet.heal ? selectedData.abilitySet.heal.icon : null, 64, 64);
            SetIcon(areaImage, selectedData.abilitySet.area ? selectedData.abilitySet.area.icon : null, 64, 64);
        }
        else
        {
            SetIcon(attackImage, null);
            SetIcon(healImage, null);
            SetIcon(areaImage, null);
        }

        SetupBarsFromData(selectedData);

        Debug.Log($"Spawned {selectedCharacterName}, ability icons & bars set.");
    }

    /*Método: SetupBarsFromData
    *Descripción: Configura los sprites de las barras de vida y maná a partir de los datos del personaje.
    *Parámetros:
    *   - data: datos del personaje seleccionados.
    */
    private void SetupBarsFromData(CharacterData data)
    {
        lifeBarImage.sprite = data.LifeBarSprite;
        manaBarImage.sprite = data.ManaBarSprite;
    }

    /*Método: SetIcon
    *Descripción: Asigna el sprite a una imagen de UI y ajusta su tamaño y aspecto.
    *Parámetros:
    *   - img: componente Image a configurar.
    *   - sprite: sprite a asignar como icono.
    *   - width: ancho deseado del rectángulo (por defecto 64).
    *   - height: alto deseado del rectángulo (por defecto 64).
    *   - forceSquare: si es true, no preserva aspecto y fuerza tamaño cuadrado.
    */
    private void SetIcon(Image img, Sprite sprite, float width = 64f, float height = 64f, bool forceSquare = true)
    {
        if (!img) return;

        img.sprite = sprite;
        img.enabled = sprite != null;

        var rt = img.rectTransform;
        if (forceSquare) img.preserveAspect = false;
        else img.preserveAspect = true;
        rt.sizeDelta = new Vector2(width, height);
    }
}
