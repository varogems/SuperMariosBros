using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;

using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SlashBeast : MovingGameObject
{
    enum ESkill
    {
        Skill1,
        Skill2,
        Skill3,
        Idle
    };


    Animator        m_animator;
    Collider2D[]    m_collider2D;
    
    Rigidbody2D     m_rgb2D;
    GameObject      m_swordLazer;
    SpriteRenderer  m_spR;

    Coroutine       m_crtATK;
    Coroutine       m_crtReadyATK;
    Coroutine       m_crtStomp;
    ESkill          m_skillBoss;


    Vector2         m_startPos;

    bool            m_isJumpSkill1;
    Coroutine       m_crtSkill1;


    int             m_ATK2times;
    bool            m_isTurnArround;
    

    float           m_angleSkill3;
    float           m_minHeighJumpSkill3;
    Vector2         m_vectorMaxJumpSkill3;
    bool            m_isMaxHeightSkill3;

    bool            m_isHurted;
    GameObject      m_triggerEnemyGameObject;


    Player          m_player;
    int             m_health;
    bool            m_isActiveBoss;
    bool            m_isDie;




    void Awake()
    {
        m_animator      = GetComponent<Animator>();
        m_rgb2D         = GetComponent<Rigidbody2D>();
        m_collider2D    = GetComponents<Collider2D>();
        m_spR           = GetComponent<SpriteRenderer>();
        m_player        = FindObjectOfType<Player>();

        m_swordLazer    = transform.GetChild(0).gameObject;
        m_swordLazer.SetActive(false);

        m_collider2D[2].enabled = m_collider2D[3].enabled = false;

        m_skillBoss         = ESkill.Idle;

        m_crtReadyATK       = null;
        m_crtATK            = null;
        
        m_isJumpSkill1      = false;
        m_crtSkill1         = null;

        m_ATK2times         = 0;
        m_isTurnArround     = false;

        m_minHeighJumpSkill3 = 5.3f;

        m_angleSkill3         = Mathf.Epsilon;
        m_vectorMaxJumpSkill3 = Vector2.zero;
        m_isMaxHeightSkill3   = false;
        m_crtStomp            = null;
        m_isHurted            = false;
        m_health              = 3;
        m_isActiveBoss        = false;
        m_isDie               = false;

        
        m_triggerEnemyGameObject    = transform.GetChild(2).gameObject;


    }


    public void ActiveBoss()
    {
        m_isActiveBoss = true;
    }

    void Update()
    {
        if(!m_isActiveBoss) return;

        if(m_crtReadyATK == null && !m_isHurted)
            StartCoroutine(IEATK());

        if(m_isDie)
            m_spR.color = new Color(m_spR.color.r,
                                    m_spR.color.g, 
                                    m_spR.color.b, 
                                    (m_spR.color.a + 0.49f) % 1f);
                                    
    }

    void FixedUpdate()
    {
        PhysicSkill();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(((m_skillBoss == ESkill.Skill1) ||  (m_skillBoss == ESkill.Skill3))&& 
                (m_collider2D[4].IsTouchingLayers(LayerMask.GetMask("Ground")) || 
                m_collider2D[4].IsTouchingLayers(LayerMask.GetMask("StairBlock"))))
                StartCoroutine(IEStand());

    }

    public override void TurnArround()
    {

        if(!m_isTurnArround)
        StartCoroutine(IETurnArround());
    }
    public override void BeStomped()
    {
        if(!m_isHurted)
            StartCoroutine(IEBeHurted());
    }

    IEnumerator IEBeHurted()
    {
        m_health -= Config.m_PlayerDamageStomp;
        m_isHurted = true;

        m_triggerEnemyGameObject.SetActive(!m_isHurted);

        StopPhysicSkill();
        DoneATK();

        if(m_health > 0)
        {
            m_animator.SetBool("isHurted", true);
            yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length);
            
            yield return new WaitForSeconds(1.5f);
            
            m_animator.SetBool("isHurted",      false);
            m_animator.SetBool("isJump",        false);
            m_animator.SetBool("isReadyATK",    false);
            m_animator.SetBool("isATK1",        false);
            m_animator.SetBool("isATK2",        false);
            m_animator.SetBool("isTurnArround", false);
            m_animator.SetBool("isStand",       false);

            m_isHurted = false;

            m_triggerEnemyGameObject.SetActive(!m_isHurted);
        }
        else 
        {
            m_isDie = true;
            m_animator.SetBool("isDie", true);
            yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length);

            m_player.Pose();
            yield return new WaitForSeconds(2f);
            FindObjectOfType<LevelManager>().LoadScene(LevelManager.LevelName.GameOver);
        }


    }

    IEnumerator IEStand()
    {
        //! Stop Skill 1
        m_animator.SetBool("isReadyATK", false);
        m_animator.SetBool("isATK1", false);

        // yield return new WaitForSeconds(.25f);
        yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length);


        if(m_skillBoss == ESkill.Skill3)
        {
            m_isMaxHeightSkill3     = false;
            m_vectorMaxJumpSkill3   = Vector2.zero;
            m_angleSkill3           = Mathf.Epsilon;
            m_skillBoss             = ESkill.Idle;
        }

        
        DoneATK();

    }


    IEnumerator IEATK()
    {
        lookPlayer();

        switch(Random.Range(0, 3))
        // switch(Random.Range(2, 2))
        {
            case 1:
                yield return (m_crtReadyATK = StartCoroutine(IEReadyATK()));
                yield return (m_crtATK = StartCoroutine(AnimSkill1()));
            break;
            case 2:
                yield return (m_crtReadyATK = StartCoroutine(IEReadyATK()));
                yield return (m_crtATK = StartCoroutine(AnimSkill2()));
            break;
            case 3:
                yield return (m_crtReadyATK = StartCoroutine(IEReadyATK()));
                yield return (m_crtATK = StartCoroutine(AnimSkill3()));
            break;
            default:
                yield return (m_crtReadyATK = StartCoroutine(IEReadyATK()));
                yield return (m_crtATK = StartCoroutine(AnimSkill3()));
            break;
        }

    }

    void lookPlayer()
    {
        transform.localScale = new Vector3(Mathf.Sign(m_player.transform.position.x - transform.position.x), 
                                    transform.localScale.y, 
                                    transform.localScale.z);
    }


    IEnumerator IEReadyATK()
    {
        yield return new WaitForSeconds(2f);
        m_animator.SetBool("isReadyATK", true);
        
        //yield return new WaitForSeconds(0.417f + 0.3f);
        
        yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length + 0.3f);
        
    }

    void DoneATK()
    {

        // m_animator.SetBool("isTurnArround", false);
        // m_animator.SetBool("isATK1", false);
        // m_animator.SetBool("isATK2", false);
        // m_animator.SetBool("isReadyATK", false);
        
        m_swordLazer.SetActive(false);

        if(m_crtSkill1 != null)
            StopCoroutine(m_crtSkill1);

        if(m_crtReadyATK != null)
            StopCoroutine(m_crtReadyATK);

        if(m_crtATK != null)
            StopCoroutine(m_crtATK);

        if(m_crtStomp != null)
            StopCoroutine(m_crtStomp);

        m_crtATK = m_crtReadyATK = m_crtStomp = m_crtSkill1 = null;


    }


    IEnumerator AnimSkill1()
    {
        m_isJumpSkill1 = true;

        /*m_collider2D[1].enabled = */m_collider2D[2].enabled = true;
        m_collider2D[0].enabled = /*m_collider2D[1].enabled = */false;
        
        m_startPos = transform.position;
        m_skillBoss = ESkill.Skill1;

        m_animator.SetBool("isReadyATK", true);
        m_animator.SetBool("isATK1", true);
        

        // Debug.Log("Name clip: " + m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        // Debug.Log("time clip: " + m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        // yield return new WaitForSeconds(m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        
        yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length);
        
        m_isJumpSkill1 = false;
        
        // yield return new WaitForSeconds(0.5f);


        m_animator.SetBool("isReadyATK", true);
        m_animator.SetBool("isATK1", false);
        
        yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length);

    }

    IEnumerator AnimSkill2()
    {
        if(m_ATK2times == 0)
            m_ATK2times = UnityEngine.Random.Range(2, 6);
            // m_ATK2times = 1;

        m_skillBoss = ESkill.Skill2;

        m_collider2D[0].enabled = /*m_collider2D[1].enabled = */true;
        
        m_animator.SetBool("isReadyATK", true);
        m_animator.SetBool("isATK2", true);

        m_swordLazer.SetActive(true);

        // yield return new WaitForSeconds(0.25f)
        yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length);
        

    }

    IEnumerator IETurnArround()
    {
        m_isTurnArround = true;
        m_swordLazer.SetActive(false);

        m_spR.color = new Color(m_spR.color.r,
                m_spR.color.g, 
                m_spR.color.b, 
                1f);
        
        m_animator.SetBool("isTurnArround", true);

        // yield return new WaitForSeconds(0.333f);
        yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length);
        
        // m_isTurnArround = false;
                           

        m_ATK2times--;
        if(m_ATK2times > 0)
        {

            m_animator.SetBool("isTurnArround", false);
            yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length * 0.5f);

            transform.localScale = new Vector3(transform.localScale.x * (-1),
                                    transform.localScale.y,
                                    transform.localScale.z);
                                    

            m_swordLazer.SetActive(true);

            
            m_isTurnArround = false;
            // DoneATK();
            yield break;
        }
        else
        {
            
            // Debug.Log("Stop TurnArround " + m_ATK2times);
            m_ATK2times = 0;
            m_skillBoss = ESkill.Idle;

            m_swordLazer.SetActive(false);
            
            m_animator.SetBool("isATK2", false);
            m_animator.SetBool("isTurnArround", false);
            m_animator.SetBool("isReadyATK", false);
            
            yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length);


            transform.localScale = new Vector3(transform.localScale.x * (-1),
                                    transform.localScale.y,
                                    transform.localScale.z);
                                    
            m_isTurnArround = false;
                                    
            DoneATK();
        }

    }


    IEnumerator AnimSkill3()
    {
        m_collider2D[3].enabled = true;
        m_collider2D[0].enabled = /*m_collider2D[1].enabled =*/ false;

        transform.localScale = new Vector3(Mathf.Sign(m_player.transform.position.x - transform.position.x), 
                                    transform.localScale.y, 
                                    transform.localScale.z);

        
        m_animator.SetBool("isReadyATK", true);
        m_animator.SetBool("isJump", true);

        m_startPos = transform.position;

        yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length);
        
        m_skillBoss     = ESkill.Skill3;



    }




    void PhysicSkill()
    {
        if(m_isHurted)
            return;

        switch(m_skillBoss)
        {
            case ESkill.Skill1: 
            PhysicSkill1();
            break;
            case ESkill.Skill2:
            PhysicSkill2();
            break;
            case ESkill.Skill3:
            PhysicSkill3();
            break;
            
            default: return;
        }
    }

    void StopPhysicSkill()
    {
        switch(m_skillBoss)
        {
            case ESkill.Skill1: 
            StopPhysicSkill1();
            break;
            case ESkill.Skill2:
            StopPhysicSkill2();
            break;
            case ESkill.Skill3:
            StopPhysicSkill3();
            break;
            default: return;
        }

        m_skillBoss = ESkill.Idle;
    }

    void PhysicSkill1()
    {
        if(!m_isJumpSkill1) 
            return;


        if(m_crtSkill1 == null)
            m_crtSkill1 = StartCoroutine(IETwinSlasher());
            

        float distanceY = Mathf.Abs(transform.position.y - m_startPos.y);
        float maxHeightJumpSkill1 = 1.7f;
        float jumpForce = 200f;

        if(distanceY < maxHeightJumpSkill1)
        {  
            float deltaTime     = Time.fixedDeltaTime;
            
            //! Hmax
            float totalVelocity                 = Mathf.Sqrt((maxHeightJumpSkill1) * -2 * Physics.gravity.y);
            float maxVelocityForFixedUpdate     = (jumpForce * deltaTime) / m_rgb2D.mass;


            if(m_rgb2D.velocity.y < totalVelocity)
            {
                float availableVelocity = Mathf.Sqrt((maxHeightJumpSkill1 - distanceY) * -2 * Physics.gravity.y);
                float force             = Mathf.Clamp(availableVelocity, 0f, maxVelocityForFixedUpdate) * m_rgb2D.mass / deltaTime;
                m_rgb2D.AddForce(transform.up * force);
            }
        }
        
    }

    void StopPhysicSkill1()
    {
        m_isJumpSkill1 = false;
        if(m_crtSkill1 != null)
        {
            StopCoroutine(m_crtSkill1);
            m_crtSkill1 = null;
        }
    }

    IEnumerator IETwinSlasher()
    {
        yield return new WaitForSeconds(.5f);
        // BulletManager.SpawnBulletBoss(transform);
        PoolManager.SpawnTwinSlasher(transform);

        // GameObject gameObject = (GameObject) Instantiate(ResourceGame.GetPrefab("TwinSlasher"),
        //                                                     transform.position, 
        //                                                     //Quaternion.identity
        //                                                     transform.rotation);
        // BulletBoss bullet = gameObject.GetComponent<BulletBoss>();
        // bullet.setDirectionVector(transform.right * (-1));

        
        
    }

    void PhysicSkill2()
    {
        if(m_isTurnArround)
        {
            m_rgb2D.velocity = new Vector2(0, m_rgb2D.velocity.y);
            return;
        }

        m_rgb2D.velocity = new Vector2(transform.right.x * transform.localScale.x * 22f, m_rgb2D.velocity.y);

        m_spR.color = new Color(m_spR.color.r,
                        m_spR.color.g, 
                        m_spR.color.b, 
                        (m_spR.color.a + 0.33f) % 1f);
    }

    void StopPhysicSkill2()
    {        
        m_isTurnArround = false;
        m_ATK2times = 0;

        m_swordLazer.SetActive(false);
        
        m_spR.color = new Color(m_spR.color.r,
                m_spR.color.g, 
                m_spR.color.b, 
                1f);
    }


    IEnumerator IEStomp()
    {
        // Debug.Log("Stomp");
        
        m_isMaxHeightSkill3 = true;

        m_animator.SetBool("isReadyATK", true);
        m_animator.SetBool("isJump", false);
        yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length);

    }

    void PhysicSkill3()
    {
        if(m_skillBoss == ESkill.Skill3)
            Utility.ObliqueProjectileMotion(m_startPos, transform.position, m_player.transform.position, 
                                    ref m_vectorMaxJumpSkill3, 100f, 
                                    m_minHeighJumpSkill3, 
                                    ref m_angleSkill3,
                                    ref m_isMaxHeightSkill3,
                                    m_rgb2D);

        if(m_skillBoss == ESkill.Skill3 && m_isMaxHeightSkill3 && m_crtStomp == null)
            m_crtStomp = StartCoroutine(IEStomp());
    }

    void StopPhysicSkill3()
    {
        m_isMaxHeightSkill3 = true;
        if(m_crtStomp != null)
        {
            StopCoroutine(m_crtStomp);
            m_crtStomp = null;
        }

    }


}
