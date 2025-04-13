using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleCannon : MonoBehaviour
{

    Player                  m_player;
    bool                    m_isShoot;
    float                   m_deltaTime;
    [SerializeField] float  m_delaytimeShoot;
    AudioManager            m_audioManager;
    void Awake()
    {
        m_player            = FindObjectOfType<Player>();
        m_audioManager      = FindObjectOfType<AudioManager>();
        m_isShoot           = true;
        m_deltaTime         = 0;
        m_delaytimeShoot    = Config.m_TCDelayTimeShoot;
    }

    void Update()
    {
        m_deltaTime  += Time.deltaTime;
        
        if(m_deltaTime > m_delaytimeShoot)
        {
            if(m_isShoot && m_player)
            {   
                Shoot();
                m_deltaTime = 0;
            }
        }

        
    }

    void Shoot()
    {
        m_audioManager.playEffectAudio(AudioManager.EffectAudio.Cannon);
        PoolManager.SpawnBulletBill(transform);
        
        
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if((1 << other.gameObject.layer) == LayerMask.GetMask("Player") )
            m_isShoot = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if((1 << other.gameObject.layer) == LayerMask.GetMask("Player") )
            m_isShoot = false;
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        m_isShoot = true;
        m_deltaTime = 0;
    }



}
