using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{

    public string charName;
    public int charLevel = 1;
    public int currentExp;
    public int[] expToNextLvl;
    public int maxLevel = 100;
    public int baseExp = 1000;

    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 30;
    public int[] MPLvlBonus;
    public int baseMPBonus = 10;
    public int strength;
    public int defence;
    public int wpnPwr;
    public int armrPwr;
    public string equippedWpn;
    public string equippedArmr;
    //public string equippedDsgs;
    public Sprite charImg;

    // Start is called before the first frame update
    void Start()
    {
        
        expToNextLvl = new int[maxLevel];
        expToNextLvl[1] = baseExp;
        for (int i = 2; i < expToNextLvl.Length; i++)
        {
            expToNextLvl[i] = Mathf.FloorToInt(expToNextLvl[i - 1] * 1.05f);
        }

        MPLvlBonus = new int[maxLevel];

        for (int i = 2; i < MPLvlBonus.Length; i++)
        {
            if(i % 3 == 0)
            {
                MPLvlBonus[i] = baseMPBonus;
                baseMPBonus = Mathf.FloorToInt(baseMPBonus * 1.1f);
            }
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            AddExp(250);
        }
    }

    public void AddExp(int expToAdd)
    {
        /* My own code
        int newExp = currentExp + expToAdd;
        int expToLvl = charLevel * 105;

        if(newExp > expToLvl)
        {
            charLevel++;
            currentExp = newExp - expToLvl;
        } else {
            currentExp = newExp;
        }
        */

        // James' code
        if (charLevel < maxLevel)
        {
            if (currentExp >= expToNextLvl[charLevel])
            {
                currentExp -= expToNextLvl[charLevel];
                charLevel++;

                // Determine whether to add to str or def based on odd or even
                
                if (charLevel % 2 == 0)
                {
                    strength++;
                }
                else
                {
                    defence++;
                }

                maxHP = Mathf.FloorToInt(maxHP * 1.05f);
                currentHP = maxHP;

                maxMP += MPLvlBonus[charLevel];
                currentMP = maxMP;
                
    }
}

        if(charLevel >= maxLevel)
        {
            currentExp = 0;
        }
        
    }

    public static implicit operator GameObject(CharStats v)
    {
        throw new NotImplementedException();
    }
}
