using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{   
    public int coins;
    public Upgrades upgrades;
    



    public SaveData(gm gm)
    {
        coins = gm.i.coins;
        upgrades = gm.i.upgrades;
    }

}