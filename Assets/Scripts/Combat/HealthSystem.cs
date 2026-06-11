using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] private int maxHealth;
     private int currentHealth;

    private bool isDead = false;

    [SerializeField] private ParticleSystem PunchParticles;
    [SerializeField] private ParticleSystem PunchParticlesCharged;

    [SerializeField] private float verticalKnockbackBias = 0.3f;

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

        if(targetRb != null)
        {
            Vector2 biasedDirection = ApplyKnockbackBias(hitDir);
            targetRb.AddForce(biasedDirection * knockback, ForceMode2D.Impulse);

        }


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

    private Vector2 ApplyKnockbackBias(Vector2 direction)
    {
        float x = Mathf.Sign(direction.x);
        return new Vector2(x, verticalKnockbackBias).normalized;
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

       // player.gameObject.SetActive(false);
        player.GetComponent<Renderer>().enabled = false;
        player.GetComponentInChildren<Renderer>().enabled = false;
        player.transform.position = Vector2.zero;
        player.enabled = false;
        GameManager.Instance.alivePlayers.Remove(player);
    }

    public void Respawn(Vector2 pos)
    {
        isDead = false;
        currentHealth = maxHealth;

        player.GetComponent<Renderer>().enabled = true;
        
        player.enabled = true;
        transform.position = pos;
        Debug.Log("Player respawned at: " + pos);
        gameObject.SetActive(true);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
