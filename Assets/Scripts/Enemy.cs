using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public int speed;
    public int damage;
    public int collisionDamage;
    Player pl;
    

    private void Start()
    {
        pl = GameObject.FindObjectOfType<Player>();
        health = maxHealth;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            Debug.Log("коллизия есть");
            BulletHit();
        }
    }

    void BulletHit()
    {
        health -= pl.damage;
        Debug.Log("текущее здоровье:" + health);
        if (health<=0)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        Destroy(gameObject);
        GameObject.FindObjectOfType < GameManager > ().enemiesAlive--;

    }

}
