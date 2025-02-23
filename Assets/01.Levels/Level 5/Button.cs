using System.Collections.Generic;
using UnityEngine;

namespace _01.Levels.Level_5
{
    public abstract class Button : MonoBehaviour
    {
        [Header("Debug")] 
        [SerializeField] protected bool m_isGizmos;
        [SerializeField] protected float m_width;
        [SerializeField] protected float m_height;
   
        [Header("Colors")] 
        [SerializeField] protected Color m_normalColor;
        [SerializeField] protected Color m_selectedColor;
        [SerializeField] protected Color m_highlightedColor;
    
        [SerializeField] protected List<GameObject> m_affectedObjects;

        protected Vector2 m_mousPos;
    
        protected SpriteRenderer m_spriteRenderer;


        protected virtual void Start()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }
        protected virtual void Update()
        {
            bool isClicked = Input.GetMouseButton(0);
            if (isClicked && isInsideRectangle())
            {
                m_selectedColor.a = 1;
                m_spriteRenderer.color =  m_selectedColor;
                OnClicked();
            }
            else
            {
                if (isInsideRectangle())
                {
                    m_highlightedColor.a = 1;
                    m_spriteRenderer.color =  m_highlightedColor;
                }
                else
                {
                    m_normalColor.a = 1;
                    m_spriteRenderer.color =  m_normalColor;
                }
            }
        }

        public abstract void OnClicked();

    
        protected bool isInsideRectangle()
        {
            return
                (m_mousPos.x > this.transform.position.x - m_width / 2
                 && m_mousPos.x < this.transform.position.x + m_width / 2
                 && m_mousPos.y > this.transform.position.y - m_height / 2
                 && m_mousPos.y < this.transform.position.y + m_height / 2
                );
        }

#if UNITY_EDITOR
        protected void OnDrawGizmos()
        {
            if (!m_isGizmos) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector2(m_width, m_height));
        }
#endif
    }
}
