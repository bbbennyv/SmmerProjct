using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RangedWeapon : BaseWeapon
{
    [SerializeField]
    protected GameObject projectilePrefab;

    [SerializeField]
    private float recoil = 5f;

    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float reloadSpeed = 1;

    private int currentAmmo;
    private bool reloading = false;

    protected float projectileSpeedMult = 1f;

    protected Transform ownerTransform;

    [SerializeField]
    protected TextMeshProUGUI ammoText;

    private void Start()
    {
        PlayerController owner = GetComponentInParent<PlayerController>();

        ownerTransform = owner.GetComponent<Transform>();

        currentAmmo = maxAmmo;
        UpdateAmmoUI(currentAmmo, maxAmmo);
    }

    public override void Update()
    {
        base.Update();

        if(currentAmmo <= 0 && !reloading)
        {
            StartCoroutine(Reload());
        }
    }

    public override void Use()
    {
        if(!CanUse()) return;
        if (reloading) return;

        FireProjectile();
        ApplyRecoil();

        ResetCooldown();
    }


    public virtual void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Vector2 direction = GetFireDirection();

        float knockback = Mathf.Lerp(weaponData.minKnockback, weaponData.maxKnockback, chargeRatio);
        int damage = (int)Mathf.Lerp(weaponData.minDamage, weaponData.maxDamage, chargeRatio);
        projectile.GetComponent<Projectile>().InitializeProjectile(direction, knockback, damage, chargeRatio, ownerTransform, projectileSpeedMult);

        currentAmmo--;
        UpdateAmmoUI(currentAmmo, maxAmmo);

    }

    private IEnumerator Reload()
    {
        reloading = true;

        Quaternion startRotation = transform.localRotation;

        float elapsed = 0f;

        while (elapsed < reloadSpeed)
        {
            elapsed += Time.deltaTime;

            float angle = 720f * elapsed;

            transform.localRotation = startRotation * Quaternion.Euler(0f, 0f, angle);

            yield return null;
        }

        transform.localRotation = startRotation;

        currentAmmo = maxAmmo;

        UpdateAmmoUI(currentAmmo, maxAmmo);

        reloading = false;
    }

    protected virtual Vector2 GetFireDirection()
    {
        return hitDirection.normalized;
    }

    protected virtual void ApplyRecoil()
    {
        Rigidbody2D rb = ownerTransform.gameObject.GetComponent<Rigidbody2D>();

        if(rb != null)
        {
            Vector2 recoilDirection = -hitDirection.normalized;

            rb.AddForce(recoilDirection * recoil, ForceMode2D.Impulse);
        }
    }

    protected void UpdateAmmoUI(int currentAmmo, int MaxAmmo)
    {
        ammoText.text = currentAmmo + "/" + MaxAmmo; 
    }

    public override bool isRanged()
    {
        return true;
    }
}
