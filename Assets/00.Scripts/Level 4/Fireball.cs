using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    //Store the position of the Fireball.
    Vector2 position;

    //Get the starting waypoint.
    GameObject start;

    //Get the ending waypoint.
    GameObject end;

    //Check if the button was pressed or not.
    bool isPaused;
    
    // Start is called before the first frame update
    void Start()
    {
        start = GameObject.Find("Starting Waypoint");
        end = GameObject.Find("Ending Waypoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            if (isPaused)
            {
                isPaused = false;
                return;
            }
            else
            {
                isPaused = true;
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isPaused)
        {
            //Get the position every frame.
            position = gameObject.transform.position;

            //If the Fireball is beyond the end waypoint, move it back to the starting waypoint.
            if (position.x > end.transform.position.x)
            {
                position.x = start.transform.position.x;
            }

            //Increase the X-position by a substantial amount every frame.
            position.x += 2;
            gameObject.transform.position = position;
        }
    }
}
