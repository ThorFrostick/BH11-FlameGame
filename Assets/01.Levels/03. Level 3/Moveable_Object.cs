using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Moveable_Object : MonoBehaviour
{
    [SerializeField] private float m_maxSpeed;
    [SerializeField] private List<GameObject> m_waypoints;
    [SerializeField] private bool m_isAutoMove;

    [Range(0, 1)] public float g_perSpeed;
    [Range(0, 1)] public float g_perDistance;

    private Vector3 m_rangeBar;
    
    private float m_speed
    {
        get { return m_maxSpeed * g_perSpeed; }
    }

    public bool IsAuto
    {
        get{return m_isAutoMove;}
    }

    private int m_count;
    
    private Rigidbody2D m_rigidbody;

    
    private bool m_isForward;

    private void Start()
    {
        m_rangeBar = (m_waypoints[1].transform.position - m_waypoints[0].transform.position);
        
    }

    private void Update()
    {
        if (m_isAutoMove)
        {
            AutoMove();
        }
        else
        {
            MoveByMouse();
        }
     
    }

    /// <summary>
    /// AutoMove the movable object back and forth
    /// </summary>
    private void AutoMove()
    {
        if (Vector2.Distance(transform.position, m_waypoints[m_count].transform.position) > 0.5f)
        {
            Vector3 direction = (m_waypoints[m_count].transform.position - transform.position).normalized;
            transform.position += direction * m_speed * Time.deltaTime;
        }
        else
        {
            //Wrap around the list to avoid out bound
            if (m_count == m_waypoints.Count - 1)
            {
                m_count = 0;
            }
            else
            {
                m_count++;
            }
        }
    }

    /// <summary>
    /// Move by draging the position
    /// </summary>
    private void MoveByMouse()
    {
        this.transform.position = m_waypoints[0].transform.position + m_rangeBar* g_perDistance;
    }
    

    
}
