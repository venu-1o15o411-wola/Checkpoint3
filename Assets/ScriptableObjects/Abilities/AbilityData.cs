using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Game/Ability Data", order = 1)]
public class AbilityData : ScriptableObject
{
    [Header("Identidad")]
    public string displayName;
    public Sprite icon;
    public Projectile projectilePrefab;
    [TextArea] public string description;
    [Header("Parámetros base")]
    public float cooldown;
    public ResourceType resourceType;
    public float costValue;
    public float damage;
    [Header("Clasificación")]
    public bool attack;
    public bool isHeal;
    public bool isArea;
}
