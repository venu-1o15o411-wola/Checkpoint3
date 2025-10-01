using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;
/*Clase: PlayerAbilities
*Descripción: Controla las habilidades del jugador y su interacción con los recursos de vida y maná.
* Gestiona ataques, curación, habilidad definitiva (ulti), regeneración de maná y actualización de la UI.
*Atributos:
*   - characterData: datos del personaje (parámetros base y set de habilidades).
*   - currentLife / maxLife: vida actual y máxima del jugador.
*   - currentMana / maxMana: maná actual y máximo del jugador.
*   - canUseAttack / canUseHeal / canUseUlti: flags para disponibilidad de habilidades.
*   - enableManaRegen: indica si el maná se regenera automáticamente.
*   - anim: referencia al Animator del jugador.
*   - rb: referencia al Rigidbody del jugador.
*   - shooter: referencia al componente Shooter para disparar proyectiles.
*/
public class PlayerAbilities : MonoBehaviour, IDamageable
{
    [Header("Character characterData")]
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

    [Header("Audio")]
    [SerializeField] private AudioClip selectedAttack;
    [SerializeField] private AudioClip selectedHeal;
    [SerializeField] private AudioClip selectedUlti;

    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip getHit;

    [Header("Regen")]
    [SerializeField]
    private bool enableManaRegen = true;

    private Animator anim;
    private Rigidbody rb;
    private Shooter shooter;
    [Header("Heal")]
    [SerializeField] private ParticleSystem healEffect;
    [Header("Ulti")]
    [SerializeField] private UltiMoveBetween ultiMoveBetween;
    [SerializeField] private Transform muzzle;

    /*Método: Start
    *Descripción: Inicializa valores de vida/maná y referencias a componentes.
    * También envía los valores iniciales a la UI.
    */
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        shooter = GetComponent<Shooter>();

        maxLife = characterData.lifeMax;
        maxMana = characterData.manaMax;
        currentLife = maxLife;
        currentMana = maxMana;
        muzzle = shooter.muzzle;

