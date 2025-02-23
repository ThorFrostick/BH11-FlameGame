using System;
using _00.Scripts.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private bool m_isGizmos;
    [SerializeField] private string m_targetSceneName;
    [SerializeField] private float m_width;
    [SerializeField] private float m_height;

    public void Update()
    {
        
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(m_width, m_height), 0);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                LoadTargetScreen();
                return;
            }
        }
    }
    

    void LoadTargetScreen()
    {
        // Load the target scene specified in the editor
        if (!string.IsNullOrEmpty( m_targetSceneName))
        {
            SceneManager.LoadScene( m_targetSceneName);
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogWarning("Target scene is not set.");
        }
#endif

    }

    public void OnDrawGizmos()
    {
        if (!m_isGizmos) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector2(m_width, m_height));
    }
}