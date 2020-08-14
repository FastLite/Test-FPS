using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int damage;
    public int maxHealth;
    public int health;
    public int collisionDamage;


    public float speed = 6.0f;

    public int maxWeapons = 1;
    public int selectedWeapon;

    public Transform initialPos;

    shoot shScri;
    GameManager gm;

    public Weapon currentWeapon;

    GameObject spawned;
    public GameObject weaponRoot;
    public bool isWeaponActive;

    void Start()
    {

        gm = GameObject.FindObjectOfType<GameManager>();
        health = maxHealth;
        selectedWeapon = 0;
        currentWeapon = gm.playerList.First();
        InstantateNewWeapon(currentWeapon);

    }


    void Update()
    {
        ChangeWeapon();

    }

    void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedWeapon < maxWeapons)
                selectedWeapon++;
            else
                selectedWeapon = 0;
            currentWeapon = gm.playerList[selectedWeapon];
            InstantateNewWeapon(currentWeapon);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (selectedWeapon > 0)
                selectedWeapon--;
            else
                selectedWeapon = maxWeapons;
            currentWeapon = gm.playerList[selectedWeapon];
            InstantateNewWeapon(currentWeapon);
        }
        

    }

    void InstantateNewWeapon(Weapon weapon)
    {
        Destroy(spawned);
        spawned = Instantiate(weapon.prefab, initialPos);
        damage = weapon.damage;


        shScri = GameObject.FindObjectOfType<shoot>();

        gm.shScriGM = shScri;
    }
    public void ShowWeapon(bool show)
    {
        weaponRoot.SetActive(show);

       

        isWeaponActive = show;
    }
}