using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TopEnemyBehavior : MonoBehaviour
{
    MovingGameObject    m_gameObjectScript;
    Collider2D          m_col2D;

    Player m_player;
    
    void Awake()
    {
        m_gameObjectScript  = transform.parent.GetComponent<MovingGameObject>();

        m_col2D             = GetComponent<Collider2D>();
        m_col2D.enabled     = false;

    }

    void Update()
    {
        
        if(m_player == null) m_player = FindObjectOfType<Player>();

        if(m_player != null)
            m_col2D.enabled = (m_player.transform.position.y - (m_col2D.transform.position.y + m_col2D.offset.y) > 0f) ? true : false;
    }


    private void OnCollisionEnter2D(Collision2D other) 
    {
        if((1 << other.gameObject.layer) == LayerMask.GetMask("Player"))
            m_gameObjectScript.BeStomped();
            
        
    }
}
