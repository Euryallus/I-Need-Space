using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class rocketLaunch : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;

    private Vector2 mouseDirection;
    private Vector2 launchDirection;
    private Vector3 mousePosition;
    private Vector2 forceToAdd;

    private scoreManager sm;

    private float mouseDist;

    public bool playing = false;
    public bool initialLaunch = false;

    public float forceConst = 0.6f;
    public float maxForce = 45f;
    public float forceMag;

    public Rigidbody2D rb;
    public Camera camera;

    public LineRenderer launchLine;

    public Vector3 lookAt;
    public Vector3 lineEnd;
    public Vector3 lineStart;
    public Vector3 initialPos;

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); //gets camera object by tag
        launchLine = gameObject.GetComponent<LineRenderer>(); //gets lineRenderer component from self
        launchLine.positionCount = 0; //sets number of points on lineRenderer to 0 
        rb = gameObject.GetComponent<Rigidbody2D>(); //gets RigidBody2D component from self
        initialPos = transform.position;
        sm = gameObject.GetComponent<scoreManager>();
    }
    public void launchRocket(Vector3 start, Vector3 end)
    {
        mouseDirection = (start - end).normalized; //gets directional vector between the rocket and the mouse
        mouseDist = Vector2.Distance(start, end); //calculates distance between mouse and rocket

        launchDirection = -mouseDirection; //sets direction of launch to direction of mouse

        forceMag = mouseDist * forceConst; //calcuates force to be applied (proportional to mouse distance from rocket)

        if (forceMag > maxForce) //caps force added to maxForce (50 at time of writing)
        {
            forceMag = maxForce; //if forceMag is greater than maxForce, replace with maxForce
        }

        forceToAdd = launchDirection * forceMag; //times forceMag by directional vector

        gameObject.GetComponent<Rigidbody2D>().velocity = forceToAdd; //adds force to rigidbody
        gameObject.GetComponent<rocketLand>().trail.SetActive(true);


        gameObject.GetComponents<AudioSource>()[2].Play();

        gameObject.GetComponent<rocketLand>().count = 0; //resets count to 0

        if (initialLaunch == false)
        {
            maxForce = maxForce * 1.3f;
        }

        initialLaunch = true;

        sm.addScore();
    }

    void Update()
    {
        if(playing == false && gameObject.GetComponent<rocketLand>().won == false && GameObject.FindGameObjectWithTag("MainCamera").GetComponent<menuDisplay>().menuOn == false)
        {
            if (Input.GetMouseButtonDown(0)) //when the left mouse button is first pressed
            {
                //sets number of positions in lineRenderer to 2
                launchLine.positionCount = 2;
                Time.timeScale = 0.4f;
                startPos = transform.position; //stores start position of launch to rocket position
                lineStart = transform.position; //sets lineStart to rocket position
                
                launchLine.SetPosition(0, lineStart); //makes 1st point in lineRenderer to the position of the rocket
            }

            if (Input.GetMouseButtonUp(0)) //when left mouse button is released
            {
                endPos = camera.ScreenToWorldPoint(Input.mousePosition); //sets endPosition of launch to current mousePositon (converted from screen to worldPoint)
                Time.timeScale = 1; //unpauses game

                playing = true; //sets game started check to true
                launchLine.positionCount = 0; //resets lineRenderer to 0 points
                gameObject.GetComponent<planetGravity>().resetMass();
                launchRocket(startPos, endPos);

                gameObject.GetComponent<rocketLand>().landed = false;
                rb.freezeRotation = false;
            }

            if (Input.GetMouseButton(0)) //if mouseButton is down from previous frame
            {
                lineEnd = camera.ScreenToWorldPoint(Input.mousePosition); //resets line end position to current mouse position
                lookAt = Input.mousePosition; //sets lookAt point to mouse position
                lineEnd.z = transform.position.z; //sets lineEnd to same Z co-ord as rocket

                mouseDist = Vector2.Distance(lineEnd, lineStart); //calculates distance from mouse to rocket
                mouseDirection = (lineEnd - transform.position).normalized; //sets mouseDirection to vector direction between mouse and rocket

                forceMag = mouseDist * forceConst; //calcuates possible launch force from distance and force constant

                if (forceMag > maxForce) //if predicted launch force is > max force
                {
                    lineEnd = new Vector2(transform.position.x, transform.position.y) + (mouseDirection * (maxForce / forceConst)); //sets lineEnd to max force distance * mouse-to-rocket direction
                }
                
                launchLine.SetPosition(1, lineEnd); //sets lineRenderer end to lineEnd co-ord

                if(initialLaunch != true)
                {
                    lookAtMouse(); //calls lookAtMouse
                }

                startPos = transform.position;
                lineStart = transform.position;
                launchLine.SetPosition(0, lineStart);

            }

            if (initialLaunch != true)
            {
                transform.position = initialPos; //ensures rocket velocity remains 0 until mouse is released
                rb.velocity = new Vector2(0, 0);
            }
        }
    }

    void lookAtMouse()
    {
        Vector3 rocketPos = new Vector3(transform.position.x, transform.position.y, 0); //sets rocket position z to 0
        Vector3 difference = camera.ScreenToWorldPoint(lookAt) - rocketPos; //finds vector3 difference between rocket and mouse input

        float Rotate = (Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg); //finds angle between rocket and mouse

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -Rotate)); //sets z rotation to angle found
    }

}
