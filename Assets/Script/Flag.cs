using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flag : MonoBehaviour
{
    Transform               m_transformFlag;
    bool                    m_getDownFlag, m_isDownFlag;
    float                   m_height;
    [SerializeField] float  m_speedDown;
    Vector2                 m_startPos;
    Player                  m_player;

    AudioManager            m_audioManager;
    void Awake() 
    {
        m_height        = Config.m_FlagDistanceMove;
        m_speedDown     = Config.m_FlagSpeedDown;
        m_transformFlag = transform.GetChild(2).gameObject.GetComponent<Transform>();

        m_getDownFlag   = false;
        m_isDownFlag    = false;

        m_player        = FindObjectOfType<Player>();
        m_audioManager  = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if(m_getDownFlag)
        {
            m_transformFlag.position = new Vector3(m_transformFlag.position.x, m_transformFlag.position.y - m_speedDown, 0);

            if(m_startPos.y - m_transformFlag.position.y > m_height) 
            {
                m_getDownFlag   = false;
                m_isDownFlag    = true;
                m_player.Climb(m_getDownFlag);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if((1 << other.gameObject.layer) == LayerMask.GetMask("Player") && !m_isDownFlag)
        {
            m_getDownFlag   = true;
            m_startPos      = m_transformFlag.position;
            m_player.Climb(m_getDownFlag);
            m_audioManager.playEffectAudio(AudioManager.EffectAudio.FlagDown);
        }
    }
}
