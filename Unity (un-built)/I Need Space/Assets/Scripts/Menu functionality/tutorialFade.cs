using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialFade : MonoBehaviour
{
    //
    // Used only on the Tutorial gameObject for level1
    //

    private rocketLaunch rocket;
    private CanvasGroup canvas;

    private bool prevState = false;
    private bool thisState = false;

    private void Start()
    {
        rocket = GameObject.FindGameObjectWithTag("Player").GetComponent<rocketLaunch>(); //defines player using tag finding
        canvas = gameObject.GetComponent<CanvasGroup>(); //defines canvasGroup
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        thisState = rocket.initialLaunch; //sets bool thisState to initial launch value

        if(thisState != prevState) //if this state is different from last frame, player has just launched
        {
            canvas.alpha -= 1 * Time.fixedDeltaTime; //reduce alpha value every frame over 1 second
        }
        else
        {
            prevState = thisState;
        }

        if(canvas.alpha == 0)
        {
            Destroy(gameObject.transform.parent.gameObject); //once reaches 0 alpha destroy object
        }

    }
}
