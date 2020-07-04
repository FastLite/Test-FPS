using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{



    public Transform spawnPoint;
    public Transform bulletRotation;

    public GameObject bulletPrefab;


    public float LastFire;

    GameManager gm;
    Player pl;

    public bool reloadingInProgress;
    
    public AudioSource sourceOfAudio;
    private float reloadingStartedTime;

    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        pl = GameObject.FindObjectOfType<Player>();

        sourceOfAudio = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        OnFire();

        if (reloadingInProgress)
            gm.reloadingProgressSlider.value = Time.time - reloadingStartedTime;


    }


    void OnFire()
    {
        if (Input.GetButton("Fire1") && LastFire + pl.currentWeapon.fireRateDelay <= Time.time && pl.currentWeapon.ammoLeft > 0 && !reloadingInProgress)
        {
            pl.currentWeapon.ammoLeft--;
            GameObject go = Instantiate(bulletPrefab, spawnPoint.transform.position, Quaternion.identity);
            go.transform.parent = null;
            go.GetComponent<Rigidbody>().AddForce(transform.forward * pl.currentWeapon.launchForce, ForceMode.Impulse);
            LastFire = Time.time;
            sourceOfAudio.PlayOneShot(pl.currentWeapon.shootSound);

        }
        else if (Input.GetKeyDown(KeyCode.R) || pl.currentWeapon.ammoLeft == 0 && !reloadingInProgress)
        {
            Debug.Log("reloading started");
            Invoke("Reloading", pl.currentWeapon.reloadingTime);
            gm.reloadingProgressSlider.maxValue = pl.currentWeapon.reloadingTime;
            reloadingInProgress = true;

            gm.reloadingProgressSlider.value = 0;
           
            reloadingStartedTime = Time.time;
            
            gm.reloadingProgressSlider.gameObject.SetActive(true);
            sourceOfAudio.PlayOneShot(pl.currentWeapon.reloadingSound);
        }
        
    }
    public void Reloading()
    {
        pl.currentWeapon.ammoLeft = pl.currentWeapon.ammoMax;
        Debug.Log("reloading Ended");
        reloadingInProgress = false;
        gm.reloadingProgressSlider.gameObject.SetActive(false);

    }

}
