using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemyBehavior : MonoBehaviour
{
    Player      m_player;
    Collider2D  m_collider2D;

    MovingGameObject m_gameObject;
    // Start is called before the first frame update
    void Awake()
    {
        m_player        = FindObjectOfType<Player>();
        m_collider2D    = GetComponent<Collider2D>();
        m_gameObject    = transform.parent.GetComponent<MovingGameObject>();
        
    }
    public void setTrigger(bool trigger = true)
    {
        m_collider2D.isTrigger = trigger;
    }


    // private void FixedUpdate() 
    private void Update() 
    {
        if(m_player == null)
            m_player = FindObjectOfType<Player>();
    }

    //! Down form player if trigger touch player.
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(m_collider2D.isTrigger && (1 << other.gameObject.layer) == LayerMask.GetMask("Player"))
            m_player.ChangeForm(false);
    }

    //! gameobject 's kicked if player touch/kich collision.
    void OnCollisionEnter2D(Collision2D collisionPlayer)
    {
        if(!m_collider2D.isTrigger && (1 << collisionPlayer.gameObject.layer) == LayerMask.GetMask("Player"))
            m_gameObject.BeKicked(collisionPlayer);
    }

}
