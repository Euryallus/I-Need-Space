using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuDisplay : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject winMenu;

    public bool menuOn = false;

    private void Awake()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu"); //finds PauseMenu from searching tag
        winMenu = GameObject.FindGameObjectWithTag("WinMenu"); 
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameObject.FindGameObjectWithTag("Player").GetComponent<rocketLand>().won == false && GameObject.FindGameObjectWithTag("Player").GetComponent<rocketLaunch>().initialLaunch == true) //if the escape key is hit and the player hasn't finished the game
        { 
            menuOn = !menuOn; //turn on / turn off menu (depending on previous value)
            Time.timeScale = (menuOn? 0 : 1); //conditional operator; if menuOn = true, value set to 0, else 1
            pauseMenu.SetActive(menuOn); //sets menu active to menuOn bool state
        }
    }    
}
