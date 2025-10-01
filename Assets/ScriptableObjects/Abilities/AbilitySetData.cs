using UnityEngine;

[CreateAssetMenu(fileName = "AbilitySetData", menuName = "Game/Ability Set Data", order = 2)]
public class AbilitySetData : ScriptableObject
{
    [Header("Habilidades principales del personaje")]
    public AbilityData attack;

    [Tooltip("Habilidad de curación.")]
    public AbilityData heal;

    [Tooltip("Habilidad de ataque en área (AOE).")]
    public AbilityData area;
}
