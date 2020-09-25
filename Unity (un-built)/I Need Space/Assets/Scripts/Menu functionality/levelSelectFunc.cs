using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class levelSelectFunc : MonoBehaviour
{
    private GameObject mainMenu;
    private GameObject fadeObj;
    private string buttonPressed;
    private string levelName;
    private int currentPage;

    private audioManager audio;
    private levelFadeIn levelFade;

    public GameObject page1;
    public GameObject page2;

    public Button[] levelButtons;
    public Button nextPage;
    public Button prevPage;
    private void Awake()
    {
        mainMenu = GameObject.FindGameObjectWithTag("mainMenu"); //gets main menu gameObject from tag        
        fadeObj = GameObject.FindGameObjectWithTag("levelFade"); //gets level transition object from tag
        levelFade = fadeObj.GetComponent<levelFadeIn>(); //gets level transition script from fadeObj

        audio = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<audioManager>(); //gets audio manager script from main camera (using tag finding)

        currentPage = 1; //sets current level select page to 1 on awake
        page1.SetActive(true); //sets 1st page to visible
        page2.SetActive(false); //sets 2nd page to invisible

        nextPage.interactable = true; //allows next page button to be pressed
        prevPage.interactable = false; // doesnt allow next page button to be pressed

        reloadLevels(); // loads levels by checking playerPrefs to find how many levels are unlocked
    }

    private void Start()
    {
        gameObject.SetActive(false); //sets self to inactive once all set-up has been completed
    }

    public void loadLevel()
    {
        buttonPressed = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text; //gets the text component of whichever button was pressed yby accessing the EventSystem
        levelName = "level" + buttonPressed; //sets level to load on press using string manipulation and button text component (e.g. "1")
        Debug.Log(levelName); 
        fadeObj.SetActive(true); //enables levelTransition object 
        levelFade.startFade(levelName); //calls startFade with level to be loaded as parameter
    }

    public void backToMenu() //disbales self and enables main menu
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void reloadLevels() //re-evaluates which buttons should be enabled / disbaled based on how many levels have been unlocked
    {
        int levelReached = PlayerPrefs.GetInt("levelUnlocked", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
            {
                levelButtons[i].interactable = false;
            }
            else
            {
                levelButtons[i].interactable = true;
            }
        }
    }

    public void levelButton(string buttonName) //attached to level select page buttons (arrows)
    {                                          //function depends on parameter passed
        bool page1Activate = true;
        switch (buttonName)
        {
            case "next":
                page1Activate = false;
                currentPage = 2;
                break;
            case "previous":
                page1Activate = true;
                currentPage = 1;
                break;
        }

        page1.SetActive(page1Activate);
        page2.SetActive(!page1Activate);

        nextPage.interactable = page1Activate;
        prevPage.interactable = !page1Activate;
    }
}
