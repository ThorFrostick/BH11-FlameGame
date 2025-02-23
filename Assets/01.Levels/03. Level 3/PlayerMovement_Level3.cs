using System.Collections;
using System.Collections.Generic;
using _00.Scripts.Manager;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement_Level3 : MonoBehaviour, IResetable
{
    private Rigidbody2D m_rigidBody;
    
    [Header("Debug")] 
    [SerializeField] private bool m_isGizmos;
    
    [Header("Detection")]
    [SerializeField] private LayerMask m_groundLayer;
    [SerializeField] private LayerMask m_moveablePlatformLayer;
    
    [SerializeField] private float m_groundCheckDist = 0.5f;
    [SerializeField] private float m_airCheckDistance = 2f;
    [SerializeField] private Vector3 m_groundCheckOffset;
    [SerializeField] private float m_groundCheckWidth = 0.5f;
    
    [Header("Movement")]
    [SerializeField] private float m_maxSpeed = 10f;
    [SerializeField] private float m_acceleration = 20f;
    [SerializeField] private float m_deceleration = 25f;
    [SerializeField] private float m_jumpForce = 10f;
    [SerializeField] private float m_linearDrag = 2f;
    
    [Header("Advanced Movement")]
    [SerializeField] private float m_coyoteTime = 0.2f;
    [SerializeField] private float m_jumpBufferTime = 0.2f;
    [SerializeField] private float m_airControlFactor = 0.5f;
    [SerializeField] private float m_fastFallMultiplier = 2.5f;
    [SerializeField] private float m_gravityScale = 1f;
    [SerializeField] private float m_jumpHoldTime = 0.3f;
    [SerializeField] private float m_jumpBoostMultiplier = 2.0f;
    
    private PlayerStates m_currentState;
    private bool m_isFaceRight;
    private float m_coyoteTimeCounter;
    private float m_jumpBufferCounter;
    private float m_jumpTimeCounter;
    private bool m_isJumping;
    
    //Animation
    private Animator m_animator;
    private bool m_faceRight;
    
    private Collider2D m_collider2D;
    
    //Reset
    private PlayerStates m_resetState;
    private Vector2 m_resetPosition;
    
    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_collider2D = GetComponent<Collider2D>();
        m_animator = GetComponent<Animator>();
        
        m_currentState = PlayerStates.Still;
        
        m_resetPosition = this.transform.position;
        m_resetState = PlayerStates.Still;
    }

    private void Update()
    {
        HandleTimers();
        StateMachine();
        FLip();
    }

    private void FixedUpdate()
    {
        ApplyGravityModifiers();
    }

    private void HandleTimers()
    {
        if (OnGround())
            m_coyoteTimeCounter = m_coyoteTime;
        else
            m_coyoteTimeCounter -= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Space))
            m_jumpBufferCounter = m_jumpBufferTime;
        else
            m_jumpBufferCounter -= Time.deltaTime;
    }

    private void StateMachine()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        bool jump = m_jumpBufferCounter > 0 && m_coyoteTimeCounter > 0;

        if (moveX > 0)
        {
            m_faceRight = true;
        }else if (moveX < 0)
        {
            m_faceRight = false;
        }
        
        switch (m_currentState)
        {
            case PlayerStates.Still:
                if (jump)
                {
                    Jump();
                    m_currentState = PlayerStates.Jump;
                }
                else if (Mathf.Abs(moveX) > 0)
                {
                    m_currentState = PlayerStates.Run;
                }
                
                m_animator.Play("Player_Idle");
                break;
            
            case PlayerStates.Run:
                if (Mathf.Approximately(moveX, 0))
                {
                    m_currentState = PlayerStates.Still;
                }
                else if (jump)
                {
                    Jump();
                    m_currentState = PlayerStates.Jump;
                }
                Move(moveX);
                m_animator.Play("Player_Walk");
                break;
                
            case PlayerStates.Jump:
                if (!OnGround() && FarGround())
                {
                    m_currentState = PlayerStates.Midair;
                }
                m_animator.Play("Player_Jump");
                if (Input.GetKey(KeyCode.Space) && m_jumpTimeCounter > 0)
                {
                    m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, m_jumpForce * m_jumpBoostMultiplier);
                    m_jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    m_isJumping = false;
                }

                if (OnGround() && !FarGround())
                {
                    m_currentState = PlayerStates.Still;
                }
                
                
                break;
                
            case PlayerStates.Midair:
                if (m_rigidBody.velocity.y < 0)
                {
                    m_currentState = PlayerStates.Falling;
                }
                m_animator.Play("Player_MidAir");
                break;
                
            case PlayerStates.Falling:
                if (OnGround())
                {
                    m_currentState = PlayerStates.Still;
                }
                m_animator.Play("Player_MidAir");
                break;
        }
    }

    private void Move(float moveX)
    {
        float targetSpeed = moveX * m_maxSpeed;
        float acceleration = Mathf.Abs(moveX) > 0 ? m_acceleration : m_deceleration;

        float factor = OnGround() ? 1f : m_airControlFactor;
        float speedDifference = targetSpeed - m_rigidBody.velocity.x;
        float movement = speedDifference * acceleration * factor * Time.deltaTime;
        
        m_rigidBody.velocity += new Vector2(movement, 0);
    }

    private void Jump()
    {
        m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x * 1.2f, 0);
        m_rigidBody.AddForce(new Vector2(0, m_jumpForce), ForceMode2D.Impulse);
        m_coyoteTimeCounter = 0;
        m_jumpBufferCounter = 0;
        m_isJumping = true;
        m_jumpTimeCounter = m_jumpHoldTime;
    }

    private void ApplyGravityModifiers()
    {
        if (m_rigidBody.velocity.y < 0)
        {
            m_rigidBody.gravityScale =m_fastFallMultiplier;
        }
        else if (m_rigidBody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            m_rigidBody.gravityScale = m_gravityScale * 3f;
        }
        else
        {
            m_rigidBody.gravityScale = m_gravityScale;
        }
    }

    private void FLip()
    {
        if (m_faceRight)
        {
            this.transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0,180,0);
        }
        
    }

    
    private bool OnGround()
    {
        bool onMoveableObj = false;
        Collider2D[] collider2Ds =
            Physics2D.OverlapBoxAll(this.transform.position + m_groundCheckOffset, new Vector2(m_groundCheckWidth, m_groundCheckDist), 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer(Constant.g_moveablePlatform))
            {
                onMoveableObj = true;
                this.transform.SetParent(collider.transform);
            }
        }

        if (!onMoveableObj)
        {
            this.transform.SetParent(null);
        }
        return Physics2D.BoxCast(this.transform.position + m_groundCheckOffset, new Vector2(m_groundCheckWidth, m_groundCheckDist), 0f, Vector2.down,m_groundCheckDist , m_groundLayer)
            || onMoveableObj;
    }

    private bool FarGround()
    {
        Bounds bounds = m_collider2D.bounds;
        return !Physics2D.BoxCast(this.transform.position + m_groundCheckOffset, new Vector2(m_groundCheckWidth, m_airCheckDistance), 0f, Vector2.down,m_airCheckDistance , m_groundLayer);
    }

    #if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (!m_isGizmos) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position + m_groundCheckOffset, new Vector3(m_groundCheckWidth, m_groundCheckDist));

        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(this.transform.position + m_groundCheckOffset, new Vector3(m_groundCheckWidth, m_airCheckDistance));

        Handles.color = Color.white;
        Handles.Label(transform.position + Vector3.up * 2, m_currentState.ToString());
    }
    #endif
    public void Reset()
    {
        m_currentState = m_resetState;
        this.transform.position = m_resetPosition;
    }
}
