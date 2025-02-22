using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement_Level3 : MonoBehaviour
{
    private Rigidbody2D m_rigidBody;

    [Header("Debug")] 
    [SerializeField] private bool m_isGizmos;
    
    [Header("Detection")]
    [SerializeField] private LayerMask m_groundLayer;

    // Use a shorter distance to check if we're grounded.
    [SerializeField] private float m_groundCheckDist = 0.5f;
    // Use a longer distance to determine if the ground is far below.
    [SerializeField] private float m_airCheckDistance = 2f;
    
    [Header("Movement")]
    [FormerlySerializedAs("speed")]
    [Tooltip("The default velocity of the player")]
    [SerializeField] private float m_speed = 10;
    [SerializeField] private float m_jumpForce = 10;

    [Tooltip("The linear drag")] 
    [SerializeField] private float m_linearDrag = 2;

    private PlayerStates m_currentState;

    private bool m_isFaceRight;
    private Vector2 m_prevPosition;
    
    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_currentState = PlayerStates.Still;
    }

    private void Update()
    {
        StateMachine();
    }

    private void FixedUpdate()
    {
        // Optional: any fixed update physics code can go here.
    }

    private void StateMachine()
    {
        float moveX = Input.GetAxis("Horizontal");
        bool jump = Input.GetKeyDown(KeyCode.Space);
        
        switch (m_currentState)
        {
            case PlayerStates.Still:
                if (jump && OnGround())
                {
                    m_prevPosition = transform.position;
                    Jump();
                    m_currentState = PlayerStates.Jump;
                }
                else if (Mathf.Abs(moveX) > 0 && OnGround())
                {
                    m_currentState = PlayerStates.Run;
                }
                break;
                
            case PlayerStates.Run:
                if (Mathf.Approximately(moveX, 0))
                {
                    m_currentState = PlayerStates.Still;
                }
                else if (jump && OnGround())
                {
                    m_prevPosition = transform.position;
                    Jump();
                    m_currentState = PlayerStates.Jump;
                    break;
                }
                Move();
                break;
                
            case PlayerStates.Jump:
                // Transition to midair if we are not grounded and the ground is far below.
                if (!OnGround() && FarGround())
                {
                    m_prevPosition = transform.position;
                    m_currentState = PlayerStates.Midair;
                }
                break;
                
            case PlayerStates.Midair:
                // Transition to falling when the player’s upward velocity turns negative.
                if (m_rigidBody.velocity.y < 0)
                {
                    m_currentState = PlayerStates.Falling;
                }
                break;
                
            case PlayerStates.Falling:
                if (OnGround())
                {
                    m_currentState = PlayerStates.Still;
                }
                break;
        }
    }

    #region Core Movement

    /// <summary>
    /// Move the player horizontally.
    /// </summary>
    private void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        if (Mathf.Approximately(moveX, -1))
        {
            m_isFaceRight = false;
        }
        else if (Mathf.Approximately(moveX, 1))
        {
            m_isFaceRight = true;   
        }
        
        Vector2 moveDirection = new Vector2(moveX, 0).normalized;
        m_rigidBody.velocity = new Vector2(moveDirection.x * m_speed, m_rigidBody.velocity.y);
        m_rigidBody.drag = m_linearDrag;
    }

    /// <summary>
    /// Apply upward force to make the player jump.
    /// </summary>
    private void Jump()
    {
        m_rigidBody.AddForce(new Vector2(0, m_jumpForce), ForceMode2D.Impulse);
    }

    #endregion

    #region Detection

    /// <summary>
    /// Check if the player is on the ground using a short raycast.
    /// </summary>
    private bool OnGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, m_groundCheckDist, m_groundLayer);
        return hit.collider != null;
    }

    /// <summary>
    /// Check if the player is far from the ground using a longer raycast.
    /// Returns true if no ground is detected within the specified distance.
    /// </summary>
    private bool FarGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, m_airCheckDistance, m_groundLayer);
        return hit.collider == null;
    }

    #endregion

    #if UNITY_EDITOR
    #region Debug

    public void OnDrawGizmos()
    {
        if (!m_isGizmos) return;
        
        // Draw a red line for the ground check.
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * m_groundCheckDist);
        
        // Draw a cyan line for the air check.
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * m_airCheckDistance);
        
        // Draw the current state above the player.
        Handles.color = Color.white;
        Handles.Label(transform.position + Vector3.up * 2, m_currentState.ToString());
    }
    
    #endregion  
    #endif
}
