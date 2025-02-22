using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public enum PlayerStates
{
    Still,
    Run,
    Jump,
    Midair,
    Falling
}

public class PlayerMovement_Level1: MonoBehaviour
{
    /* -- Naming Convention:    Private: m_Variable     Public: g_Variable -- */
    
    //The player has a position that we must account for.
    Vector2 m_position;

    //The player also has a velocity to affect.
    Vector2 m_Velocity;

    //Remember to get the player's Rigidbody2D.
    Rigidbody2D m_Rb;

    //Save the state the player is in at all times.
    PlayerStates m_PlayerState;

    //Track if the player has hit the jump button.


    // Start is called before the first frame update
    void Start()
    {
        //Get the rigidbody component of the Player.
        m_Rb = GetComponent<Rigidbody2D>();
        
        //Start the Player in the still state.
        m_PlayerState = PlayerStates.Still;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player pressed the A or Left Arrow keys
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            //m_Rb.AddForce(new Vector2(-1.0f, 0.0f));
            gameObject.transform.Translate(Vector2.left * 5 * Time.deltaTime);
        }

        //If the player presses D or the Right Arrow key, apply a right force to them.
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            //m_Rb.AddForce(new Vector2(1.0f, 0.0f));
            gameObject.transform.Translate(Vector2.right * 5 * Time.deltaTime);
        }

        //Finally, if the Player presses W or Space, apply an upward force to them.
        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && 
            (m_PlayerState == PlayerStates.Still || m_PlayerState == PlayerStates.Run))
        {
            m_Rb.AddForce(new Vector2(0.0f, 20.0f));
        }
    }

    private void FixedUpdate()
    {
        //Get the velocity of the Player. 
        m_Velocity = m_Rb.velocity;

        //We will use Fixed Update as our Finite State Machine.

        //Player starts in the idle state, and can go to either Jump or Run.
        if(m_PlayerState == PlayerStates.Still)
        {
            //If the player gains an upward velocity, then switch them to Jumping.
            if(m_Velocity.y > 0.0f)
            {
                m_PlayerState = PlayerStates.Jump;
            }

            //If the player gains movement in the X-axis, switch them to Running.
            //They must not be moving in the Y-axis.
            if(m_Velocity.x != 0.0f && m_Velocity.y == 0)
            {
                m_PlayerState = PlayerStates.Run;
            }

            //While in Resting, print resting.
            Debug.Log("Player is Resting.");
        }

        //When a Player goes into Run, they can go to Idle or Jump.
        else if(m_PlayerState == PlayerStates.Run)
        {
            //If the X velocity is less than 0.2 in either direction, then they will return to Resting.
            if(m_Velocity.x < 0.5f && m_Velocity.x > -0.5f)
            {
                m_PlayerState = PlayerStates.Still;
            }

            //If the Y velocity increases, then they are Jumping.
            if (m_Velocity.y > 0.1f)
            {
                m_PlayerState = PlayerStates.Jump;
            }

            //If the Player is running and starts going down, then they are falling.
            else if(m_Velocity.y < 0.0f)
            {
                m_PlayerState = PlayerStates.Falling;
            }

            //While Running, pring running
            Debug.Log("Player is Running");
        }

        //When a Player is Jumping, they are moving upwards. 
        //When they stop moving up, and start moving down, they are falling.
        else if(m_PlayerState == PlayerStates.Jump)
        {
            //If the Y velocity is less than 0, then they will be Falling.
            if(m_Velocity.y < 0.0f)
            {
                m_PlayerState = PlayerStates.Falling;
            }

            Debug.Log("Player is Jumping");
        }

        //While a Player is Falling, if they stop moving down, then they are Resting.
        else if(m_PlayerState == PlayerStates.Falling)
        {
            if(m_Velocity.y == 0.0f)
            {
                m_PlayerState = PlayerStates.Still;
            }

            Debug.Log("Player is Falling");
        }

        ////While the player is moving down at all, the player is in a falling state.
        //if(m_Velocity.y < 0.0)
        //{
        //    m_PlayerState = PlayerStates.Falling;
        //    Debug.Log("Player is falling");
        //}

        ////While the player is not moving up or down, but is moving along the X-axis,
        ////then the Player is running.
        //else if(m_Velocity.y == 0 && (m_Velocity.x > 0.5 || m_Velocity.x < -0.5))
        //{
        //    m_PlayerState = PlayerStates.Run;
        //    Debug.Log("Player is running");
        //}

        ////While the player is moving up, then they are jumping.
        //else if(m_Velocity.y > 0.5)
        //{
        //    m_PlayerState = PlayerStates.Jump;
        //    Debug.Log("Player is jumping");
        //}

        ////Finally, if we are not moving at all, then we are still.
        //else
        //{
        //    m_PlayerState = PlayerStates.Still;
        //    Debug.Log("Player is resting");
        //}
    }

    // ------------------ This is demo code ---------------------- //


    // ----------------------------------------------------------- //
}
