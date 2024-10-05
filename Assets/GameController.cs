using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool won = false;
    public int donutsGained = 0;
    public bool gameOver = false;
    bool first = true;

    int startAis = 0;
    
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
                gm.i.coins += donutsGained;
            }
            else if(gm.i.spawnedAis.Count > 0 && gm.i.dead)
            {
                won = false;
                donutsGained = (startAis - gm.i.spawnedAis.Count) * 10 * (gm.i.player.level / 2);
                gameOver = true;
                gm.i.coins += donutsGained;
            }
        }
        
    }
}
