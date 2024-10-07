using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ItemType
{
    UPGRADE,
    ITEM
}

public class ShopItem : MonoBehaviour
{
    public float price;
    public ItemType type;
    public AudioSource buyAudio;

    public Upgrade speed = new Upgrade(0, 0);
    public Upgrade range = new Upgrade(0, 0);

    private bool ended = true;

    [SerializeField] private String itemName;
    [SerializeField] private bool hasText;

    private TextMeshProUGUI itemText;

    private Upgrade upgrade;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if(hasText)
        {
            if(!gameObject.GetComponent<TextMeshProUGUI>())
                itemText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            else
                itemText = gameObject.GetComponent<TextMeshProUGUI>();
            
        }

        if(stringContains(itemName, "speed"))
            upgrade = speed;
        else if(stringContains(itemName, "range"))
            upgrade = range;
        
    }

    // Update is called once per frame
    
    
    void Update()
    {
        if(ended) setVarStart();

        

        if(hasText)
        {
            switch(type)
            {
                case ItemType.UPGRADE:
                    setText("speed", "Speed Lvl " + (speed.lvl + 1).ToString() + ":\n" + price.ToString() + " coins");
                    setText("range", "Food Value Lvl " + (range.lvl + 1).ToString() + ":\n" + price.ToString() + " coins");
                    if(speed.lvl == 10)
                        setText("speed", "Speed Lvl Max");
                    if(range.lvl == 10)
                        setText("range", "Food Value Lvl Max");
                    break;
            }
        }
    }

    private void setText(string name, string text)
    {
        if(stringContains(itemName, name))
        {
            itemText.text = text;
        }
    }

    private bool stringContains(string origString, string stringToFind)
    {
        return origString.IndexOf(stringToFind, System.StringComparison.OrdinalIgnoreCase) >= 0;
    }


    private void scalePrice(Upgrade up, int lvlSwitch, float multi1, float multi2)
    {
        if(up.lvl < lvlSwitch) price *= multi1;
        else if (up.lvl >= lvlSwitch) price *= multi2;
        price = (int)price;
    }

    public void Buy()
    {
        if(gm.i.coins >= price && upgrade.lvl < 10)
        {
            gm.i.coins -= (int)price;
            buyAudio.PlayOneShot(buyAudio.clip);

            switch(type)
            {
                case ItemType.UPGRADE:
                    if(stringContains(itemName, "speed"))
                    {
                        speed.lvl++;
                        scalePrice(speed, 4, 1.5f, 2.0f);
                    }
                    if(stringContains(itemName, "range"))
                    {
                        range.lvl++;
                        scalePrice(range, 3, 1.5f, 2.2f);
                    }

                    if(!ended) setVarEnd();
                    break;
            }
        }
    }

    public void debug()
    {
        print($"Coins: {gm.i.coins}, Price: {price}");
    }

    private void setVarStart()
    {
        if(stringContains(itemName, "speed")) speed = gm.i.upgrades.speed;
        if(stringContains(itemName, "range")) range = gm.i.upgrades.range;

        if(stringContains(itemName, "speed")) price = speed.price;
        if(stringContains(itemName, "range")) price = range.price;

        if(stringContains(itemName, "speed")) upgrade = speed;
        else upgrade = range;

        ended = false;
    }

    private void setVarEnd()
    {
        if(stringContains(itemName, "speed")) gm.i.upgrades.speed = speed;
        if(stringContains(itemName, "range")) gm.i.upgrades.range = range;

        if(stringContains(itemName, "speed")) gm.i.upgrades.speed.price = price;
        if(stringContains(itemName, "range")) gm.i.upgrades.range.price = price;

        ended = true;
    }
}
