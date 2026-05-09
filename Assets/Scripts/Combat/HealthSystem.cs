using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] private int maxHealth;
     private int currentHealth;

    private bool isDead = false;

    private PlayerController player;
    public System.Action<int, int> OnHealthChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerController>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead) return;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        //Debug.Log(currentHealth);
    }

    public void Heal(int amount)
    {

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
       // Debug.Log(currentHealth);
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        player.gameObject.SetActive(false);
        GameManager.Instance.alivePlayers.Remove(player);
    }

    public void Respawn(Transform pos)
    {
        isDead = false;
        currentHealth = maxHealth;

        transform.position = pos.position;
        gameObject.SetActive(true);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
