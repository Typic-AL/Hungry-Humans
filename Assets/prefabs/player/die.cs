using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class die : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    private bool soundPlayed = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gm.i.dead = !gm.i.player.gameObject.activeSelf;

        if(gm.i.dead && !soundPlayed)
        {
            audioSource.PlayOneShot(audioSource.clip);
            soundPlayed = true;
        }
        print(gm.i.spawnedAis.Count);
    }

    public void Die()
    {
        if(gm.i.player.gameObject.activeSelf)
                gm.i.player.gameObject.SetActive(false);
        gm.i.dead = true;
        gm.i.player.gameObject.SetActive(false);
    }
}
