using System.Collections;
using UnityEngine;

/*Clase: EnemyAbilities
*Descripción: Controla las habilidades, recursos y comportamiento de un enemigo.
* Gestiona ataques, curación, habilidades definitivas, regeneración de maná,
* efectos visuales y de sonido, así como la recepción de daño y muerte.
*Atributos:
*   - characterData: datos base del enemigo (vida, maná, habilidades).
*   - currentLife / maxLife: vida actual y máxima.
*   - currentMana / maxMana: maná actual y máximo.
*   - distanciaN: distancia usada para calcular la posición base de la ulti.
*   - canUseAttack / canUseHeal / canUseUlti: controlan disponibilidad de habilidades.
*   - enableManaRegen: habilita regeneración automática de maná.
*   - anim: referencia al Animator.
*   - rb: referencia al Rigidbody.
*   - shooter: referencia al componente Shooter.
*   - attackGround / healGround / ultiGround / die / getHit: clips de audio para eventos.
*   - healEffect: partículas de curación.
*   - ultiMoveBetween: componente que ejecuta el movimiento de la ulti.
*   - muzzle: punto de salida de proyectiles.
*/
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

    [Header("Audio")]
    [SerializeField] private AudioClip attackGround;
    [SerializeField] private AudioClip healGround;
    [SerializeField] private AudioClip ultiGround;
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip getHit;

    [Header("Heal")]
    [SerializeField] private ParticleSystem healEffect;

    [Header("Ulti")]
    [SerializeField] private UltiMoveBetween ultiMoveBetween;
    [SerializeField] private Transform muzzle;

    /*Método: Awake
    *Descripción: Inicializa referencias a componentes necesarios del enemigo.
    */
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        shooter = GetComponent<Shooter>();
    }

    /*Método: Start
    *Descripción: Inicializa valores de vida, maná y componentes asociados al enemigo.
    */
    void Start()
    {
        maxLife = characterData.lifeMax;
        maxMana = characterData.manaMax;
        currentLife = maxLife;
        currentMana = maxMana;
        muzzle = shooter.muzzle;
        shooter.ownerTag = "Enemy";
    }

    /*Método: Update
    *Descripción: Gestiona regeneración de maná si está habilitada y es necesaria.
    */
    void Update()
    {
        if (enableManaRegen && characterData.manaRegenRate > 0f && currentMana < maxMana)
        {
            SetMana(currentMana + characterData.manaRegenRate * Time.deltaTime);
        }
    }

    /*Método: TryAttack
    *Descripción: Intenta ejecutar la habilidad de ataque del enemigo.
    *Retorna: true si se pudo realizar el ataque, false en caso contrario.
    */
    public bool TryAttack()
    {
        if (!canUseAttack) return false;
        var ability = characterData?.abilitySet?.attack;
        if (ability == null) return false;
        if (!TryPayCost(ability)) return false;
        AudioManager.Instance.PlaySFX(attackGround);
        anim?.SetTrigger("Attack");
        canUseAttack = false;
        StartCooldown(1, ability.cooldown);
        if (shooter != null) shooter.Fire(ability.damage);
        return true;
    }

    /*Método: TryHeal
    *Descripción: Intenta ejecutar la habilidad de curación del enemigo.
    *Retorna: true si se pudo curar, false en caso contrario.
    */
    public bool TryHeal()
    {
        if (!canUseHeal) return false;
        var ability = characterData?.abilitySet?.heal;
        if (ability == null) return false;
        AudioManager.Instance.PlaySFX(healGround);

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

    /*Método: TryUlti
    *Descripción: Intenta ejecutar la habilidad definitiva (ulti) del enemigo.
    *Retorna: true si se pudo ejecutar, false en caso contrario.
    */
    public bool TryUlti()
    {
        if (!canUseUlti) return false;
        var ability = characterData?.abilitySet?.area;
        if (ability == null) return false;
        if (!TryPayCost(ability)) return false;
        AudioManager.Instance.PlaySFX(ultiGround);
        anim?.SetTrigger("Ulti");
        canUseUlti = false;
        StartCooldown(3, ability.cooldown);
        Transform m = muzzle != null ? muzzle : (shooter != null ? shooter.muzzle : null);
        if (m != null)
        {
            Vector3 basePos = m.position + m.forward * distanciaN;
            ultiMoveBetween?.ActivateAtBasePosition(gameObject, basePos, ability.damage, gameObject.tag);
        }
        return true;
    }

    /*Método: SetLife
    *Descripción: Ajusta la vida del enemigo y ejecuta la lógica de muerte si llega a 0.
    *Parámetros:
    *   - value: nuevo valor de vida.
    */
    public void SetLife(float value)
    {
        currentLife = Mathf.Clamp(value, 0f, maxLife);
        if (currentLife <= 0f) Die();
    }

    /*Método: SetMana
    *Descripción: Ajusta el maná actual del enemigo dentro de los límites válidos.
    *Parámetros:
    *   - value: nuevo valor de maná.
    */
    public void SetMana(float value)
    {
        currentMana = Mathf.Clamp(value, 0f, maxMana);
    }

    /*Método: TryPayCost
    *Descripción: Intenta pagar el costo de una habilidad en función de su tipo de recurso.
    *Parámetros:
    *   - ability: habilidad cuyo costo se debe pagar.
    *Retorna: true si el costo se pagó correctamente, false en caso contrario.
    */
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

    /*Método: StartCooldown
    *Descripción: Inicia la corrutina que controla el enfriamiento de habilidades.
    *Parámetros:
    *   - type: identificador de habilidad (1=Attack, 2=Heal, 3=Ulti).
    *   - cd: duración del cooldown en segundos.
    */
    public void StartCooldown(int type, float cd)
    {
        StartCoroutine(CooldownRoutine(type, cd));
    }

    /*Método: CooldownRoutine
    *Descripción: Corrutina que espera el tiempo del cooldown y habilita nuevamente la habilidad.
    *Parámetros:
    *   - type: identificador de habilidad.
    *   - cd: duración del cooldown en segundos.
    */
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

    /*Método: ReceiveDamage
    *Descripción: Aplica daño al enemigo y ejecuta animaciones/sonido de golpe o muerte.
    *Parámetros:
    *   - amount: cantidad de daño recibido.
    */
    public void ReceiveDamage(float amount)
    {
        if (amount <= 0f) return;
        SetLife(currentLife - amount);
        if (currentLife > 0f)
        {
            AudioManager.Instance.PlaySFX(getHit);
            anim?.SetTrigger("GetHit");
        }
        else
        {
            Die();
        }
    }

    /*Método: Die
    *Descripción: Ejecuta la lógica de muerte del enemigo: sonido, animación y destrucción del objeto.
    */
    private void Die()
    {
        AudioManager.Instance.PlaySFX(die);
        anim?.SetTrigger("Die");
        Destroy(gameObject, 5f);
    }
}
