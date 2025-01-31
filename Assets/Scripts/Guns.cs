using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{
    private Vector3 mousePos;

    [Header("References")]
    public GameObject cross;
    public GameObject bullet;
    public Transform firePoint;

    [Header("Shooting Settings")]
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    [Header("Audio")]
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip shootingSound; // The shooting sound clip

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)
        );

        cross.transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);

        Vector3 targetDirection = mousePos - transform.position;
        float rotateZ = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotateZ);

        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(bullet, firePoint.position, transform.rotation);

        // Play the shooting sound
        if (audioSource != null && shootingSound != null)
        {
            audioSource.PlayOneShot(shootingSound);
        }
    }
}


