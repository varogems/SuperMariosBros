using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class KoopaTroopa1 : MovingGameObject
{
    Rigidbody2D     m_rgb2D;
    
    Animator        m_animKoopaTroopa;
    GameObject      m_gameobjWing;

    Collider2D[]    m_colliders;
    bool            m_isActive;
    bool            m_beKicked;
    float           m_speedMove;
    float           m_speedFly;
    float           m_maxHeightFly;
    float           m_timeActive;
    float           m_speedBekicked;

    bool            m_canFly = false;
    [SerializeField] bool m_hasWing = false;

    Vector2         m_startPosition;

    bool            m_isTurnArround;
    [SerializeField] bool            m_isRedKoopaTroopa = false;


    //! Crash Into
    bool            m_isCrashIntoDie;
    Vector3         m_startpos;
    float           m_deltaTime;
    float           m_angleCrashInto;

    ScoreKeeper     m_scoreKeeper;
    AudioManager    m_audioManager;
    bool            m_isDangerWithPlayer;
    Coroutine       m_CRActive;
    bool            m_standPlatform;
    
    

    private void Awake() 
    {
        m_rgb2D             = GetComponent<Rigidbody2D>();
        m_animKoopaTroopa   = gameObject.GetComponentsInChildren<Animator>()[0];
        m_gameobjWing       = gameObject.transform.GetChild(1).gameObject;
        m_colliders         = GetComponents<Collider2D>();

        m_speedMove         = Config.m_KTSpeedMove;

        m_canFly            = false;
        m_speedFly          = Config.m_KTSpeedFly;
        m_maxHeightFly      = Config.m_KTMaxHeightCanFly;
        
        m_isTurnArround     = false;

        m_isActive          = true;
        m_timeActive        = Config.m_KTTimeActive;

        m_beKicked          = false;
        m_speedBekicked     = Config.m_KTSpeedBeKicked;
        
        m_isCrashIntoDie    = m_isDangerWithPlayer = false;
        

        m_scoreKeeper       = FindObjectOfType<ScoreKeeper>();
        m_audioManager      = FindObjectOfType<AudioManager>();

        m_standPlatform     = false;
    }


    void OnTriggerEnter2D(Collider2D other) 
    {
        //! Redirection with obstacle
        if(m_colliders[1].IsTouchingLayers(LayerMask.GetMask("Block"))
        || m_colliders[1].IsTouchingLayers(LayerMask.GetMask("StairBlock"))
        || m_colliders[1].IsTouchingLayers(LayerMask.GetMask("TurtleCannon"))
        || m_colliders[1].IsTouchingLayers(LayerMask.GetMask("Pipe")))
        {
            if(m_beKicked)
            {        
                transform.localScale = new Vector3(transform.localScale.x * (-1),
                                    transform.localScale.y,
                                    transform.localScale.z);
                return;
            }
            StartCoroutine(IETurnArround());
        }


        //! Redirection with Enemy
        if(m_colliders[1].IsTouchingLayers(LayerMask.GetMask("Spiny"))
        || m_colliders[1].IsTouchingLayers(LayerMask.GetMask("LevelMushroom"))
        || m_colliders[1].IsTouchingLayers(LayerMask.GetMask("Green Koopa Troopa"))
        || m_colliders[1].IsTouchingLayers(LayerMask.GetMask("Goomba")))
            StartCoroutine(IETurnArround());

        //! Fly
        if(m_hasWing)
            if(m_colliders[3].IsTouchingLayers(LayerMask.GetMask("Block"))
            || m_colliders[3].IsTouchingLayers(LayerMask.GetMask("Bridge"))
            || m_colliders[3].IsTouchingLayers(LayerMask.GetMask("StairBlock"))
            || m_colliders[3].IsTouchingLayers(LayerMask.GetMask("Pipe"))
            || m_colliders[3].IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                m_canFly = true;
                m_startPosition = transform.position;
                // m_standPlatform = true;
            }


    }

    public override void BeStomped()
    {
        Debug.Log("Koopa BeStomped");
        if(m_hasWing)
        {
            m_hasWing = false;
            m_rgb2D.velocity = Vector2.down * (50);
            m_audioManager.playEffectAudio(AudioManager.EffectAudio.KoopaShell);
            return;
        }

        if(!m_hasWing)
        {
            //!Recede into their shells when player stomp
            if(m_isActive)
            {
                m_isActive = false;
                m_animKoopaTroopa.SetBool("isActive", false);
                m_animKoopaTroopa.SetBool("isMove", false);
                m_audioManager.playEffectAudio(AudioManager.EffectAudio.KoopaShell);

                m_CRActive = StartCoroutine(IEActive());
                return;
            }
        }
    }

    // void OnTriggerStay2D(Collider2D other)
    // {
    //     //! Redirection
    //     if(m_colliders[1].IsTouchingLayers(LayerMask.GetMask("Block"))
    //     || m_colliders[1].IsTouchingLayers(LayerMask.GetMask("StairBlock"))
    //     || m_colliders[1].IsTouchingLayers(LayerMask.GetMask("Pipe")))
    //     {
    //         if(m_beKicked)
    //         {        
    //             transform.localScale = new Vector3(transform.localScale.x * (-1),
    //                                 transform.localScale.y,
    //                                 transform.localScale.z);
    //             return;
    //         }
    //         StartCoroutine(IETurnArround());
    //     }

    //     //! Fly
    //     if(m_hasWing)
    //         if(m_colliders[3].IsTouchingLayers(LayerMask.GetMask("Block"))
    //         || m_colliders[3].IsTouchingLayers(LayerMask.GetMask("StairBlock"))
    //         || m_colliders[3].IsTouchingLayers(LayerMask.GetMask("Pipe"))
    //         || m_colliders[3].IsTouchingLayers(LayerMask.GetMask("Ground")))
    //         {
    //             m_canFly = true;
    //             m_startPosition = transform.position;
    //         }


    // }

    // private void OnTriggerStay2D(Collider2D other) 
    // {
    //     if(m_isRedKoopaTroopa)
    //         StartCoroutine(IETurnArround());
    // }


    private void OnTriggerExit2D(Collider2D other) 
    {
        //Debug.Log("m_isRedKoopaTroopa: " + m_isRedKoopaTroopa + " m_beKicked: " + m_beKicked);
        if(m_isRedKoopaTroopa && !m_beKicked)
        {
            if((1 << other.gameObject.layer) == LayerMask.GetMask("Block") || 
                (1 << other.gameObject.layer) == LayerMask.GetMask("StairBlock") ||
                (1 << other.gameObject.layer) == LayerMask.GetMask("Ground") ||
                (1 << other.gameObject.layer) == LayerMask.GetMask("Island") ||
                (1 << other.gameObject.layer) == LayerMask.GetMask("Bridge"))
                StartCoroutine(IETurnArround());

            // Debug.Log("OnTriggerExit2D");
        }
    }


    IEnumerator IETurnArround()
    {
        m_isTurnArround = true;
        m_animKoopaTroopa.SetBool("isTurnArround", m_isTurnArround);

        yield return new WaitForSeconds(Config.m_KTTimeTurnArround);
        
        m_isTurnArround = false;
        m_animKoopaTroopa.SetBool("isTurnArround", m_isTurnArround);

        transform.localScale = new Vector3(transform.localScale.x * (-1),
                                    transform.localScale.y,
                                    transform.localScale.z);
    }

    public override void TurnArround()
    {
        StartCoroutine(IETurnArround());
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if(m_isActive && m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            Player player = other.gameObject.GetComponent<Player>();  
            if(player != null)player.ChangeForm(false);
        }
        
        //! Fly
        if(m_hasWing)
        {
            // If top collider with obstacle
            if(m_colliders[2].IsTouchingLayers(LayerMask.GetMask("Block"))
            || m_colliders[2].IsTouchingLayers(LayerMask.GetMask("Pipe"))
            || m_colliders[2].IsTouchingLayers(LayerMask.GetMask("Bridge"))
            || m_colliders[2].IsTouchingLayers(LayerMask.GetMask("StairBlock")))
            {
                if(other.contacts[0].normal.y < 0)
                    m_canFly = false;

                if(other.contacts[0].normal.x != 0)
                    StartCoroutine(IETurnArround());
                    
            }

            //! If player jump top
            if(m_colliders[2].IsTouchingLayers(LayerMask.GetMask("Player")))
            {
                m_hasWing = false;
                m_rgb2D.velocity = Vector2.down * (20);
                m_audioManager.playEffectAudio(AudioManager.EffectAudio.KoopaShell);
            }
        }
        else
        {

            
            //! Kich shells by player.
            if(!m_isActive && m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Player")))
            {
                if(m_CRActive != null)
                {
                    Debug.Log("StopCoroutine Active.");
                    StopCoroutine(m_CRActive);
                    m_CRActive = null;

                    transform.localScale = new Vector3(transform.localScale.x * Mathf.Sign(other.contacts[0].normal.x * (-1)),
                                                       transform.localScale.y,
                                                       transform.localScale.z);

                    this.gameObject.layer = LayerMask.NameToLayer("Danger");

                    if(m_isRedKoopaTroopa)
                    {
                        m_colliders[1].enabled = true;
                        m_colliders[4].enabled = false;
                    }
                    

                    m_beKicked = true;
                    StartCoroutine(IEDangerWithPlayer());

                    // Debug.Log( LayerMask.GetMask("Danger") + " " + LayerMask.NameToLayer("Danger"));

                }
            }

            
            //!Recede into their shells
            if(m_isActive && m_colliders[2].IsTouchingLayers(LayerMask.GetMask("Player")))
            {
                m_isActive = false;
                m_animKoopaTroopa.SetBool("isActive", false);
                m_animKoopaTroopa.SetBool("isMove", false);
                m_audioManager.playEffectAudio(AudioManager.EffectAudio.KoopaShell);

                m_CRActive = StartCoroutine(IEActive());
            }

            if(!m_isActive && m_beKicked)
                CrashInto(other);
        }

    }

    IEnumerator IEDangerWithPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        m_isDangerWithPlayer = true;
    }



    IEnumerator IEActive()
    {
        m_colliders[2].enabled = false;

        yield return new WaitForSeconds(m_timeActive);
        
        m_animKoopaTroopa.SetBool("isActive", true);

        yield return new WaitForSeconds(.4f);

        m_animKoopaTroopa.SetBool("isMove", true);
        
        m_colliders[2].enabled  = true;
        m_isActive              = true;
        m_CRActive              = null;

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

        if(m_beKicked)
        {
            m_rgb2D.velocity = new Vector2(m_speedBekicked * transform.localScale.x * (-1),  
                                           m_rgb2D.velocity.y);
            return;
        }
        if(m_isTurnArround)
        {
            m_rgb2D.velocity = new Vector2(0, m_rgb2D.velocity.y);
            // m_rgb2D.velocity = Vector2.zero;
            return;
        }


        if(m_hasWing)
        {
            if(m_canFly)
            {
                m_rgb2D.velocity = new Vector2(m_speedMove * transform.localScale.x,  
                                                transform.up.y * m_speedFly);
                                                
                if(transform.position.y - m_startPosition.y > m_maxHeightFly)
                    m_canFly = false;
            }
                
            else
            m_rgb2D.velocity = new Vector2(m_speedMove * transform.localScale.x,  
                                                m_rgb2D.velocity.y);
                                            
        }
        else
        {
            m_gameobjWing.SetActive(false);

            if(m_isActive)
                m_rgb2D.velocity = new Vector2(m_speedMove * transform.localScale.x,  
                                                    m_rgb2D.velocity.y);
            else
                m_rgb2D.velocity = Vector2.zero;
            
        }
    }
    


    void CrashInto(Collision2D other)
    {
        if(m_isDangerWithPlayer && m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            Player player = other.gameObject.GetComponent<Player>();  
            if(player != null)player.ChangeForm(false);
        }

        if(m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Goomba")))
        {
            Goomba goomba = other.gameObject.GetComponent<Goomba>();
            if(goomba != null) goomba.CrashIntoDie();
        }

        if(m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Green Koopa Troopa")))
        {
            KoopaTroopa koopa = other.gameObject.GetComponent<KoopaTroopa>();
            if(koopa != null) koopa.CrashIntoDie();
        }

        if(m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Spiny")))
        { 
            Spiny spiny =  other.gameObject.GetComponent<Spiny>();
            if(spiny != null) spiny.CrashIntoDie();}
    }


   // private void OnTriggerStay2D(Collider2D other) 
    // {
    //     if(m_hasWing)
    //         if(m_colliders[3].IsTouchingLayers(LayerMask.GetMask("Block"))
    //         || m_colliders[3].IsTouchingLayers(LayerMask.GetMask("StairBlock"))
    //         || m_colliders[3].IsTouchingLayers(LayerMask.GetMask("Pipe"))
    //         || m_colliders[3].IsTouchingLayers(LayerMask.GetMask("Ground")))
    //         {
    //             m_canFly = true;
    //             m_startPosition = transform.position;
    //         }

                
    // }


    public override void CrashIntoDie()
    {
        m_deltaTime         = 0f;
        m_isCrashIntoDie    = true;
        m_rgb2D.velocity    = Vector3.zero;
        m_startpos          = transform.position;
        
        m_angleCrashInto    = Config.GetAngleCrashInto();

        
        m_animKoopaTroopa.SetBool("isActive", false);
        m_animKoopaTroopa.SetBool("isMove", false);
        
        transform.localScale = new Vector3(transform.localScale.x, 
                                            -transform.localScale.y,
                                            transform.localScale.z);

                                            
        if(m_scoreKeeper != null)m_scoreKeeper.collectScore(Config.m_KTScore);
        
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
        if(m_scoreKeeper != null)m_scoreKeeper.collectScore(Config.m_KTScore);
        Destroy(gameObject);
    }


}
