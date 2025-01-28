using UnityEngine;

public class BossGun : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject bossBulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Shoot()
    {
        if (player == null) return;
        if (bossBulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("BossGun: bulletPrefab veya firePoint eksik!");
            return;
        }

        GameObject bullet = Instantiate(bossBulletPrefab, firePoint.position, Quaternion.identity);
        
        Vector2 direction = (player.transform.position - firePoint.position).normalized;
        
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }
}