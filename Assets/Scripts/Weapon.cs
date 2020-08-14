

using UnityEngine;
// базовый класс описыющий структуры оружия и содержащий все его значения
public class Weapon
{
    public GameObject prefab;
    public int damage;
    public float fireRateDelay;
    public int ammoLeft;
    public int ammoMax;
    public float reloadingTime;
    public float launchForce;
    public int shootPatern; //1 = regular bullet, 2 = spreading shoot (shootgun), 3 constant force should be added (rocket like), 4 - парабола (по типу гранатомета)

    public AudioClip shootSound;
    public AudioClip reloadingSound;

    public override string ToString()
    {
        return "damage = " + damage +"fireRateDelay = " + fireRateDelay;
    }
}
