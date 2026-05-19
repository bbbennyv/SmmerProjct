using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] private int maxHealth;
     private int currentHealth;

    private bool isDead = false;

    [SerializeField] private ParticleSystem PunchParticles;
    [SerializeField] private ParticleSystem PunchParticlesCharged;

    private PlayerController player;
    public System.Action<int, int> OnHealthChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = gameObject.GetComponent<PlayerController>();
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

    public void TakeDamage(int amount, float knockback, Vector2 hitDir, Rigidbody2D targetRb, float charge)
    {
        if (isDead) return;

        targetRb.AddForce(hitDir * knockback, ForceMode2D.Impulse);

        if (charge < 0.5)
        {
            Instantiate(PunchParticles, targetRb.position, Quaternion.identity);
            CameraShake.Instance.ShakeCamera(2f, .1f);
        }
        else
        {
            Instantiate(PunchParticlesCharged, targetRb.position, Quaternion.identity);
            CameraShake.Instance.ShakeCamera(3f, .1f);

        }

        currentHealth = Mathf.Max(0, currentHealth - amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
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

    public void Respawn(Vector2 pos)
    {
        isDead = false;
        currentHealth = maxHealth;

        transform.position = pos;
        gameObject.SetActive(true);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
