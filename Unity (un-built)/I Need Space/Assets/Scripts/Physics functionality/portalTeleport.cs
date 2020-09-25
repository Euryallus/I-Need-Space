using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalTeleport : MonoBehaviour
{
    private Vector3 linkedLocation;

    private float tagResetCount;

    public GameObject linked;
    public GameObject cameraPoint;

    public bool keepDirection;
    public float spinRate = 5;


    private void Start()
    {
        linkedLocation = linked.transform.position; //sets linkedLocation vector to location of linked portal given in inspector
    }

    private void FixedUpdate()
    {
        tagResetCount += 1 * Time.fixedDeltaTime; //updates "time since last teleport" counter

        if(linked.transform.parent.CompareTag("Untagged") && tagResetCount > 1.5f) //if the portal had is tag removed and time elapsed since teleport is > 1.5 seconds, re-assign the tag
        {
            //this prevents a weird effect where the rocket is attracted to the portal on the other side and gets sucked back in immediately
            linked.transform.parent.tag = "planet";
        }
    }

    public void returnLinkLocation(GameObject rocket) //called from the triggerDetect script, teleports the rocket to the location of it's linked portal
    {
        rocket.GetComponents<ParticleSystem>()[0].Play(); //play teleport effect on rocket

        linked.transform.parent.tag = "Untagged"; //un-tag linked rocket 
        tagResetCount = 0; //reset counter

        Rigidbody2D rocketRb = rocket.GetComponent<Rigidbody2D>();
        rocket.transform.position = linked.transform.parent.gameObject.transform.position + new Vector3(rocketRb.velocity.x * Time.fixedDeltaTime * 2, rocketRb.velocity.y * Time.fixedDeltaTime * 2, 0); //set teleport position to the location of linked portal + the position it'd be in a frame after (avoids hitting trigger on location portal)

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        camera.transform.position = new Vector3(cameraPoint.transform.position.x, cameraPoint.transform.position.y, camera.transform.position.z); //sets camera position to the cameraPoint assigned in inspector (usually close to the exit portal)

        if(keepDirection != true) //if the rocket should change direction when exiting the portal
        {
            float originalVelocity = rocketRb.velocity.magnitude; 
            Vector2 originalDirection = rocketRb.velocity.normalized;

            float portalRotation = linked.transform.parent.GetChild(0).gameObject.transform.rotation.x + 90; //calculates rotation rocket needs on exit based on the portal's rotation relitive to its own

            rocket.transform.rotation = Quaternion.FromToRotation(originalDirection.normalized, new Vector3(0,0,portalRotation)); //https://docs.unity3d.com/ScriptReference/Quaternion.FromToRotation.html //rotates the rocket from current rotation to exit rotation

            Vector2 newRotation = rocket.transform.rotation.eulerAngles;

            rocketRb.velocity = newRotation.normalized * originalVelocity; //sets velocity to original magnitude but going in the new direction
        }

        
    }
}
