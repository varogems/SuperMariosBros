using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Block : MonoBehaviour
{

    Animator                m_anim;
    Vector2                 m_startPos;
    bool                    m_isStrucking;
    [SerializeField] float  m_range;
    [SerializeField] float  m_maxRange;
    bool m_isPushUp;
    [SerializeField] int    m_timesItem = 1;
    [SerializeField] bool   m_isUpLevelMushroom = false;
    SpriteRenderer          m_sprR;
    Collider2D              m_collider2D;

    ScoreKeeper             m_scoreKeeper;
    AudioManager            m_audioManager;

    void Awake() 
    {
        m_isStrucking   = false;
        m_startPos      = transform.position;
        m_isPushUp      = false;
        m_sprR          = GetComponent<SpriteRenderer>();
        m_anim          = GetComponent<Animator>();
        m_collider2D    = GetComponent<Collider2D>();
        m_scoreKeeper   = FindObjectOfType<ScoreKeeper>();
        m_audioManager  = FindObjectOfType<AudioManager>();
    }



    void Update() 
    {
        //! If is None Question block, to play effect QuestionBlockNone
            if(m_isStrucking && m_timesItem == 0 && m_anim != null)
            {
                m_isPushUp = m_isStrucking = false;
                m_audioManager.playEffectAudio(AudioManager.EffectAudio.QuestionBlockNone);
                return;
            }

            if(m_isStrucking && m_timesItem > 0)
            {
                
                if(m_isPushUp)
                {
                    transform.position += Vector3.up * m_range;
                    if(transform.position.y - m_startPos.y > m_maxRange)
                    {
                        m_isPushUp = false;
                        transform.position = new Vector2(m_startPos.x, m_startPos.y + m_range);
                    }
                }
                else
                {
                    transform.position = Vector3.up * m_range;
                    if(transform.position.y  < m_startPos.y)
                    {
                        m_isPushUp = m_isStrucking = false;
                        transform.position = m_startPos;
                        DropItem();
                    }
                }
            }


    }


    void DropItem()
    {

        if(m_anim != null)
        {
            if(m_anim.enabled)
            {
                m_anim.enabled = false;
                m_timesItem = 0;

                GameObject mushroom;

                if(m_isUpLevelMushroom)
                    mushroom = (GameObject) Instantiate(Resources.Load("Prefab/UpLevelMushRoom"), transform);
                else
                    mushroom = (GameObject) Instantiate(Resources.Load("Prefab/PoisonMushRoom"), transform);

                m_audioManager.playEffectAudio(AudioManager.EffectAudio.QuestionBlock);
                
                mushroom.transform.position = transform.position;
                mushroom.GetComponent<Mushroom>().Appear();

                

                // m_sprR.sprite = Resources.LoadAll<Sprite>("tiles-4.png_4")[0];
                //! Remember destroy bef call Destroy func
                Texture2D txt2d = Resources.Load<Texture2D>("Sprite/map/tiles-4");

                m_sprR.sprite = Sprite.Create(txt2d, new Rect(86, 924, 16, 16), new Vector2(0.5f, 0.5f), 15);
            }

        }
        //! Normal block
        else
        {
            if(m_scoreKeeper != null)
                m_scoreKeeper.collectCoin();
                
            StartCoroutine(collectCoin());
        }
    }




    IEnumerator collectCoin()
    {
        //! NeedSound
        m_audioManager.playEffectAudio(AudioManager.EffectAudio.Coin);

        GameObject coin = (GameObject) Instantiate(Resources.Load("Prefab/Coin"), transform);
        coin.transform.position = transform.position;
        coin.GetComponent<Rigidbody2D>().velocity = Vector2.up * 2.2f;


        // yield return new WaitForSeconds(.5f);
        yield return new WaitForSeconds(.3f);

        Destroy(coin);

        m_timesItem--;
        if(m_timesItem == 0)
        {
            m_sprR.enabled = m_collider2D.enabled = false;
            m_audioManager.playEffectAudio(AudioManager.EffectAudio.BrickBroken);
        }

    }



    private void OnCollisionEnter2D(Collision2D other) 
    {
        if((1 << other.gameObject.layer) == LayerMask.GetMask("Player")
        && other.contacts[0].normal.y > 0)
        {
            m_isStrucking = m_isPushUp = true;
        }
    }


}
