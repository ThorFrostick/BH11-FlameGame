using System;
using System.Collections;
using System.Collections.Generic;
using _00.Scripts.Manager;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct SliderPoint
{
    public float SpeedPer;
    public GameObject GameObject;
}

[RequireComponent(typeof(SpriteRenderer))]
public class Slider : MonoBehaviour, IResetable
{
    [Header("Debug")]
    [SerializeField] private bool m_isGizmos;

    [Header("Settings")]
    [SerializeField] private Moveable_Object m_moveableObject;
    [SerializeField] private List<SliderPoint> m_sliderPoints = new List<SliderPoint>();
    [SerializeField] private float m_radius;

    
    [SerializeField] private Color m_highlightColor;
    [SerializeField] private Color m_selectedColor;
    [SerializeField] private Color m_normalColor;

    private SpriteRenderer m_spriteRenderer;
    private SliderPoint m_currentPoint;

    private Vector2 m_mousePos;
    private bool m_isDragging = false; // New flag for dragging
    private IResetable _resetableImplementation;
    
    //Reset
    private SliderPoint m_resetPoint;
    private float m_resetPerSpeed;
    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Initialize to the rightmost slider point
        SliderPoint cloestPoint = new SliderPoint();
        float minDistance = float.MaxValue;
        foreach (SliderPoint sliderPoint in m_sliderPoints)
        {
            float distance = Vector2.Distance(sliderPoint.GameObject.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                cloestPoint = sliderPoint;
            }
            
        }
        m_currentPoint = cloestPoint;
        m_moveableObject.g_perSpeed = m_currentPoint.SpeedPer;
        
        //Asign reset data
        m_resetPoint = cloestPoint;
        m_resetPerSpeed = m_currentPoint.SpeedPer;
    }

    private void Update()
    {
        m_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Start dragging if mouse button down and near the slider
        if (Input.GetMouseButtonDown(0) && isNear())
        {
            m_isDragging = true;
        }

        // Stop dragging on mouse button release and snap to nearest point
        if (Input.GetMouseButtonUp(0))
        {
            m_isDragging = false;
            RoundPositionToNearestPoint();
            if (m_moveableObject.IsAuto)
            {
                m_moveableObject.g_perSpeed = m_currentPoint.SpeedPer;
            }
           
        }

        // If dragging, move the slider
        if (m_isDragging)
        {
            m_spriteRenderer.color = m_selectedColor;

            float x = Mathf.Clamp(m_mousePos.x,
                m_sliderPoints[0].GameObject.transform.position.x,
                m_sliderPoints[m_sliderPoints.Count - 1].GameObject.transform.position.x);
            
            if (!m_moveableObject.IsAuto)
            {
                // Clamp the normalized value to the range of [0, 1]
                float normalizedValue = NormalizeValue(m_mousePos.x, m_sliderPoints[0].GameObject.transform.position.x,
                    m_sliderPoints[m_sliderPoints.Count - 1].GameObject.transform.position.x);
    
                // Ensure that the value stays within the 0-1 range
                m_moveableObject.g_perDistance = Mathf.Clamp01(normalizedValue);
            }


            transform.position = new Vector2(x, transform.position.y);
        }
        else
        {
            // If not dragging but the mouse is near, highlight the slider
            if (isNear())
            {
                m_spriteRenderer.color = m_highlightColor;
            }
            else
            {
                m_spriteRenderer.color = m_normalColor;
            }
            // Snap to the nearest slider point continuously when not dragging
            RoundPositionToNearestPoint();
        }
    }

    private void RoundPositionToNearestPoint()
    {
        float minDistance = float.MaxValue;
        Vector2 nearestPos = Vector2.zero;
        SliderPoint nearestPoint = new SliderPoint();
        foreach (SliderPoint sliderPoint in m_sliderPoints)
        {
            float distance = Vector2.Distance(sliderPoint.GameObject.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPos = sliderPoint.GameObject.transform.position;
                nearestPoint = sliderPoint;
            }
        }
        transform.position = nearestPos;
        m_currentPoint = nearestPoint;
    }
    
    float NormalizeValue(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    private bool isNear()
    {
        float distance = Vector2.Distance(transform.position, m_mousePos);
        return distance < m_radius;
    }

    #if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (!m_isGizmos) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_radius);
    }
    #endif
    public void Reset()
    {
        m_currentPoint = m_resetPoint;
        m_moveableObject.g_perSpeed = m_resetPerSpeed;
        this.transform.position = m_resetPoint.GameObject.transform.position;
    }
}
