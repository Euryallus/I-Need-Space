using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class planetGravity : MonoBehaviour
{
    private Rigidbody2D rb; //rocket rigidbody
    private Rigidbody2D planetRb;

    private GameObject[] planetList; //array of planets in scene

    private float originalMass; //default mass of rocket
    private float defaultScale; //default Y scale of rocket

    private float planetDistance; //distance from planet to rocket
    private float planetMass; //mass of planet
    private float planetRadius; //radius of planet
    private float forceMag; //force to be applied to rocket
    private float smallestDistance = 75; //distance from rocket to closest planet

    private Vector2 distanceVector; 
    private Vector2 direction;
    private Vector3 lookAt;

    public GameObject closestPlanet; //closest planet (game object)

    public float closestRadius; //radius of closest planet
    public float gravConst = 60f; 
    public float rocketMass = 1.5f;
    public float stretchConst = 0.025f; //amount rocket stretches depending on speed

    public float closestForceMag; //saves magnitude of force exerted by closest object

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); //sets rb to rocket's rigidBpdy2D component
        defaultScale = transform.localScale.y; //sets default Y scale 
        Time.timeScale = 1;
        originalMass = rocketMass; //saves original mass assigned to rocket from inspector
    }

    private void Gravity() //causes force proportional to planet size to be applied to the rocket
    {
        planetList = GameObject.FindGameObjectsWithTag("planet"); //creates updated list of all planets in a scene (allows disappear and re-appear)

        //resets vital distance information each turn
        closestPlanet = null; 
        closestRadius = 0f;
        smallestDistance = 100f;

        for(int i = 0; i < planetList.Length; i++) //cycles all planets in scene each frame
        {
            planetRb = planetList[i].GetComponent<Rigidbody2D>(); //temp store of planet's rigidbody component

            if(planetRb.transform.localScale.x < 2) //if the object has a small scale (e.g. portal scale ~ 1.2)
            {
                planetMass = 350; //manually choose a mass
            }
            else
            {
                planetMass = planetRb.transform.localScale.x * 15; //sets mass propotional to planet size (scale)
            }

            planetRadius = planetList[i].transform.localScale.x / 2; //stores radius of planet (assuming it's circular)

            planetDistance = Vector2.Distance(gameObject.transform.position, planetList[i].transform.position); //gets distance between player and planet being inspeccted in for loop

            if(planetDistance - planetRadius < smallestDistance) //finds planet closest to the player at any given time
            {
                smallestDistance = planetDistance - planetRadius; //stores info about closest planet (distance and gameObject info)
                closestRadius = planetRadius;
                closestPlanet = planetList[i];
                closestForceMag = ((planetMass * rocketMass) / (planetDistance * (planetDistance / 1.2f))) * gravConst;
            }

            distanceVector = planetList[i].transform.position - gameObject.transform.position; //gets distance from planet to player
            direction = distanceVector.normalized; //gets direction from planet to player

            forceMag = ((planetMass * rocketMass) / (planetDistance * (planetDistance / 1.2f))) * gravConst; //calculates force to add by using grav equation ( (m1 * m2) / distance^2) * grav constant)

            if(forceMag > 650) //if the force mag. is too big (e.g. going through a portal would give a very small distance (~ 0.5) so force calculated using formula would be massive)
            {
                forceMag = 650; //sets force to a more managable size
            }

            rb.AddForce(direction * forceMag); //applies force calculated to the player in the direction of the planet
        }
    }

    private void FixedUpdate()
    {
        Gravity(); //calls gravity to be applied every frame

        if (smallestDistance > 2f) //checks if the rocket is further away than 2 units from planet's surface 
        {   
            //rotates rocket to face direction it's currently moving in
            Vector2 position2D = new Vector2(transform.position.x, transform.position.y); //gets vector2 of current rocket position
            Vector2 alteredVelociy = rb.velocity / Time.fixedDeltaTime; //transforms velocity to the distance travelled in one frame

            float Rotate = -(Mathf.Atan2(position2D.x + alteredVelociy.x, position2D.y + alteredVelociy.y) * Mathf.Rad2Deg); //calculates angle that velocity is moving in (0 to 360 from UP)

            Quaternion targetRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Rotate); //transforms angle to Quaternion

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10); //rotates towards the direction of velocity each Update
        }

        Vector3 newScale = new Vector3(transform.localScale.x, defaultScale + (rb.velocity.magnitude * stretchConst), transform.localScale.z); //sets y scale to be proportional to original scale * constant (causes "stretch" visual effect)
        transform.localScale = newScale; //sets new scale once it's calculated
    }

    public void increaseMass() //increases rockets mass (used on landing to ensure rocket "sticks" to planet surface)
    {
        rocketMass = rocketMass * 2;
    }

    public void resetMass() //resets rocket mass before lift-off
    {
        rocketMass = originalMass;
    }
}
