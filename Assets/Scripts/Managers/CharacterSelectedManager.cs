// CharacterSelectedManager.cs
using UnityEngine;

public class CharacterSelectedManager : MonoBehaviour
{
    [Header("Available characters (drag your SOs here)")]
    public CharacterData[] characters;

    [Header("Spawn")]
    public Transform spawnPoint;

    void Start()
    {
        string selectedCharacterName = PlayerPrefs.GetString("SelectedCharacter");
        GameObject selectedCharacterPrefab = null;

        foreach (CharacterData characterData in characters)
        {
            if (characterData.name == selectedCharacterName)
            {
                selectedCharacterPrefab = characterData.prefab;
                break;
            }
        }
        if (selectedCharacterPrefab != null)
        {
            GameObject parent = GameObject.Find("PLAYER-----------");
            GameObject instance = Instantiate(
                selectedCharacterPrefab,
                spawnPoint != null ? spawnPoint.position : Vector3.zero,
                Quaternion.identity,
                parent != null ? parent.transform : null
            );
            instance.name = "Player";
            Debug.Log($"Spawned {selectedCharacterName} under PLAYER-----------");
        }
        else
        {
            Debug.LogError("Character not found in list!");
        }
    }
}