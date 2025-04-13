using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Goomba : MovingGameObject
{
    Rigidbody2D     m_rgb2D;
    float           m_speedMove;
    Animator        m_anim;
    Collider2D[]    m_colliders;
    bool            m_isAlive;

    Player          m_player;

    //! Crash Into
    bool            m_isCrashIntoDie;
    Vector3         m_startpos;
    float           m_deltaTime;
    float           m_angleCrashInto;

    ScoreKeeper     m_scoreKeeper;
    private void Awake() 
    {

        m_player            = FindObjectOfType<Player>();

        m_anim              = GetComponent<Animator>();
        m_rgb2D             = GetComponent<Rigidbody2D>();
        m_colliders         = GetComponents<Collider2D>();

        m_isAlive           = true;
        m_speedMove         = Config.m_GoombaSpeedMove;
        m_rgb2D.velocity    = new Vector2(m_speedMove, 0f);

        m_isCrashIntoDie    = false;
        m_scoreKeeper       = FindObjectOfType<ScoreKeeper>();
        
    }

    void Update()
    {
        if(m_isCrashIntoDie)
        {
            
            Vector3 _desPos = Vector3.zero;
            Utility.ObliqueProjectileMotion(ref m_startpos, ref _desPos, m_angleCrashInto, 8f, m_deltaTime);

            m_deltaTime += Time.deltaTime;
            transform.position = _desPos;

            //! Need rotate gameobject to make funny ^.^

            return;
        }
        
        if(m_isAlive)
            m_rgb2D.velocity = new Vector2(m_speedMove * transform.localScale.x,  
                                            m_rgb2D.velocity.y);

        // else 
        // {
        //     //m_rgb2D.velocity = Vector2.zero;
        //     m_rgb2D.bodyType = RigidbodyType2D.Static;
        // }
    }

    

    IEnumerator IEDie()
    {
        m_isAlive = false;

        m_anim.SetBool("isDie", true);
        yield return new WaitForSeconds(Config.m_GoombaTimeDie);

        if(m_scoreKeeper != null)m_scoreKeeper.collectScore(Config.m_GoombaScore);
        Destroy(gameObject);
    }


    public override void CrashIntoDie()
    {
        
        Debug.Log("Goomba::CrashIntoDie");
        m_deltaTime         = 0f;
        m_isCrashIntoDie    = true;
        //m_rgb2D.velocity    = Vector3.zero;
        m_startpos          = transform.position;
        
        m_angleCrashInto    = Config.GetAngleCrashInto();
        
        if(m_scoreKeeper != null)m_scoreKeeper.collectScore(Config.m_GoombaScore);
        
        DeactivePhysic();
    }

    
    void DeactivePhysic()
    {
        m_rgb2D.gravityScale    = 0;

        for(int i = 0; i < m_colliders.Count(); i++)
            m_colliders[i].enabled = false;

    }



    public override void DieByBulletPlayer()
    {
        if(m_scoreKeeper != null)m_scoreKeeper.collectScore(Config.m_GoombaScore);
        Destroy(gameObject);
    }




    public override void BeStomped()
    {
        base.BeStomped();
        StartCoroutine(IEDie());
    }



}
