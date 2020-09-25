using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuFunc : MonoBehaviour
{
    private GameObject levelSelectMenu; //stores game objects for all main menu "screens"
    private GameObject optionsMenu;
    private GameObject scoresMenu;
    private void Awake()
    {
        levelSelectMenu = GameObject.FindGameObjectWithTag("levelSelect"); //gets screens via tags
        optionsMenu = GameObject.FindGameObjectWithTag("options");
        scoresMenu = GameObject.FindGameObjectWithTag("scores");
        optionsMenu.SetActive(false);

        PlayerPrefs.SetFloat("backgroundPlaytime", 0.0f); //resets audio to play from beginning on menu load
        PlayerPrefs.SetFloat("backgroundMusic", 0.0f);
    }

    public void openMenu(string menu) //opens menu screen depending on parameter passed
    {
        switch (menu) { //switchs statement for each possible outcome
            case "levelSelect":
                levelSelectMenu.SetActive(true);
                break;
            case "options":
                optionsMenu.SetActive(true);
                break;
            case "scores":
                scoresMenu.SetActive(true);
                break;
            case "quit":
                Application.Quit();
                break;
            default:
                return;
        }
        gameObject.SetActive(false);
    }


}
