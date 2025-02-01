using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject deathScreen;
    public GameObject winScreen;
    public TextMeshProUGUI MapShrinkText;
    public Timer MapShrinkTimer;

    public GameController gc;

    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI coinsGainedText;

    private AudioSource winAudio;
    private bool winAudioPlayed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if(winScreen) winAudio = winScreen.GetComponent<AudioSource>();
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main Menu"))
        {
            
        }
        else
        {
            deathScreen.SetActive(false);
            winScreen.SetActive(false);
            coinsGainedText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Solo"))
        {
            UpdateShrinkText();
            UpdateDeathScreen();
            UpdateWinScreen();
            UpdateCoinsGainedText();
        }

        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main Menu"))
        {
            UpdateCoinsText(coinsText);
            gm.i.deathSoundPlayed = false;
        }
        
        
    }

    private void UpdateShrinkText()
    {
        int seconds = (int)MapShrinkTimer.GetRemainingSeconds();
        int minutes = (int)MapShrinkTimer.timeRemaining/60;
        if(minutes > 0)
        {
            if(seconds - minutes*60 < 10)
                MapShrinkText.text = "Map Shrinks In: " + minutes + ":0" + (seconds - minutes*60);
            else
                MapShrinkText.text = "Map Shrinks In: " + minutes + ":" + (seconds - minutes*60);
        }     
        else
            MapShrinkText.text = "Map Shrinks In: " + seconds;
            
    }

    private void UpdateDeathScreen()
    {
        deathScreen.SetActive(gc.gameOver && !gc.won);

        if(deathScreen.activeSelf)
        {
            
        }
    }

    private void UpdateWinScreen()
    {
        if(gc.gameOver && gc.won)
        {
            winScreen.SetActive(true);
            if(!winAudioPlayed)
            {
                winAudio.PlayOneShot(winAudio.clip);
                winAudioPlayed = true;
            }
        }
    }

    private void UpdateCoinsGainedText()
    {
        if(gc.gameOver)
        {
            string text = "+ " + gc.donutsGained + " Coins";
            if(!coinsGainedText.gameObject.activeSelf) coinsGainedText.gameObject.SetActive(true);
            if(coinsGainedText.text != text) coinsGainedText.text = text;
        }
        else
        {
            if(coinsGainedText.gameObject.activeSelf) coinsGainedText.gameObject.SetActive(false);
        }
    }

    private void UpdateCoinsText(TextMeshProUGUI text)
    {
        float coins = gm.i.coins;


        if (coins >= 1000 && coins < 1000000)
        {
            float displayCoins = coins;
            displayCoins = Mathf.Round((displayCoins / 1000) * 100.0f) * .01f;
            text.text = displayCoins.ToString() + "K";
        }
            
        if (coins >= 1000000 && coins < 1000000000)
        {
            float displayCoins = coins;
            displayCoins = Mathf.Round((displayCoins / 1000000) * 100.0f) * .01f;
            text.text = displayCoins.ToString() + "M";
        }
        if (coins >= 1000000000)
        {
            float displayCoins = coins;
            displayCoins = Mathf.Round((displayCoins / 1000000000) * 100.0f) * .01f;
            text.text = displayCoins.ToString() + "B";
        }
        if (coins < 1000)
        {
            text.text = (coins).ToString();
        }     
    }
}
