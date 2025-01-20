using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy: MonoBehaviour
{
  [SerializeField] float health, maxhealth=3f;

  private void Start(){

health=maxhealth;

  }

public void TakeDamage(float damageAmount)
{

    health -= damageAmount;

    if(health <= 0)
    {
        Destroy(gameObject);
    }
}

}