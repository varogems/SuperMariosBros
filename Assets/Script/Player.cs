using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    Animator                m_anim;

    Rigidbody2D             m_rb2d;
    Vector2                 m_inputValue;
    [SerializeField] float  m_speedRun;
    [SerializeField] float  m_speedLinear;
    [SerializeField] float  m_maxSpeedLinear;
    [SerializeField] float  m_maxHeightJump;
    [SerializeField] float  m_timeFiring;
    [SerializeField] float  m_timePosing;

    Collider2D[]            m_colliders; 
    bool                    m_canJump;
    bool                    m_canSprint;
    bool                    m_isWalking;
    bool                    m_isCrouching;
    bool                    m_isFiring;
    bool                    m_isPosing;
    Vector2                 m_startPosBefJump;
    static int              m_curIndexForm = 0;
    bool                    m_isLockControl;

    bool                    m_isInvicible;
    float                   m_timeHurt;
    float                   m_timeUpform;
    float                   m_timeInvicibleAferBeHurted;
    bool                    m_isHurt;

    LevelManager            m_levelManager;
    AudioManager            m_audioManager;
    SpriteRenderer          m_spriteRenderer;



    void Awake()
    {
        // if(FindObjectsOfType(this.GetType()).Length > 1)
        // {
        //     this.gameObject.SetActive(false);
        //     Destroy(this.gameObject);
        //     return;
        // }
        // else 
        // {
        //     DontDestroyOnLoad(this.gameObject);
        // }


        m_colliders     = GetComponents<Collider2D>();
        m_anim          = GetComponent<Animator>();
        m_rb2d          = GetComponent<Rigidbody2D>();
        m_spriteRenderer= GetComponent<SpriteRenderer>();

        m_canJump       = false;
        m_canSprint     = false;
        m_isWalking     = false;
        m_isCrouching   = false;
        m_isFiring      = false;
        m_isPosing      = false;

        m_isLockControl = false;

        m_isHurt        = false;
        m_isInvicible   = false;
        m_timeHurt      = Config.m_PlayerTimeHurt;
        m_timeInvicibleAferBeHurted = Config.m_PlayerTimeInvicibleAfterBeHurted;
        m_timeUpform    = Config.m_PlayerTimeUpform;



        m_speedRun          = Config.m_PlayerSpeedRun;
        m_speedLinear       = Config.m_PlayerSpeedLinear;
        m_maxSpeedLinear    = Config.m_PlayerMaxSpeedLinear;

        m_maxHeightJump     = Config.m_PlayerMaxHeightJump;
        m_timeFiring        = Config.m_PlayerTimeFiring;
        m_timePosing        = Config.m_PlayerTimePosing;

        m_levelManager      = FindObjectOfType<LevelManager>();
        m_audioManager      = FindObjectOfType<AudioManager>();



        setForm(m_curIndexForm);
        //Debug.Log("Awake m_curIndexForm " + m_curIndexForm);

    }


    // Update is called once per frame
    void Update()
    {
        
        if(m_isLockControl) 
        {
            m_rb2d.velocity = new Vector2(0, m_rb2d.velocity.y);
            return;
        }

        BeHurted();
        
        Run();
        Crouch();
        Sprint();
        FlipAnim();
    }

    void FixedUpdate()
    {
        Jump();
    }

    void BeHurted()
    {
        if(m_isHurt)
            m_spriteRenderer.color = new Color(m_spriteRenderer.color.r,
                                            m_spriteRenderer.color.g, 
                                            m_spriteRenderer.color.b, 
                                            (m_spriteRenderer.color.a + 0.49f) % 1f);
        else
        m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, 
                                            m_spriteRenderer.color.g, 
                                            m_spriteRenderer.color.b, 
                                            1f);
        
    }


    void OnMove(InputValue value)
    {
        if(m_isLockControl) 
            return;

        m_inputValue    = value.Get<Vector2>();
        m_isWalking     = (Mathf.Abs(m_inputValue.x) > 0) ? true : false;
        m_isCrouching   = (m_inputValue.y < 0) ? true : false;
        
    }

    void Run()
    {
        if(!m_canSprint && m_isWalking)
            m_rb2d.velocity = new Vector2(m_inputValue.x * m_speedRun, m_rb2d.velocity.y);

        m_anim.SetBool("isWalking", m_isWalking);

    }


    void OnSprint(InputValue value)
    {
        if(m_isLockControl) return;
        
        m_canSprint = value.Get<float>() > 0 ? true : false;
        
        if(!m_canSprint)m_rb2d.velocity = new Vector2(0, m_rb2d.velocity.y);
    }

    void Sprint()
    {
        if(m_canSprint && m_isWalking)
        {
            m_rb2d.velocity += new Vector2(transform.localScale.x * m_speedLinear, 0);

            if(Mathf.Abs(m_rb2d.velocity.x) > m_maxSpeedLinear)
                m_rb2d.velocity = new Vector2(m_maxSpeedLinear * transform.localScale.x, m_rb2d.velocity.y);

        }
    }

    void FlipAnim()
    {
        //! If has velocity from input, so check and flip animation
        if(Mathf.Abs(m_inputValue.x) > Mathf.Epsilon)
            transform.localScale = new Vector3(Mathf.Sign(m_inputValue.x), 1f, 1f);

        //! If has velocity from input, so check and flip animation
        // if(Mathf.Abs(m_rb2d.velocity.x) > Mathf.Epsilon)
        //     transform.localScale = new Vector3(Mathf.Sign(m_rb2d.velocity.x), 1f, 1f);

    }

    void OnJump(InputValue value)
    {
        if(m_isLockControl) return;
        
        m_canJump = (value.Get<float>() > 0)? true: false;

        if(m_canJump) 
        {

            if(m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Ground"))
            || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Bridge"))
            || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Island"))
            || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Block")) 
            || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("StairBlock"))
            || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Pipe"))
            || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("TurtleCannon")))
            {
                m_startPosBefJump = transform.position;
                if(m_curIndexForm == 0)
                    m_audioManager.playEffectAudio(AudioManager.EffectAudio.JumpLv1);
                else m_audioManager.playEffectAudio(AudioManager.EffectAudio.JumpLv2);
            }
            else 
                m_canJump = false;

        }

    }

    void Jump()
    {
        if(m_canJump)
        {
            float distanceY = Mathf.Abs(transform.position.y - m_startPosBefJump.y);

            if(distanceY > m_maxHeightJump)
            {
                m_canJump = false;
                m_rb2d.velocity =Vector2.zero;
            }
            else
            {  
                float deltaTime     = Time.fixedDeltaTime;
                
                //! Hmax
                float totalVelocity                 = Mathf.Sqrt((m_maxHeightJump) * -2 * Physics.gravity.y);
                float maxVelocityForFixedUpdate     = (Config.m_PlayerForceJump * deltaTime) / m_rb2d.mass;


                if(m_rb2d.velocity.y < totalVelocity)
                {
                    float availableVelocity = Mathf.Sqrt((m_maxHeightJump - distanceY) * -2 * Physics.gravity.y);
                    float force             = Mathf.Clamp(availableVelocity, 0f, maxVelocityForFixedUpdate) * m_rb2d.mass / deltaTime;
                    m_rb2d.AddForce(transform.up * force);
                }
            }

            m_anim.SetBool("isJumping", true);
        }
    }



    void OnCollisionEnter2D(Collision2D other) 
    {
        //! Stop jump if have top collision
        if(((1 << other.gameObject.layer) == LayerMask.GetMask("Block")
        || (1 << other.gameObject.layer) == LayerMask.GetMask("StairBlock")
        || (1 << other.gameObject.layer) == LayerMask.GetMask("Pipe"))
        && (other.contacts[0].normal.y < 0))
               m_canJump =  false;
    }



    void Crouch()
    {
        if(m_isCrouching)
        {
            if(m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Ground"))
            || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Bridge"))
            || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Island"))
            || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Block")) 
            || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("StairBlock"))
            || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Pipe"))
            || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("TurtleCannon")))
                    {

                        if(m_curIndexForm != 0)
                        {
                            m_colliders[m_curIndexForm + 1].enabled = false;
                            m_anim.SetBool("isCrouching", m_isCrouching);
                        }
                    }
        }
        else 
        {
            
            if(m_curIndexForm != 0)
            {
                m_colliders[m_curIndexForm + 1].enabled = true;
                m_anim.SetBool("isCrouching", m_isCrouching);
            }
        }
    }

    void OnFire(InputValue value)
    {
        if(m_isLockControl) return;
        
        if(value.isPressed && !m_isFiring && (m_curIndexForm == 2))
            StartCoroutine(Fire());
    }

    void OnSceneBoss()
    {
        m_levelManager.LoadScene(LevelManager.LevelName.Level4);
    }

    IEnumerator Fire()
    {   m_isFiring  = true;
        m_anim.SetBool("isFiring", m_isFiring);
        ShootingBullet();
        yield return new WaitForSeconds(m_timeFiring);

        m_isFiring = false;
        m_anim.SetBool("isFiring", m_isFiring);
    }

    void ShootingBullet()
    {

        PoolManager.SpawnFireBullets(transform);
        m_audioManager.playEffectAudio(AudioManager.EffectAudio.FireBall);
    }


    void OnPose(InputValue value)
    {
        if(m_isLockControl) return;
        
        if(value.isPressed && !m_isPosing) 
            StartCoroutine(IEPose());
    }


    IEnumerator IEPose()
    {   m_isPosing  = true;
        m_anim.SetBool("isPosing", m_isPosing);

        yield return new WaitForSeconds(m_timePosing);

        m_isPosing = false;
        m_anim.SetBool("isPosing", m_isPosing);
    }

    public void Pose()
    {
        StartCoroutine(IEPose());
    }



    void OnUpForm(InputValue value)
    {
        if(m_isLockControl) return;
        if(m_isInvicible)
            return;
        
        if(value.isPressed)
            ChangeForm(true);

    }

    void OnDownForm(InputValue value)
    { 
        if(m_isLockControl) return;

        if(value.isPressed)
            ChangeForm(false);


    }

    IEnumerator IEStopInvicible()
    {
        if(m_isHurt)
        {
            // m_colliders[1].isTrigger = true;
            // m_rb2d.bodyType = RigidbodyType2D.Static;

            yield return new WaitForSeconds(m_timeHurt);

            // m_rb2d.bodyType = RigidbodyType2D.Dynamic;
            // m_colliders[1].isTrigger = false;

            yield return new WaitForSeconds(m_timeInvicibleAferBeHurted);
            m_isHurt = false;

            //! return
            yield break;

        }
        else
        {
            m_isInvicible = true;
            // m_colliders[1].isTrigger = true;
            // m_rb2d.bodyType = RigidbodyType2D.Static;

            yield return new WaitForSeconds(m_timeUpform);

            // m_rb2d.bodyType = RigidbodyType2D.Dynamic;
            // m_colliders[1].isTrigger = false;
            
            m_isInvicible = false;


        }

    }



    public void ChangeForm(bool isUpForm)
    {
        
        if(m_isInvicible)
            return;

        if(m_isHurt)
            return;


        m_isHurt = !isUpForm;
            
        if(isUpForm)
        {
            if(m_curIndexForm == 2)
            {
                // Not upgrade form and add point
                return;
            }
            m_curIndexForm++;
            m_audioManager.playEffectAudio(AudioManager.EffectAudio.UpLvlMushroom);
            
        }
        else
        {
            if(m_curIndexForm == 0)
            {
                //Die
                m_isLockControl = true;
                m_anim.SetBool("isDie", true);

                m_audioManager.stopBackgroungMusic();
                m_audioManager.playEffectAudio(AudioManager.EffectAudio.LostALive);
                
                m_levelManager.reloadScene();

                return;
            }
            
            m_curIndexForm--;

        }

        setForm(m_curIndexForm);
            
    }

    public void ImmediatelyDie()
    {
        //! Set Form
        m_curIndexForm = 0;
        int countLayer = m_anim.layerCount;

        //! Reset weight all layer
        for(int i = 0; i < countLayer; i++)
            m_anim.SetLayerWeight(i, 0);

        //! Active layer form with current index
        m_anim.SetLayerWeight(m_curIndexForm, 1);

        //! Active collider with corresponding layer
        for(int i = 2; i < m_colliders.Length; i++)
                m_colliders[i].enabled = false;
                
        m_colliders[m_curIndexForm + 1].enabled = true;
        

        //Die
        m_isLockControl = true;
        m_anim.SetBool("isDie", true);
        

        m_audioManager.stopBackgroungMusic();
        m_audioManager.playEffectAudio(AudioManager.EffectAudio.LostALive);
        
        m_levelManager.reloadScene();
    }
    

    IEnumerator IETurnOffCollisonBySecond(float deltaTime)
    {
        m_colliders[1].isTrigger = true;
        yield return new WaitForSeconds(deltaTime);
        m_colliders[1].isTrigger = false;
    }

    void setForm(int indexForm)
    {
        m_curIndexForm = indexForm;

        int countLayer = m_anim.layerCount;

        //! Reset weight all layer
        for(int i = 0; i < countLayer; i++)
            m_anim.SetLayerWeight(i, 0);

        //! Active layer form with current index
        m_anim.SetLayerWeight(m_curIndexForm, 1);

        //! Active collider with corresponding layer
        for(int i = 2; i < m_colliders.Length; i++)
                m_colliders[i].enabled = false;
                
        m_colliders[m_curIndexForm + 1].enabled = true;

                
        StartCoroutine(IEStopInvicible());
    }



    //TriggerEnter2D & TriggerStay2D guarante anim is Idle if stand on platform
    void OnTriggerEnter2D(Collider2D other) 
    {
        IsStanding(other);
    }

    void OnTriggerStay2D(Collider2D other) 
    {
        IsStanding(other);
    }

    void IsStanding(Collider2D other)
    {
        if((m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Ground"))
        || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Bridge"))
        || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Island"))
        || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Block")) 
        || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("StairBlock"))
        || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Pipe"))
        || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("TurtleCannon")))
        || m_colliders[0].IsTouchingLayers(LayerMask.GetMask("Goomba")))
        // other.contacts[0].normal.y > 0)
            m_anim.SetBool("isJumping", false);


    }


    public void Climb(bool isClimbing)
    {
        m_anim.SetBool("isClimbing", isClimbing);
        m_isLockControl = isClimbing;

        if(isClimbing)
            m_isWalking = m_canJump = m_canSprint = false;
        
    }


}
