using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreDisplay : MonoBehaviour
{
    public Text totalMovesOutput;
    public Text overallScoreOutput;
    public Text averageMovesOutput;

    private int levelsUnlocked;
    private int totalMoves;
    private string rank;

    private GameObject mainMenu;
    private void Awake()
    {
        mainMenu = GameObject.FindGameObjectWithTag("mainMenu"); //accesses the main menu game object using tag
        updateScores(); 
    }
    private void Start()
    {
        gameObject.SetActive(false); //sets self to inactive
    }

    private void OnEnable()
    {
        updateScores();
    }

    private void updateScores()
    {
        levelsUnlocked = PlayerPrefs.GetInt("levelUnlocked", 1); //gets number of levels unlocked from playerPrefs

        totalMoves = 0; 

        for (int i = 1; i < levelsUnlocked + 1; i++)
        {
            totalMoves += PlayerPrefs.GetInt("level" + i.ToString(), 0); //cycles each level unlocked and creates total number from "high scores"
        }

        totalMovesOutput.text = totalMoves.ToString(); //outputs totals to UI text component

        averageMovesOutput.text = (totalMoves / levelsUnlocked).ToString(); //creates total & outputs to UI text component

        switch(totalMoves / levelsUnlocked) //uses average moves per level to create rank
        {
            case 1:
                rank = "S+";
                break;
            case 2:
                rank = "S";
                break;
            case 3:
                rank = "A";
                break;
            case 4:
                rank = "B";
                break;
            case 5:
                rank = "C";
                break;
            default:
                rank = "D";
                break;
        }

        overallScoreOutput.text = rank; //outputs rank to UI text component
    }

    public void backToMain() //takes user back to main menu
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
