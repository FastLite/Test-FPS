using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFacric : MonoBehaviour
{//здесь выстовляются все префабы через инспектор
    public GameObject pistolPrefab;
    public GameObject rifflePrefab;
    public GameObject assaultPrefab;
    public GameObject rocketLauncherPrefab;
    public GameObject shotgunPrefab;
    public GameObject grenadeLauncherPrefab;
    public GameObject railgunPrefab;

    public AudioClip pShootSound;
    public AudioClip pReloadingSound;
    
    public AudioClip rifShootSound;
    public AudioClip rifReloadingSound;
    
    public AudioClip assShootSound;
    public AudioClip assReloadingSound;
    
    public AudioClip rockShootSound;
    public AudioClip rockReloadingSound;
    
    public AudioClip shoShootSound;
    public AudioClip shoReloadingSound;
    
    public AudioClip greShootSound;
    public AudioClip greReloadingSound;
    
    public AudioClip railShootSound;
    public AudioClip railReloadingSound;


    /// <summary>
    ///  этот метод создает изначальные оружия для игрока
    /// </summary>
    /// <returns></returns>
    public List<Weapon> CreateDefaults()
    {
        List<Weapon> resultListStarter = new List<Weapon>();
        resultListStarter.Add(createPistol());
        resultListStarter.Add(createRifle());
        resultListStarter.Add(createAssault());
        resultListStarter.Add(createShotgun());
        for (int i = 0; i < 2; i++)
        {
            resultListStarter.RemoveAt(Random.Range(1, 4 -i));
        }
            return resultListStarter;
    }


    /// <summary>
    /// создает все что стреляет GachiBASS
    /// </summary>
    /// <returns></returns>
    public List<Weapon> CreateGlobalWeaponPool()
    {
        List<Weapon> resultListGlobal = new List<Weapon>();
        resultListGlobal.Add(createPistol());
        resultListGlobal.Add(createRifle());
        resultListGlobal.Add(createAssault());
        resultListGlobal.Add(createShotgun());
        resultListGlobal.Add(createGranadeLauncher());
        resultListGlobal.Add(createRocket());
        resultListGlobal.Add(createRailgun());
        return resultListGlobal;

        
    }


    /// <summary>
    /// Возвращяет пистолет с базывыми значениями
    /// </summary>
    /// <returns></returns>
    Weapon createPistol()
    {
        Weapon pistol = new Weapon
        {
            ammoMax = 10,

            ammoLeft = 20,
            damage = 15,
            fireRateDelay = 0.5f,
            launchForce = 2,
            reloadingSound = pReloadingSound,
            shootSound = pShootSound,
            reloadingTime = 1f,
            prefab = pistolPrefab,
            shootPatern = 1
        };
        return pistol;
    }

    Weapon createRifle()
    {
        Weapon riffle = new Weapon
        {
            ammoMax = 8,
            ammoLeft = 8,
            damage = 30,
            fireRateDelay = 1f,
            launchForce = 2,
            reloadingSound = rifReloadingSound,
            shootSound = rifShootSound,
            reloadingTime = 1.5f,
            prefab = rifflePrefab,
            shootPatern = 1
        };
        return riffle;
    }
    Weapon createShotgun()
    {
        Weapon Shotgun = new Weapon
        {
            ammoMax = 6,
            ammoLeft = 6,
            damage = 6,
            fireRateDelay = 0.7f,
            launchForce = 2,
            reloadingSound = shoReloadingSound,
            shootSound = shoShootSound,
            reloadingTime = 1.5f,
            prefab = shotgunPrefab,
            shootPatern = 2
        };
        return Shotgun;
    }
    Weapon createRocket()
    {
        Weapon Rocket = new Weapon
        {
            ammoMax = 20,
            ammoLeft = 20,
            damage = 20,
            fireRateDelay = 0.5f,
            launchForce = 2,
            reloadingSound = rockReloadingSound,
            shootSound = rockShootSound,
            reloadingTime = 1.5f,
            prefab = rocketLauncherPrefab,
            shootPatern = 1
        };
        return Rocket;
    }
    Weapon createGranadeLauncher()
    {
        Weapon GranadeLauncher = new Weapon
        {
            ammoMax = 20,
            ammoLeft = 20,
            damage = 20,
            fireRateDelay = 0.5f,
            launchForce = 2,
            reloadingSound = greReloadingSound,
            shootSound = greShootSound,
            reloadingTime = 1.5f,
            prefab = grenadeLauncherPrefab,
            shootPatern = 1
        };
        return GranadeLauncher;
    }

    Weapon createRailgun()
    {
        Weapon Railgun = new Weapon
        {
            ammoMax = 20,
            ammoLeft = 20,
            damage = 20,
            fireRateDelay = 0.5f,
            launchForce = 2,
            reloadingSound = railReloadingSound,
            shootSound = railShootSound,
            reloadingTime = 1.5f,
            prefab = railgunPrefab,
            shootPatern = 1
        };
        return Railgun;
    }
    
    Weapon createAssault()
    {
        Weapon Assault = new Weapon
        {
            ammoMax = 30,
            ammoLeft = 30,
            damage = 5,
            fireRateDelay = 0.1f,
            launchForce = 8,
            reloadingSound = assReloadingSound,
            shootSound = assShootSound,
            reloadingTime = 2.5f,
            prefab = assaultPrefab,
            shootPatern = 1

        };
        
        return Assault;
    }

}
