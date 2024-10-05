using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Upgrades {
    public Upgrade speed;
    public Upgrade range;


    public Upgrades(int lvl1, float price1, int lvl2, float price2)
    {
        speed.lvl = lvl1;
        speed.price = price1;
        range.price = price2;
        range.lvl = lvl2;
    }
}

[Serializable]
public struct Upgrade {
    public int lvl;
    public float price;

    public Upgrade(int lvl, int price) {
        this.lvl = lvl;
        this.price = price;
    }
}

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
    public List<GameObject> aiTargetedFood = new List<GameObject>(new GameObject[10]);
    public List<ai> spawnedAis = new List<ai>();
    public Grow player;

    public bool saveFileFound;

    public Upgrades upgrades = new Upgrades(1, 5, 1, 10);
    public FoodSpawner foodSpawner;
    public bool dead = false;
    
    public int coins = 100;

}
