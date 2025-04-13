using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class PiranhaPlant : MovingGameObject
{
    bool m_isStart = false;
    bool m_isWaiting = false;

    Collider2D m_collider2D;

    [SerializeField] float  m_speed              = 400f;
    [SerializeField] float  m_timeWaitingVisible = 2f;
    [SerializeField] float  m_timeWaitingHide    = 4f;
    Vector2                 m_startPos;
    Vector3                 m_direction;
    Rigidbody2D             m_rigidbody;
    ScoreKeeper             m_scoreKeeper;

    float m_maxDistance = 2f;
    // Start is called before the first frame update

    void Awake()
    {
        m_startPos  = transform.localPosition;
        m_direction = transform.up;
        
        m_rigidbody     = GetComponent<Rigidbody2D>();
        m_collider2D    = GetComponent<Collider2D>();
        m_scoreKeeper   = FindObjectOfType<ScoreKeeper>();
    }
    void Start()
    {
        StartCoroutine(startGameObject());
    }

    void Update()
    {   
        if(!m_isStart) return;
        if(m_isWaiting) return;

        m_rigidbody.velocity = m_direction * m_speed * Time.deltaTime;

        if(((transform.localPosition.y - m_startPos.y) > m_maxDistance) && (m_direction == transform.up))
        {
            transform.localPosition = new Vector2(transform.localPosition.x, m_startPos.y + m_maxDistance);
            m_direction = -transform.up;
            m_isWaiting = true;
            m_rigidbody.velocity = new Vector2();
            StartCoroutine(waitingGameObject(m_timeWaitingVisible));
        }
        else if((transform.localPosition.y < m_startPos.y) && (m_direction != transform.up))
        {
            
            transform.localPosition = m_startPos;
            m_direction = transform.up;
            m_isWaiting = true;
            m_rigidbody.velocity = new Vector2();
            StartCoroutine(waitingGameObject(m_timeWaitingHide));
        }
    }

    IEnumerator startGameObject()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        m_isStart = true;
    }

        IEnumerator waitingGameObject(float timeWaiting)
    {
        yield return new WaitForSeconds(timeWaiting);
        m_isWaiting = false;
    }




    // void OnTriggerEnter2D(Collider2D other) 
    // {
    //     //! Down form player.
    //     if(m_collider2D.IsTouchingLayers(LayerMask.GetMask("Player")))
    //     {
    //         Player player = other.gameObject.GetComponent<Player>();
    //         if(player != null)player.ChangeForm(false);
    //     }
    // }

    
    public override void DieByBulletPlayer()
    {
        if(m_scoreKeeper != null)
            m_scoreKeeper.collectScore(Config.m_PiranhaPlantScore);
            
        Destroy(gameObject);
    }

}
