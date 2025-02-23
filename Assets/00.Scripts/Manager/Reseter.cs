using System.Collections;
using System.Collections.Generic;
using _00.Scripts.Manager;
using UnityEngine;

public class Reseter : MonoBehaviour
{
    public static Reseter Instance { get; private set; }

    [SerializeField] private bool m_isGizmos;
    [SerializeField] private GameObject m_playerObject;
    [SerializeField] private List<GameObject> m_restables;
    [SerializeField] private float m_fallThreshold = -10f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void Update()
    {
        if (m_playerObject.transform.position.y < m_fallThreshold)
        {
            foreach (GameObject restable in m_restables)
            {
                IResetable reset = restable.GetComponent<IResetable>();
                if (reset != null)
                {
                    reset.Reset();
                }
            }
           
        }
    }

    private void OnDrawGizmos()
    {
        if (!m_isGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(-100, m_fallThreshold), new Vector2(100, m_fallThreshold));
    }
    
}
