using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public CharStats[] playerStats;

    public bool gameMenuOpen, 
                dialogueActive,
                fadingBetweenAreas,
                shopActive;

    public string[] itemsHeld;
    public int[] numOfItems;
    public Item[] referenceItems;

    public int currentGold;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);

        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameMenuOpen || dialogueActive || fadingBetweenAreas || shopActive)
        {
            PlayerController.instance.canMove = false;
        } else {
            PlayerController.instance.canMove = true;
        }

        if(Input.GetKeyUp(KeyCode.P))
        {
            AddItem("Iron Armour");
        }

        if(Input.GetKeyUp(KeyCode.O))
        {
            AddItem("Mana Potion");
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            RemoveItem("Iron Armour");
        }

        if (Input.GetKeyUp(KeyCode.U))
        {
            RemoveItem("Mana Potion");
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadData();
        }
    }

    public Item GetItemDetails(string itemToGrab)
    {
        for(int i = 0; i < referenceItems.Length; i++)
        {
            if (referenceItems[i] != null)
            {
                if (referenceItems[i].itemName == itemToGrab)
                {
                    return referenceItems[i];
                }
            }
        }
        
        return null;
    }

    public void SortItems()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;

            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    numOfItems[i] = numOfItems[i + 1];
                    numOfItems[i + 1] = 0;

                    if(itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public void AddItem(string itemToAdd)
    {
        /* My Code
        for(int i = 0; i < itemsHeld.Length; i++)
        {
            if(itemsHeld[i] == itemToAdd)
            {
                numOfItems[i]++;
                break;
            } else if(itemsHeld[i] == "")
            {
                itemsHeld[i] = itemToAdd;
                numOfItems[i]++;
                break;
            }
        }
        GameMenu.instance.ShowItems();
        */

        // James' code
        int newItemPosition = 0;
        bool foundSpace = false;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                i = itemsHeld.Length;
                foundSpace = true;
            }
        }

        if (foundSpace)
        {
            bool itemExists = false;
            for(int i = 0; i < referenceItems.Length; i++)
            {
                if(referenceItems[i].itemName == itemToAdd)
                {
                    itemExists = true;

                    i = referenceItems.Length;
                }
            }

            if (itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numOfItems[newItemPosition]++;
                if(GameMenu.instance.activeItem == null)
                {
                    GameMenu.instance.SelectItem(GetItemDetails(itemsHeld[0]));
                }
            }
            else
            {
                Debug.LogError(itemToAdd + " does not exist!");
            }
        }
        GameMenu.instance.SelectItem(GetItemDetails(itemsHeld[newItemPosition]));
        GameMenu.instance.ShowItems();
    }

    public void RemoveItem(string itemToRemove)
    {
        /* My Code
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToRemove)
            {
                numOfItems[i]--;
                if(numOfItems[i] <=0)
                {
                    itemsHeld[i] = "";
                }
                break;
            }
        }
        GameMenu.instance.ShowItems();
        */

        // James' code
        bool foundItem = false;
        int itemPosition = 0;

        for(int i = 0; i < itemsHeld.Length; i++)
        {
            if(itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;

                i = itemsHeld.Length;
            }
        }

        if(foundItem)
        {
            numOfItems[itemPosition]--;

            if(numOfItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
                SortItems();
                if (itemsHeld[0] != "")
                {
                    GameMenu.instance.SelectItem(GetItemDetails(itemsHeld[0]));
                } else
                {
                    GameMenu.instance.SelectItem(null);
                }
            }

            GameMenu.instance.ShowItems();
        } else
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_x", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", PlayerController.instance.transform.position.z);

        //Save Character Info
        for(int i = 0; i<playerStats.Length; i++)
        {
            if(playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 1);
            } else {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 0);
            }
            
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Level", playerStats[i].charLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentExp", playerStats[i].currentExp);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentHP", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxHP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentMP", playerStats[i].currentMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxMP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Defence", playerStats[i].defence);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_WpnPwr", playerStats[i].wpnPwr);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_ArmrPwr", playerStats[i].armrPwr);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EqippedWpn", playerStats[i].equippedWpn);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedArmr", playerStats[i].equippedArmr);
        }

        //Store Inventory Data
        for(int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numOfItems[i]);
        }
    }

    public void LoadData()
    {
        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));

        for(int i = 0; i < playerStats.Length; i++)
        {
            if(PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            } else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            playerStats[i].charLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Level");
            playerStats[i].currentExp = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentExp");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentHP");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxHP");
            playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentMP");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxMP");
            playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Strength");
            playerStats[i].defence = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Defence");
            playerStats[i].wpnPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_WpnPwr");
            playerStats[i].armrPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_ArmrPwr");
            playerStats[i].equippedWpn = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EqippedWpn");
            playerStats[i].equippedArmr = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedArmr");
        }

        for(int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }
    }
}
