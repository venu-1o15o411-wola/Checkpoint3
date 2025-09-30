using System.Collections;
using UnityEngine;

public class EnemyAbilities : MonoBehaviour, IDamageable
{
    [Header("EnemyData")]
    public CharacterData characterData;

    [Header("Values")]
    public float currentLife;
    public float currentMana;
    public float maxLife;
    public float maxMana;
    public float distanciaN = 5f;

    private bool canUseAttack = true;
    private bool canUseHeal = true;
    private bool canUseUlti = true;

    [Header("Regen")]
    [SerializeField] private bool enableManaRegen = true;

    private Animator anim;
    private Rigidbody rb;
    private Shooter shooter;
    [Header("Heal")]
    [SerializeField] private ParticleSystem healEffect;

    [Header("Ulti")]
    [SerializeField] private UltiMoveBetween ultiMoveBetween;
    [SerializeField] private Transform muzzle;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        shooter = GetComponent<Shooter>();
    }

    void Start()
    {
        maxLife = characterData.lifeMax;
        maxMana = characterData.manaMax;
        currentLife = maxLife;
        currentMana = maxMana;
        muzzle = shooter.muzzle;
        shooter.ownerTag = "Enemy";
    }

    void Update()
    {
        if (enableManaRegen && characterData.manaRegenRate > 0f && currentMana < maxMana)
        {
            SetMana(currentMana + characterData.manaRegenRate * Time.deltaTime);
        }
    }

    public bool TryAttack()
    {
        if (!canUseAttack) return false;
        var ability = characterData?.abilitySet?.attack;
        if (ability == null) return false;
        if (!TryPayCost(ability)) return false;
        anim?.SetTrigger("Attack");
        canUseAttack = false;
        StartCooldown(1, ability.cooldown);
        if (shooter != null) shooter.Fire(ability.damage);
        return true;
    }

    public bool TryHeal()
    {
        if (!canUseHeal) return false;
        var ability = characterData?.abilitySet?.heal;
        if (ability == null) return false;
        anim?.SetTrigger("Heal");
        canUseHeal = false;
        StartCooldown(2, ability.cooldown);
        if (ability.isHeal && ability.damage > 0f)
        {
            SetLife(currentLife + ability.damage);
            healEffect?.Play();
        }
        return true;
    }

    public bool TryUlti()
    {
        if (!canUseUlti) return false;
        var ability = characterData?.abilitySet?.area;
        if (ability == null) return false;
        if (!TryPayCost(ability)) return false;
        anim?.SetTrigger("Ulti");
        canUseUlti = false;
        StartCooldown(3, ability.cooldown);
        Transform m = muzzle != null ? muzzle : (shooter != null ? shooter.muzzle : null);
        if (m != null)
        {
            Vector3 basePos = m.position + m.forward * distanciaN;
            ultiMoveBetween?.ActivateAtBasePosition(gameObject, basePos);
        }
        return true;
    }

    public void SetLife(float value)
    {
        currentLife = Mathf.Clamp(value, 0f, maxLife);
        if (currentLife <= 0f) Die();
    }

    public void SetMana(float value)
    {
        currentMana = Mathf.Clamp(value, 0f, maxMana);
    }

    private bool TryPayCost(AbilityData ability)
    {
        float cost = Mathf.Max(0f, ability.costValue);
        switch (ability.resourceType)
        {
            case ResourceType.Mana:
                if (currentMana < cost) return false;
                SetMana(currentMana - cost);
                return true;
            case ResourceType.Health:
                if (currentLife <= cost) return false;
                SetLife(currentLife - cost);
                return true;
            default:
                return true;
        }
    }

    public void StartCooldown(int type, float cd)
    {
        StartCoroutine(CooldownRoutine(type, cd));
    }

    private IEnumerator CooldownRoutine(int type, float cd)
    {
        yield return new WaitForSeconds(cd);
        switch (type)
        {
            case 1: canUseAttack = true; break;
            case 2: canUseHeal = true; break;
            case 3: canUseUlti = true; break;
        }
    }

    public void ReceiveDamage(float amount)
    {
        if (amount <= 0f) return;
        SetLife(currentLife - amount);
        if (currentLife > 0f)
        {
            anim?.SetTrigger("GetHit");
        }
        else
        {
            anim?.SetTrigger("Die");
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject, 5f);
    }
}
