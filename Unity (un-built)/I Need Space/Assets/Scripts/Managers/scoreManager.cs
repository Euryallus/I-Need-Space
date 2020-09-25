using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scoreManager : MonoBehaviour
{
    private int currentHighscore;
    private string currentLevel;
    private int currentScore;

    public bool newHighscore;

    private void Awake()
    {
        currentLevel = SceneManager.GetActiveScene().name; //gets current highscore from playerPrefs (using level name)
        currentHighscore = PlayerPrefs.GetInt(currentLevel, 50);
    }

    public void addScore() //adds 1 to score (called on each launch)
    {
        currentScore += 1;
    }

    public int getCurrentScore() //returns current score count
    {
        return currentScore;
    }

    public int getCurrentHighscore() //returns highscore
    {
        return currentHighscore;
    }

    public void endLevel() //saves score data to playerPrefs IF new score is < than former "high score"
    {
        if(currentScore < currentHighscore)
        {
            PlayerPrefs.SetInt(currentLevel, currentScore);
            newHighscore = true;
        }
    }
}
