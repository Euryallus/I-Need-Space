using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTrack : MonoBehaviour
{
    private GameObject rocket;
    private GameObject[] levelRestraints;

    private float cameraScale;

    private Vector2 posCameraBounds;
    private Vector2 negCameraBounds;
    private Vector2 posBoundAltered;
    private Vector2 negBoundAltered;

    private Vector3 newPosition;
    private Vector3 currentPos;
    private Vector3 targetPos;

    public float cameraBuffer = 10;
    public float driftSpeed = 5;
    public float cameraSpeedConst = 40f;

    private void Start()
    {
        rocket = GameObject.FindGameObjectWithTag("Player"); //sets rocket to gameObject found by searching tags
        
        levelRestraints = GameObject.FindGameObjectsWithTag("levelBounds"); //sets levelRestraints gameObject to object found searching tags

        for(int i = 0; i<levelRestraints.Length; i++) //cycles each item found with tag levelRestraints
        {
            if(levelRestraints[i].transform.position.x > 0) //if x is positive, must be positive bound
            {
                posCameraBounds = levelRestraints[i].transform.position; //set variable accordingly
            }
            if (levelRestraints[i].transform.position.x < 0) //if x is negative, must be negative bound
            {
                negCameraBounds = levelRestraints[i].transform.position;
            }
        }

        cameraScale = gameObject.GetComponent<Camera>().orthographicSize; //gets size of camera (distance from top to bottom)

        //calculates camera bounds using size, aspect ratio and the buffer
        //cameraScale * 1.8f converts vertical size to horizontal using 16:9 ratio knowledge
        //cameraBuffer is how far off the edges the camera stops (allows rocket to go completely off-screen before "dying")

        posBoundAltered = new Vector2(posCameraBounds.x - cameraBuffer - (cameraScale * 1.8f), posCameraBounds.y - cameraBuffer - cameraScale);
        negBoundAltered = new Vector2(negCameraBounds.x + cameraBuffer + (cameraScale * 1.8f), negCameraBounds.y + cameraBuffer + cameraScale);
        
        
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(rocket.transform.position.x, rocket.transform.position.y, gameObject.transform.position.z);
        }

        driftSpeed = Vector3.Magnitude(rocket.transform.position - gameObject.transform.position) * (cameraSpeedConst * Time.fixedDeltaTime); //speed camera catches up to player proportional to distance from rocket

        if(driftSpeed < rocket.GetComponent<Rigidbody2D>().velocity.magnitude) //if the driftspeed is < than the rockets velocity for any reason, adjust driftSpeed
        {
            driftSpeed = rocket.GetComponent<Rigidbody2D>().velocity.magnitude;
        }
        
        //sets currentPosition to transform.position for easy referencing
        currentPos = transform.position;

        //sets target position to roket position, sets z to be camera z so no Z axis movement takes place
        targetPos = rocket.transform.position;
        targetPos.z = currentPos.z;

        //moves the camera towards the rocket at driftSpeed (scales to frame-by-frame speed)
        transform.position = Vector3.MoveTowards(currentPos, targetPos, (driftSpeed * Time.fixedDeltaTime * 0.8f));

        //Mathf.Clamp takes current value and if it's more or less than the two bounds specified will make it equal to min or max value accordingly
        if(rocket.GetComponent<rocketLand>().won != true)
        {
            //RESEARCH DONE:
            //Answers.unity.com. 2013
            //How To Set Boundaries With Camera Movement - Unity Answers. 
            //[online] Available at: <https://answers.unity.com/questions/467192/how-to-set-boundaries-with-camera-movement.html> 
            //[Accessed 25 March 2020].

            transform.position = new Vector3( 
            Mathf.Clamp(transform.position.x, negBoundAltered.x, posBoundAltered.x), //Mathf.clamp allows value to be set to max or min value defined if value input is bigger or smaller than the bounds set
            Mathf.Clamp(transform.position.y, negBoundAltered.y, posBoundAltered.y), //means camera can't move outside of the bounds given
            transform.position.z
            );
        }
    }
}
