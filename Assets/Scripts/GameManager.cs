using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
