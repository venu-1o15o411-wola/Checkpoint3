using UnityEngine;

public enum PotionType
{
    Health,
    Mana,
}

public class PotionPickup : MonoBehaviour, IPickup
{
    [Header("Setup")]
    [SerializeField] private PotionType type = PotionType.Health;
    [SerializeField] private float amount = 50f;
    [SerializeField] private string playerTag = "Player";
    //[Header("Audio")]
    // [SerializeField] private AudioClip pickupSfx;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            TakeIt(amount, other.gameObject);
            //  AudioManager.Instance.PlaySFX(pickupSfx);
            Destroy(gameObject);
        }
    }

    public void TakeIt(float amount, GameObject gameObject)
    {
        var abilities = gameObject.GetComponent<PlayerAbilities>();
        if (abilities == null) return;
        switch (type)
        {
            case PotionType.Health:
                Debug.Log("vida vieja: " + abilities.currentLife);
                abilities.SetLife(abilities.currentLife + amount);
                Debug.Log("vida nueva: " + abilities.currentLife);

                break;
            case PotionType.Mana:
                Debug.Log("mana vieja: " + abilities.currentMana);
                abilities.SetMana(abilities.currentMana + amount);
                Debug.Log("mana nueva: " + abilities.currentMana);

                break;
        }
    }
}