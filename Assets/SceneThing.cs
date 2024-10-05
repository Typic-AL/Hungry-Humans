using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneThing : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMenu()
    {
        SaveSystem.Save(gm.i);
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadWorld()
    {
        SaveSystem.Save(gm.i);
        SceneManager.LoadScene("Solo");
    }

    

    
}
