using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class optionsFunctions : MonoBehaviour
{
    private GameObject mainMenu;
    private GameObject post;
    private GameObject levelSelect;

    public Button muteButton;
    public Sprite muted;
    public Sprite unMuted;

    private void Awake()
    {
        mainMenu = GameObject.FindGameObjectWithTag("mainMenu"); //gets main menu game object
        post = GameObject.FindGameObjectWithTag("postProcessing"); //sets post-processin
        levelSelect = GameObject.FindGameObjectWithTag("levelSelect");

        updateMute();

        gameObject.transform.GetChild(2).gameObject.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Volume", 0.5f); //sets default value of slider to volume stored in playerPrefs (if none is present, 0.5)
    }

    public void mute() //mutes all audio in-game
    {
        switch (PlayerPrefs.GetInt("Muted")) {
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

    public void FixedUpdate()
    {
        updateMute();
    }

    public void back() //returns to main menu
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void setGraphics(int level) //sets graphics level based on parameter input
    {
        QualitySettings.SetQualityLevel(level);
    }

    public void resetData() //resets all saved data back to defaults
    {
        PlayerPrefs.DeleteAll();
        levelSelect.GetComponent<levelSelectFunc>().reloadLevels();
        gameObject.transform.GetChild(2).gameObject.GetComponent<Slider>().value = 0.5f;
        PlayerPrefs.SetInt("Muted", 0);
        AudioListener.volume = 0.5f;
    }

    public void setVolume() //sets volume stored in playerPrefs based on value input on slider
    {
        PlayerPrefs.SetFloat("Volume", gameObject.transform.GetChild(2).gameObject.GetComponent<Slider>().value);
        AudioListener.volume = gameObject.transform.GetChild(2).gameObject.GetComponent<Slider>().value;
    }

    private void updateMute() //updates which sprite the Mute button should have using playerPrefs data
    {
        switch (PlayerPrefs.GetInt("Muted")) //assigns the mute button the correct sprite based on whether the game's muted or not
        {
            case 1:
                muteButton.GetComponent<Image>().sprite = muted;
                break;
            case 0:
                muteButton.GetComponent<Image>().sprite = unMuted;
                break;
        }
    }

}
