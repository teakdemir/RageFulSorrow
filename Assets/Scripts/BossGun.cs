using UnityEngine;
using System.Collections;

public class BossGun : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bossBulletPrefab;  // Fırlatılacak mermi
    public Transform firePoint;          // Merminin çıkış noktası
    public float shootInterval = 1f;     // 1 saniyede bir ateş et
    public float bulletSpeed = 5f;       // Mermi hızı
    public float angleBetweenBullets = 45f; // Mermiler arasındaki açı

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            ShootPattern();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    void ShootPattern()
    {
        if (bossBulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("BossGun: bulletPrefab veya firePoint eksik!");
            return;
        }

        // Oyuncunun yönüne göre mermileri hesapla
        Vector2 directionToPlayer = (player.transform.position - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        // 3 mermiyi 45 derece açıyla ateşle
        for (int i = -1; i <= 1; i++)
        {
            float bulletAngle = baseAngle + (i * angleBetweenBullets);
            Quaternion bulletRotation = Quaternion.Euler(0, 0, bulletAngle);
            GameObject bullet = Instantiate(bossBulletPrefab, firePoint.position, bulletRotation);

            // Mermiye hız ver
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = bulletRotation * Vector2.right * bulletSpeed;
            }
        }
    }
}
