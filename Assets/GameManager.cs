using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gm
{
    private static gm instance;

    private gm()
    {

    }

    public static gm i

    {

        get
        {
            if (instance == null)
            {
                instance = new gm();
            }

            return instance;
        }
    }
    
    public float foodTimerCountdownAmount;
    public List<GameObject> aiTargetedFood = new List<GameObject>(6);
    public List<ai> spawnedAis = new List<ai>();
    public Grow player;
    public FoodSpawner foodSpawner;
}