        PlayerUIManager.Instance?.UpdateLife(currentLife, maxLife);
        PlayerUIManager.Instance?.UpdateMana(currentMana, maxMana);
    }

    /*Método: Update
    *Descripción: Aplica regeneración de maná si está habilitada y hay capacidad restante.
    */
    void Update()
    {
        if (enableManaRegen && characterData.manaRegenRate > 0f && currentMana < maxMana)
        {
            SetMana(currentMana + characterData.manaRegenRate * Time.deltaTime);
        }
    }

    /*Método: OnAttack
    *Descripción: Intenta ejecutar la habilidad de ataque cuando la acción de entrada es realizada.
    *Parámetros:
    *   - context: contexto de la acción del Input System.
    */
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || !canUseAttack)
            return;

        var ability = characterData?.abilitySet?.attack;
        if (ability == null)
            return;

        if (!TryPayCost(ability))
        {
            Debug.LogWarning("[PlayerAbilities] No alcanza recurso para ATTACK.");
            return;
        }
        AudioManager.Instance.PlaySFX(selectedAttack);
        anim?.SetTrigger("Attack");
        canUseAttack = false;
        StartCooldown(1, ability.cooldown);
        PlayerUIManager.Instance?.StartCoolDown(1, ability.cooldown);

        if (shooter != null)
        {
            shooter.Fire(ability.damage);
        }
    }

    /*Método: OnHeal
    *Descripción: Intenta ejecutar la habilidad de curación cuando la acción de entrada es realizada.
    *Parámetros:
    *   - context: contexto de la acción del Input System.
    */
    public void OnHeal(InputAction.CallbackContext context)
    {
        if (!context.performed || !canUseHeal)
            return;
        var ability = characterData?.abilitySet?.heal;
        AudioManager.Instance.PlaySFX(selectedHeal);
        anim?.SetTrigger("Heal");
        canUseHeal = false;
        StartCooldown(2, ability.cooldown);
        PlayerUIManager.Instance?.StartCoolDown(2, ability.cooldown);
        if (ability.isHeal && ability.damage > 0f)
        {
            SetLife(currentLife + ability.damage);
            healEffect?.Play();
        }
    }

    /*Método: OnUlti
    *Descripción: Intenta ejecutar la habilidad definitiva (área) cuando la acción de entrada es realizada.
    *Parámetros:
    *   - context: contexto de la acción del Input System.
    */
    public void OnUlti(InputAction.CallbackContext context)
    {
        if (!context.performed || !canUseUlti)
            return;
        var ability = characterData?.abilitySet?.area;
        if (ability == null)
            return;

        if (!TryPayCost(ability))
        {
            Debug.LogWarning("[PlayerAbilities] No alcanza recurso para ULTI.");
            return;
        }
        AudioManager.Instance.PlaySFX(selectedUlti);
        anim?.SetTrigger("Ulti");
        canUseUlti = false;
        StartCooldown(3, ability.cooldown);
        PlayerUIManager.Instance?.StartCoolDown(3, ability.cooldown);
        Vector3 basePos = muzzle.position + muzzle.forward * distanciaN;
        ultiMoveBetween.ActivateAtBasePosition(gameObject, basePos, ability.damage, gameObject.tag);
    }
    /*Método: SetLife
    *Descripción: Ajusta la vida actual dentro de los límites válidos y actualiza la UI.
    *Parámetros:
    *   - value: nuevo valor propuesto de vida.
    */
    public void SetLife(float value)
    {
        currentLife = Mathf.Clamp(value, 0f, maxLife);
        PlayerUIManager.Instance?.UpdateLife(currentLife, maxLife);
        if (currentLife <= 0f) Die();
    }
    /*Método: SetMana
    *Descripción: Ajusta el maná actual dentro de los límites válidos y actualiza la UI.
    *Parámetros:
    *   - value: nuevo valor propuesto de maná.
    */
    public void SetMana(float value)
    {
        currentMana = Mathf.Clamp(value, 0f, maxMana);
        PlayerUIManager.Instance?.UpdateMana(currentMana, maxMana);
    }

    /*Método: TryPayCost
    *Descripción: Intenta pagar el costo de la habilidad según su tipo de recurso.
    *Parámetros:
    *   - ability: habilidad cuyo costo se intenta pagar.
    *Retorna: true si el costo se pudo pagar; false si el recurso es insuficiente.
    */
    private bool TryPayCost(AbilityData ability)
    {
        float cost = Mathf.Max(0f, ability.costValue);

        switch (ability.resourceType)
        {
            case ResourceType.Mana:
                if (currentMana < cost)
                    return false;
                SetMana(currentMana - cost);
                return true;
            case ResourceType.Health:
                if (currentLife <= cost)
                    return false;
                SetLife(currentLife - cost);
                return true;

            default:
                return true;
        }
    }

    /*Método: StartCooldown
    *Descripción: Inicia la corrutina de enfriamiento para un tipo de habilidad.
    *Parámetros:
    *   - type: 1 = ataque, 2 = curación, 3 = ulti.
    *   - cd: duración del enfriamiento en segundos.
    */
    public void StartCooldown(int type, float cd)
    {
        StartCoroutine(CooldownRoutine(type, cd));
    }

    /*Método: CooldownRoutine
    *Descripción: Espera el tiempo de enfriamiento y vuelve a habilitar la habilidad correspondiente.
    *Parámetros:
    *   - type: identificador del tipo de habilidad (1/2/3).
    *   - cd: duración del enfriamiento en segundos.
    */
    private IEnumerator CooldownRoutine(int type, float cd)
    {
        yield return new WaitForSeconds(cd);
        switch (type)
        {
            case 1:
                canUseAttack = true;
                break;
            case 2:
                canUseHeal = true;
                break;
            case 3:
                canUseUlti = true;
                break;
        }
    }

    /*Método: ReceiveDamage
    *Descripción: Aplica daño al jugador, reproduce feedback de impacto si sigue con vida
    * y, si la vida llega a 0, ejecuta la lógica de muerte.
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
    *Descripción: Maneja la muerte del jugador: reproduce audio y animación,
    * destruye el objeto y regresa al menú principal.
    */
    private void Die()
    {
        AudioManager.Instance.PlaySFX(die);
        anim?.SetTrigger("Die");
        Destroy(gameObject, 5f);
        SceneManager.LoadScene("Menu");
    }

}
