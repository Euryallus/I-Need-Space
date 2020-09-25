using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerDetector : MonoBehaviour
{
    private GameObject pauseMenu;
    private GameObject planetParticle;
    private ParticleSystem.MainModule planetParticleEffect;

    private float collisionCount = 100f;
    private float planetCollisionCount = 100f;
    public float ufoForce = 25;

    private bool hasPlayed = false;

    private Vector2 ufoDirection;

    private void Awake()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        planetParticle = GameObject.FindGameObjectWithTag("planetParticle");
        planetParticleEffect = planetParticle.GetComponent<ParticleSystem>().main; 
    }

    private void FixedUpdate()
    {
        collisionCount += Time.fixedDeltaTime;
        planetCollisionCount += Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionObj = collision.transform.gameObject;

        switch (collisionObj.tag)
        {
            case("portal"): //if collision is with the portal, teleport player & play audio
                if(collisionCount > 1f)
                {
                    collisionObj.GetComponent<portalTeleport>().returnLinkLocation(gameObject);
                    gameObject.GetComponents<AudioSource>()[1].Play();

                    collisionCount = 0;
                }
                break;

            case ("level"): //if the collision is with the level restaints, pause game and show pause menu
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                pauseMenu.GetComponent<AudioSource>().Play();
                break;     
        }
    }

    private void OnTriggerStay2D(Collider2D collision) //while colliding with the UFO, cause player to move towards the centre of the UFO
    {
        if (collision.CompareTag("UFO"))
        {
            ufoDirection = (collision.gameObject.transform.position - transform.position).normalized;
            gameObject.GetComponent<Rigidbody2D>().velocity = (ufoDirection * ufoForce);

            if (collision is EdgeCollider2D) //if collides with the UFO centre, end game
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                gameObject.GetComponent<rocketLaunch>().initialLaunch = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("planet"))
        {
            //if player collides with planet, and if last collision happened > 0.4 seconds ago, play collision audio

            AudioSource audio = gameObject.GetComponent<AudioSource>();
            if (hasPlayed == false && planetCollisionCount > 0.4f)
            {
                audio.Play();
                hasPlayed = true;
                planetCollisionCount = 0;
            }

            planetParticleEffect.startColor = collision.gameObject.GetComponent<SpriteRenderer>().color; //set planet collision particle colour to colour of planet collided with

            if(planetParticle.GetComponent<ParticleSystem>().isPlaying == false)
            {
                planetParticle.transform.position = gameObject.transform.position - (((gameObject.transform.position - collision.transform.position).normalized * gameObject.transform.localScale.x) / 2); //set particle location to point where rocket hits planet
                planetParticle.GetComponent<ParticleSystem>().Play(); //plays particle effect
            }
           
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (hasPlayed) //when planet collision ends, set hasPlayed to false
        {
            hasPlayed = false;
        }
    }
}
