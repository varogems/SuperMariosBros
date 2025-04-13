using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBill : MovingGameObject
{

    Collider2D[]                    m_colliders;
    bool                            m_beStepedOn;
    Rigidbody2D                     m_rg2D;
    [SerializeField]float           m_speedMove;
    ScoreKeeper                     m_scoreKeeper;
    Player                          m_player;

    // Start is called before the first frame update
    void Awake()
    {
        m_colliders     = GetComponentsInChildren<Collider2D>();
        m_rg2D          = GetComponent<Rigidbody2D>();
        m_beStepedOn    = false;
        m_speedMove     = Config.m_BulletBillSpeedMove;

        m_scoreKeeper   = FindObjectOfType<ScoreKeeper>();
        m_player        = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_beStepedOn)
            m_rg2D.velocity = new Vector2(m_speedMove * transform.right.x * transform.localScale.x, 0f);
        else
            m_rg2D.velocity = new Vector2(0 , -7f); 

    }



    public override void BeStomped()
    {
        base.BeStomped();
        m_beStepedOn = true;
        if(m_scoreKeeper != null)m_scoreKeeper.collectScore(Config.m_BulletBillScore);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {    
        if(((1 << other.gameObject.layer) == LayerMask.GetMask("Block"))||
            ((1 << other.gameObject.layer) == LayerMask.GetMask("StairBlock"))||
            ((1 << other.gameObject.layer) == LayerMask.GetMask("Pipe")) ||
            ((1 << other.gameObject.layer) == LayerMask.GetMask("Ground")) ||
            ((1 << other.gameObject.layer) == LayerMask.GetMask("Island")) ||
            ((1 << other.gameObject.layer) == LayerMask.GetMask("TurtleCannon")))
        {
                m_beStepedOn = false;
                this.gameObject.SetActive(false);
        }
    }
}
