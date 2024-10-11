using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool won = false;
    public int donutsGained = 0;
    public bool gameOver = false;
    bool first = true;

    public AudioSource musicSource;

    int startAis = 0;

    public Rewarded rewardAd;
    
    void OnValidate()
    {
        gm.i.gc = GetComponent<GameController>();
    }
    
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if(first)
        {
            startAis = gm.i.spawnedAis.Count;
            first = false;
        }
            
        if(!gameOver)
        {
            if(gm.i.spawnedAis.Count == 0 && !gm.i.dead)
            {
                won = true;
                donutsGained = (startAis + 50) * (gm.i.player.level / 2);
                gameOver = true;
                //gm.i.coins += donutsGained;
            }
            else if(gm.i.spawnedAis.Count > 0 && gm.i.dead)
            {
                won = false;
                if((startAis - gm.i.spawnedAis.Count) > 0)
                {
                    donutsGained = (startAis - gm.i.spawnedAis.Count) * 10 * (gm.i.player.level / 2);
                }
                else
                {
                    donutsGained = 10 * gm.i.player.level;
                }
                
                gameOver = true;
                //gm.i.coins += donutsGained;
            }
        }
        if(gameOver)
        {
            musicSource.Pause();
        }
        
    }

    public void Revive()
    {
        rewardAd.ShowRewardedAd(ReviveAction);
    }

    void ReviveAction()
    {
        gm.i.Revive();
        musicSource.UnPause();
    }

    public void DoubleCoins()
    {
        rewardAd.ShowRewardedAd(DoubleCoinsAction);
        gm.i.coinsDoubled = true;
    }
    
    void DoubleCoinsAction()
    {
        donutsGained *= 2;
    }

    public void Menu()
    {
        gm.i.coins += donutsGained;
        gm.i.coinsDoubled = false;
        
        SaveSystem.Save(gm.i);
        SceneManager.LoadScene("Main Menu");
    }
}
