using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Weapon> playerList;
    public List<Weapon> globalList;

    public float time;
    public int score;
    public int enemiesAlive;
    public int totalEnemies;

    public Slider reloadingProgressSlider;

   public shoot shScriGM;
    
    
    public AudioClip menuMusic;
    public AudioClip firstLevelMusic;
    public AudioClip secondLevelMusic;
    public AudioSource sourceOfAudio;

    void Start()
    {
        playerList = gameObject.GetComponent<WeaponFacric>().CreateDefaults();
        globalList = gameObject.GetComponent<WeaponFacric>().CreateGlobalWeaponPool();
        foreach(Weapon w in playerList)
        {
            Debug.Log(w);
            Debug.Log(w.damage/w.fireRateDelay);
        }
        reloadingProgressSlider.gameObject.SetActive(false);

        
    }

    void Update()
    {
        
    }

    public void ZeroSlider()
    {
        reloadingProgressSlider.value = 0;
    }
}
