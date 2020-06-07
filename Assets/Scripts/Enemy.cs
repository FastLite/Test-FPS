using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public int speed;
    public int damage;
    public int collisionDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("bullet"))
        {
            BulletHit();
        }
    }

    void BulletHit()
    {
        health -= GetComponent<Player>().damage;

        if(health<=0)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        GetComponent<GameManager>().enemiesAlive--;

    }
}
