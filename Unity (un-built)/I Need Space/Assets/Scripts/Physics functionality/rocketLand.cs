using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rocketLand : MonoBehaviour
{
    private string objectTag;
    private bool effectPlayed = false;
    private float originalGrav;

    private GameObject pauseMenu;
    private GameObject winMenu;
    private RaycastHit2D raycastHit;
    private Rigidbody2D rb;

    private scoreManager sm;

    public GameObject collideObj = null;
    public GameObject trail;
    public bool won;
    public bool landed = false;
    public float launchThreshold = 1f;
    public float count = 0;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); //sets rb to own rigidBody2D component
        winMenu = GameObject.FindGameObjectWithTag("WinMenu"); //finds win menu object by tag
        won = false;
        effectPlayed = false;
        sm = gameObject.GetComponent<scoreManager>(); //gets score manager component from self
    }


    private void FixedUpdate()
    {
        raycastHit = Physics2D.Raycast(gameObject.transform.position, (gameObject.GetComponent<planetGravity>().closestPlanet.transform.position - gameObject.transform.position).normalized, 3f); //assign that raycast data to raycastHit

        if (raycastHit == true && raycastHit.collider.isTrigger == false) //if raycast in direction of velocity hits something:
        {

            collideObj = raycastHit.transform.gameObject; //sets reference to object raycast hits

            if (collideObj.tag == ("planet")) //if the raycast hits a planet
            {
                count += Time.fixedDeltaTime; //increases seconds landed count by time between updates
                gameObject.GetComponent<rocketLaunch>().playing = false; //sets rocketLaunch playing bool to false (allows re-launch)

            }

            if (collideObj.GetComponent<planetOrbit>() != false && landed == true && (gameObject.GetComponent<planetOrbit>() == false || gameObject.GetComponent<planetOrbit>().enabled == false)) //if rocket lands on planet oribitting another planet, assign the rocket that same component
            {   //prevents rocket from being "dragged" along by planet when landed on orbitting planet

                if(gameObject.GetComponent<planetOrbit>() == false) //if the rocket DOESN'T already have an orbit component, give it one
                {
                    gameObject.AddComponent<planetOrbit>();
                }
                else
                {
                    gameObject.GetComponent<planetOrbit>().enabled = true; //if it has one, enable it
                }

                gameObject.GetComponent<planetOrbit>().anchor = collideObj.GetComponent<planetOrbit>().anchor; //set values of planetOrbit component = to that of the planet it's landed on
                gameObject.GetComponent<planetOrbit>().planetSpeed = collideObj.GetComponent<planetOrbit>().planetSpeed;

            }

            if (count > launchThreshold) //if rocket has been on planet for more than 1 second
            {
                if(landed == false){
                    land(); //call land
                }

                if (raycastHit != false)
                {
                    if (collideObj.name == "PlanetFlag Variant") //checks to see if collision object is the goal
                    {
                        Won(); //call Won
                    }
                }
            }
            else {
                collideObj = null;
            }
        }
        else if(raycastHit == false)//once raycast stops hitting planet, reset count and rotation lock
        {
            if (originalGrav != 0 && gameObject.GetComponent<planetGravity>().gravConst != originalGrav) //if grav. constant was changed on land, reset it once raycast stops hitting object
            {
                gameObject.GetComponent<planetGravity>().gravConst = originalGrav;
            }
            count = 0; //reset land count
            rb.freezeRotation = false; //unfreeze rotation (if frozen)

            if (gameObject.GetComponent<rocketLaunch>().initialLaunch == true)
            {
                gameObject.GetComponent<rocketLaunch>().playing = true; //sets rocketLaunch playing bool to true
            }

            if (gameObject.GetComponent<planetOrbit>() != false) //if planetOrbit component is still active, disable it as rocket is no longer landed on planet
            {
                gameObject.GetComponent<planetOrbit>().enabled = false;
            }

            gameObject.GetComponent<planetGravity>().resetMass(); //ensures rocket has original mass assigned instead of altered value

        }
    }

    private void land()
    {
        landed = true;
        gameObject.GetComponent<planetGravity>().increaseMass(); //increases rocket mass when landed to reduce unwanted movement
        trail.SetActive(false);
    }

    private void Won()
    {
        won = true;

        Time.timeScale = 1; //sets time to 1 
        gameObject.GetComponent<rocketLaunch>().playing = true; //sets playing to true (prevents pause menu being opened and rocket being re-launched)

        ParticleSystem planetPart = collideObj.GetComponent<ParticleSystem>(); //sets reference to planet's particle effect

        if (effectPlayed == false && count > 1.5f) //plays confetti effect if it hasnt already played
        {
            collideObj.GetComponents<AudioSource>()[0].Play(); //plays particle effect and SFX, and calls EndLevel from score manager
            planetPart.Play();
            effectPlayed = true;
            sm.endLevel();
        }

        if (count > 3f) //once it's been 3 seconds, game is won, show win menu
        {
            //Time.timeScale = 0;
            if(winMenu != null)
            {
                winMenu.SetActive(true); //activate winMenu if it isn't already active
            }

            int currentScene = SceneManager.GetActiveScene().buildIndex + 1; //increase levelUnlocked playerPrefs if it's not already at this level
            if (PlayerPrefs.GetInt("levelUnlocked") < currentScene)
            {
                PlayerPrefs.SetInt("levelUnlocked", currentScene);
            }
        }
    }

}
