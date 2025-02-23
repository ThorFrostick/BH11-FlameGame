using _00.Scripts.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private string m_targetSceneName;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Constant.g_playerTag))
        {
            Debug.Log("Log new scene");
            LoadTargetScreen();
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
}