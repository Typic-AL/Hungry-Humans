using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnStart : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
        SaveData data = SaveSystem.Load();
        
        
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main Menu") && gm.i.saveFileFound)
        {
            gm.i.coins = data.coins;
            gm.i.upgrades = data.upgrades;
            
            gm.i.aiTargetedFood = new List<GameObject>(new GameObject[10]);
            gm.i.spawnedAis = new List<ai>();
            gm.i.dead = false;
            
        }

        

        print(Application.persistentDataPath);


    }

    

    // Update is called once per frame
    void Update()
    {
        


    }

    private void OnApplicationQuit()
    {
        SaveSystem.Save(gm.i);
    }

    
}
