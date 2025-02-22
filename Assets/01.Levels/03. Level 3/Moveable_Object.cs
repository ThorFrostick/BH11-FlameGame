using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Moveable_Object : MonoBehaviour
{
    [SerializeField] private float m_maxSpeed;
    [SerializeField] private List<GameObject> m_waypoints;
    
    [Range(0,1)]
    public float g_perSpeed;

    private float m_speed
    {
        get { return m_speed * g_perSpeed; }
    }

    private int m_count;

    
    private bool m_isForward;


    private void Update()
    {
        Move();
    }

    /// <summary>
    /// Move the movable object back and forth
    /// </summary>
    private void Move()
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
    

    
}
