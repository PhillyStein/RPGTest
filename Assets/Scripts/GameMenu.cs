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

    // Start is called before the first frame update
    void Start()
    {
        theMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if(theMenu.activeInHierarchy)
            {
                //theMenu.SetActive(false);
                //GameManager.instance.gameMenuOpen = false;
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
    }

    public void CloseMenu()
    {
        for(int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }

        theMenu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;
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
}
