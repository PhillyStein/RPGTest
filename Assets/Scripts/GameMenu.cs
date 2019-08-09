using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{

    public GameObject theMenu;
    public GameObject[] windows;

    private CharStats[] playerStats;

    public Text[] nameText, 
                    hpText, 
                    mpText, 
                    expText, 
                    lvlText;

    public Slider[] expSlider;
    public Image[] charImage;
    public GameObject[] charStatHolder;
    public Image statusImage;
    public Text statusText;
    public GameObject[] statusButtons;

    public ItemButton[] itemButtons;

    public string selectedItem;
    public Item activeItem;
    public Text itemName,
                itemDesc,
                useButtonText,
                goldText;

    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceNames;

    public Button useButton,
                    discardButton;

    public static GameMenu instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        theMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if(theMenu.activeInHierarchy)
            {
                CloseMenu();
            } else
            {
                theMenu.SetActive(true);
                UpdateMainStats();
                GameManager.instance.gameMenuOpen = true;
            }
        }
    }

    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;

        for(int i = 0; i<playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);

                nameText[i].text = playerStats[i].charName;
                charImage[i].sprite = playerStats[i].charImg;
                hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                mpText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
                expText[i].text = playerStats[i].currentExp + "/" + playerStats[i].expToNextLvl[playerStats[i].charLevel];
                expSlider[i].maxValue = playerStats[i].expToNextLvl[playerStats[i].charLevel];
                expSlider[i].value = playerStats[i].currentExp;
                lvlText[i].text = "Lvl: " + playerStats[i].charLevel;
            }
            else
            {
                charStatHolder[i].SetActive(false);
            }
        }
    }

    public void UpdateStatus(int playerNumber)
    {
        OpenStatus();
        playerStats = GameManager.instance.playerStats;

        statusImage.sprite = playerStats[playerNumber].charImg;
        statusText.text = "Name: " + playerStats[playerNumber].charName +
                        "\n HP: " + playerStats[playerNumber].currentHP + "/" + playerStats[playerNumber].maxHP +
                        "\n MP: " + playerStats[playerNumber].currentMP + "/" + playerStats[playerNumber].maxMP +
                        "\n Strength: " + playerStats[playerNumber].strength +
                        "\n Defence: " + playerStats[playerNumber].defence +
                        "\n Equipped Weapon: " + playerStats[playerNumber].equippedWpn +
                        "\n Weapon Power: " + playerStats[playerNumber].wpnPwr +
                        "\n Equpped Armour: " + playerStats[playerNumber].equippedArmr +
                        "\n Armour Power: " + playerStats[playerNumber].armrPwr +
                        "\n XP to Next Level: " + playerStats[playerNumber].expToNextLvl[playerStats[playerNumber].charLevel];

        goldText.text = GameManager.instance.currentGold + " G";
    }

    public void ToggleWindow(int windowNumber)
    {
        UpdateMainStats();

        for (int i = 0; i < windows.Length; i++)
        {
            if(i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            } else
            {
                windows[i].SetActive(false);
            }
        }

        itemCharChoiceMenu.SetActive(false);
    }

    public void CloseMenu()
    {
        for(int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }

        theMenu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;

        itemCharChoiceMenu.SetActive(false);
    }

    public void OpenStatus()
    {
        UpdateMainStats();
        for (int i = 0; i < statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
        }
    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numOfItems[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem)
    {
        if (newItem != null)
        {

            useButton.gameObject.SetActive(true);
            discardButton.gameObject.SetActive(true);

            activeItem = newItem;

            if (activeItem.isItem)
            {
                useButtonText.text = "Use";
            }

            if (activeItem.isWeapon || activeItem.isArmour)
            {
                useButtonText.text = "Equip";
            }

            itemName.text = activeItem.itemName;
            itemDesc.text = activeItem.description;
        } else
        {
            useButton.gameObject.SetActive(false);
            discardButton.gameObject.SetActive(false);
            itemName.text = "";
            itemDesc.text = "";
        }
    }

    public void DiscardItem()
    {
        if(activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for(int i = 0; i < itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].charName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }
    }

    public void closeItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);
        closeItemCharChoice();
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }
}
