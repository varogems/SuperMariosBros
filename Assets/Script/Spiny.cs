using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spiny : MovingGameObject
{
    Rigidbody2D     m_rgb2D;
    float           m_speedMove;
    Animator        m_anim;
    Collider2D[]    m_colliders;

    bool            m_isTurnArround;
    
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

        m_speedMove         = Config.m_SpinySpeedMove;
        m_rgb2D.velocity    = new Vector2(m_speedMove, 0f);

        m_isTurnArround     = false;
        m_anim.SetBool("isEgg", true);

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

            return;
        }

        if(m_isTurnArround)
            m_rgb2D.velocity = Vector2.zero;
        else
            m_rgb2D.velocity = new Vector2(m_speedMove * transform.localScale.x,  
                                        m_rgb2D.velocity.y);
    }

    
    void OnCollisionEnter2D(Collision2D other) 
    {       

        if(((1 << other.gameObject.layer) == LayerMask.GetMask("Block")
        || (1 << other.gameObject.layer) == LayerMask.GetMask("Bridge")
        || (1 << other.gameObject.layer) == LayerMask.GetMask("Island")
        || (1 << other.gameObject.layer) == LayerMask.GetMask("TurtleCannon")
        || (1 << other.gameObject.layer) == LayerMask.GetMask("StairBlock")
        || (1 << other.gameObject.layer) == LayerMask.GetMask("Pipe")
        || (1 << other.gameObject.layer) == LayerMask.GetMask("Ground"))
        && (other.contacts[0].normal.y > 0))
                m_anim.SetBool("isEgg", false);

    
        if((1 << other.gameObject.layer) == LayerMask.GetMask("Player"))
            m_player.ChangeForm(false);


        // if(m_colliders[1].IsTouchingLayers(LayerMask.GetMask("Player")))
        //     StartCoroutine(IEDie());
            


    }

    public override void TurnArround()
    {
        StartCoroutine(IETurnArround());
    }

    IEnumerator IETurnArround()
    {
        m_isTurnArround = true;
        m_anim.SetBool("isTurnArround", m_isTurnArround);

        yield return new WaitForSeconds(Config.m_SpinyTimeTurnArround);
        
        m_isTurnArround = false;
        m_anim.SetBool("isTurnArround", m_isTurnArround);

        transform.localScale = new Vector3(transform.localScale.x * (-1),
                                    transform.localScale.y,
                                    transform.localScale.z);
    }

    public override void CrashIntoDie()
    {
        m_isCrashIntoDie    = true;
        m_rgb2D.velocity    = Vector3.zero;
        m_startpos          = transform.position;
        m_deltaTime         = 0f;
        
        m_angleCrashInto    = Config.GetAngleCrashInto();
        
        m_anim.SetBool("isEgg", true);

        if(m_scoreKeeper != null)m_scoreKeeper.collectScore(Config.m_SpinyScore);
        
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
        if(m_scoreKeeper != null)m_scoreKeeper.collectScore(Config.m_SpinyScore);
        Destroy(gameObject);
    }



}
