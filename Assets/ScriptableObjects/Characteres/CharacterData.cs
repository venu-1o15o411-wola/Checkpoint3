using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character Data", order = 0)]
public class CharacterData : ScriptableObject
{
    [Header("Identidad")]
    public int id;

    public string displayName;

    [TextArea] public string description;
    public Sprite sprite;
    public Sprite LifeBarSprite;
    public Sprite ManaBarSprite;
    public GameObject prefab;
    public Element element;

    public float lifeMax;

    public float manaMax;

    public float manaRegenRate;

    [Header("Habilidades")]
    public AbilitySetData abilitySet;
}
