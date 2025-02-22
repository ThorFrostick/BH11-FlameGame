using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

enum PlayerStates
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
    Vector3 m_position;

    //The player also has a velocity to affect.
    Vector3 m_velocity;

    //Remember to get the player's Rigidbody2D.
    Rigidbody2D m_Rb;


    // Start is called before the first frame update
    void Start()
    {
        //Get the rigidbody component of the Player.
        m_Rb = GetComponent<Rigidbody2D>();
        
        //Start the Player in the still state.
        PlayerStates m_PlayerState = PlayerStates.Still;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ------------------ This is demo code ---------------------- //


    // ----------------------------------------------------------- //
}
