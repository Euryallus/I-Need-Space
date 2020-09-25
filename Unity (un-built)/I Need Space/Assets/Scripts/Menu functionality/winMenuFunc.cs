using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class winMenuFunc : MonoBehaviour
{
    private GameObject levelFade;
    private CanvasGroup canvas;
    private scoreManager score;

    public Text movesCount;
    public GameObject highscoreDisplay;

    bool fadeIn = false;
    private void Start()
    {
        highscoreDisplay.SetActive(false); //disables "new highscore" text
        score = GameObject.FindGameObjectWithTag("Player").GetComponent<scoreManager>(); //gets scoreManager component
        levelFade = GameObject.FindGameObjectWithTag("levelFade"); //gets levelFade object
        canvas = gameObject.transform.GetChild(0).GetComponent<CanvasGroup>(); //gets canvasGroup component

        gameObject.SetActive(false);  //sets self to inactive
    }
    public void  nextLevel() //loads next level on button by calling startFade from levelFade
    {
        levelFade.SetActive(true);
        levelFade.GetComponent<levelFadeIn>().startFade("next");
    }

    private void OnEnable() //when enabled update movesCount text component and fade in (using canvasGroup.alpha in Update)
    {
        movesCount.text = score.getCurrentScore().ToString();
        if(score.newHighscore == true)
        {
            highscoreDisplay.SetActive(true); //if new highscore reached, enable the "new highscore" text
        }
        fadeIn = true;
        canvas.alpha = 0; //set alpha of CanvasGroup to 0 
    }

    private void FixedUpdate()
    {
        if(fadeIn && canvas.alpha < 1) //once fadeIn is set to true & win menu is not full alpha, increase alpha to 1 over 1 second
        {
            canvas.alpha += 1 * Time.fixedDeltaTime;
        }
    }

    public void mainMenu() //exit to main menu, disables self and uses levelFade to fade to main menu
    {
        levelFade.SetActive(true);
        levelFade.GetComponent<levelFadeIn>().startFade("MainMenu");
    }
}
