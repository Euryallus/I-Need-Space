using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketAutoLaunch : MonoBehaviour
{
    //
    // asigned to rocket seen in main menu background - fires at set trajectory without user input
    //

    private RaycastHit2D rch;

    private float landCount = 0;
    private bool confetti = false;

    private Vector2 launchDirection;
    private float launchMag;

    public GameObject launchTarget;

    private void Start()
    {
        Time.timeScale = 1;

        launchDirection = (launchTarget.transform.position-transform.position).normalized; //calculates direction dired using launch target location - current location
        launchMag = Vector3.Distance(transform.position, launchTarget.transform.position); //magnitude of launch calculated using distance between target & start pos of rocket

        gameObject.GetComponent<Rigidbody2D>().velocity = launchDirection * launchMag * 0.5f; //sets velocity to direction * magnitude * 0.1f
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (Physics2D.Raycast(gameObject.transform.position, (gameObject.GetComponent<Rigidbody2D>().velocity.normalized), 3f))
        {
            rch = Physics2D.Raycast(gameObject.transform.position, (gameObject.GetComponent<Rigidbody2D>().velocity.normalized), 3f); //raycasts in direction of rocket's velocity

            if (rch.transform.name == "PlanetFlag Variant") 
            {
                landCount += Time.fixedDeltaTime;
                if (landCount > 0.75f && confetti == false) //if raycast hits flag & time landed > 45 milliseconds & confetti hasn't fired
                {
                    rch.transform.gameObject.GetComponent<ParticleSystem>().Play(); //fire confetti
                    confetti = true;
                    gameObject.GetComponent<Rigidbody2D>().freezeRotation = true; //freeze rocket rotation
                    gameObject.GetComponent<planetGravity>().rocketMass = 2; //increase rocket mass by 2 (avoid jiggling when planet grows / shrinks as force is increased)
                }
            }
            else
            {
                landCount = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.gameObject.name == "background") //if rocket goes off-screen for any reason
        {
            Destroy(gameObject); //destroy itself
        }
    }
}
