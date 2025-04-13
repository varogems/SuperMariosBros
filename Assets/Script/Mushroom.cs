using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mushroom : MovingGameObject
{
    [SerializeField] bool m_isPosion;
    Rigidbody2D m_rgb2D;
    CircleCollider2D m_flagRedirect;
    Player m_player;

    bool m_isAppear;
    float m_speedUp;
    float m_speedMove;

    void Awake() 
    {
        m_speedUp           = Config.m_MSpeedUp;
        m_speedMove         = Config.m_MSpeedMove;
        m_rgb2D             = GetComponent<Rigidbody2D>();
        m_rgb2D.velocity    = new Vector2(m_speedMove, 0f);
        m_player            = FindObjectOfType<Player>();
    }

    void Update()
    {

        if(m_isAppear)
            m_rgb2D.velocity = Vector2.up * m_speedUp;
        else
        //! Remain velocity with rigibody type dynamic.
        m_rgb2D.velocity = new Vector2(m_speedMove * transform.localScale.x,  
                                        m_rgb2D.velocity.y);
    }

    public void Appear()
    {
        StartCoroutine(IEAppear());
    }

    IEnumerator IEAppear()
    {
        m_isAppear       = true;
        m_rgb2D.bodyType = RigidbodyType2D.Kinematic;

        yield return new WaitForSeconds(.5f);
        
        m_isAppear          = false;
        m_rgb2D.bodyType    = RigidbodyType2D.Dynamic;
    }


    void OnCollisionEnter2D(Collision2D other) 
    {
        if((1 << other.gameObject.layer) == LayerMask.GetMask("Player"))
        {
            if(m_isPosion)m_player.ChangeForm(false);
            else m_player.ChangeForm(true);

            Destroy(gameObject);
        }
    }

    



}
