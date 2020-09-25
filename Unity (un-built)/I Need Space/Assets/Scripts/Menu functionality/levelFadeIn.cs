using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelFadeIn : MonoBehaviour
{
    private bool fadeOut = false;
    private bool menuFade = false;

    private string load;
    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu") //plays intro noise IF the level being loaded isn't the main menu
        {
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private void FixedUpdate()
    {
        if (fadeOut == false)
        {
            if (gameObject.GetComponent<CanvasGroup>().alpha > 0)
            {
                gameObject.GetComponent<CanvasGroup>().alpha -= 1 * Time.fixedDeltaTime;
            }
            else
            {
                if (gameObject.GetComponent<AudioSource>().isPlaying == false)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if(gameObject.GetComponent<CanvasGroup>().alpha < 1)
            {
                gameObject.GetComponent<CanvasGroup>().alpha += 1 * Time.fixedDeltaTime;

            }
            else
            {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<audioManager>().SetTime();

                if (menuFade)
                {
                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    if(load == "next")
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
                    else
                    {
                        SceneManager.LoadScene(load);
                    }
                }
            }
        }
    }

    public void startFade(string levelName) //initiates the "fade", and once at full opacity loads the level passed in parameters
    {
        fadeOut = true;
        load = levelName;
    }
}
