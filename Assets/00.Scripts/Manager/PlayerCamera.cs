using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCamera : MonoBehaviour
{
    [Header("Debug")] 
    [SerializeField] private bool m_isGizmos;
    
    
    [Header("Camera Zones")]
    [SerializeField] private CameraZone[] m_cameraZones; 
    [SerializeField] private Transform m_player; 
    [SerializeField] private CinemachineVirtualCamera[] m_virtualCameras; 

    private void Update()
    {
        SwitchCameraBasedOnZone();
    }

    private void SwitchCameraBasedOnZone()
    {
        for (int i = 0; i < m_cameraZones.Length; i++)
        {
            // Check if the player is within the current zone
            if (m_player.position.x >= m_cameraZones[i].x1 && m_player.position.x <= m_cameraZones[i].x2)
            {
                m_virtualCameras[i].Priority = 10;
                
                for (int j = 0; j < m_virtualCameras.Length; j++)
                {
                    if (j != i) m_virtualCameras[j].Priority = 0;
                }

                return; 
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!m_isGizmos || m_cameraZones == null || m_virtualCameras == null) return;
        
        Gizmos.color = Color.red;
        foreach (CameraZone zone in m_cameraZones)
        {
            Gizmos.DrawLine(new Vector2(zone.x1, -30), new Vector2(zone.x1, 50));
            Gizmos.DrawLine(new Vector2(zone.x2, -30), new Vector2(zone.x2, 50));

        }
    }

    [System.Serializable]
    public class CameraZone
    {
        public float x1; // Start of the zone
        public float x2; // End of the zone
    }
}
