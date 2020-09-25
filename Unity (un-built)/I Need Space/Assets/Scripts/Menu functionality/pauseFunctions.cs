using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class pauseFunctions : MonoBehaviour
{
    private scoreManager playerScore;
    
    public Sprite muted;
    public Sprite unMuted;

    public Button muteButton;
    public Text scoreCount;

    private void Start()
    {
        playerScore = GameObject.FindGameObjectWithTag("Player").GetComponent<scoreManager>();

        switch (PlayerPrefs.GetInt("Muted")) //assigns the mute button the correct sprite based on whether the game's muted or not
        {
            case 1:
                muteButton.GetComponent<Image>().sprite = muted;
                break;
            case 0:
                muteButton.GetComponent<Image>().sprite = unMuted;
                break;
        }

        gameObject.SetActive(false); //disables self once all set-up is complete
    }

    private void OnEnable()
    {
        scoreCount.text = playerScore.getCurrentScore().ToString(); //updates score count when enabled
    }

    public void restartLevel() //sets time in audio manager and reloads current scene
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<audioManager>().SetTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainMenu() //loads the main menu scene
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void mute() 
    {
        switch (PlayerPrefs.GetInt("Muted"))
        {
            case 1:
                PlayerPrefs.SetInt("Muted", 0); //saves value of "muted" in playerPrefs to opposite of what it was previously and sets sprite accordingly
                muteButton.GetComponent<Image>().sprite = unMuted;
                break;
            case 0:
                PlayerPrefs.SetInt("Muted", 1);
                muteButton.GetComponent<Image>().sprite = muted;
                break;
        }
    }
}
