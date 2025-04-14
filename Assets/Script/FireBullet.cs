using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{

    Rigidbody2D m_rgb2D;
    float m_speedMove;
    // Start is called before the first frame update
    void Awake()
    {
        m_rgb2D = GetComponent<Rigidbody2D>();
        m_speedMove = 15f;

        m_rgb2D.velocity = new Vector2(m_speedMove * transform.localScale.x, 1.2f);

        StartCoroutine(IEWaitAwakePoolManager());
    }

    IEnumerator IEWaitAwakePoolManager()
    {
        // while(ResourceGame.m_instance == null)
        //     yield return new WaitForSeconds(0.1f);

        if(PoolManager.m_instance == null)
            yield return new WaitWhile(() => PoolManager.m_instance == null);

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position +=  m_speedMove * Vector3.right * transform.localScale.x * Time.deltaTime;
        m_rgb2D.velocity = new Vector2(m_speedMove * transform.localScale.x,  
                                        m_rgb2D.velocity.y);
    }



    private void OnCollisionEnter2D(Collision2D other) 
    {   
        if((1 << other.gameObject.layer) == LayerMask.GetMask("Player"))
            return;


        if((1 << other.gameObject.layer) == LayerMask.GetMask("Enemy"))
        {
            var _gameObject = other.gameObject.GetComponent<MovingGameObject>();
            if(_gameObject != null)
                _gameObject.DieByBulletPlayer();

            _gameObject = null;
        }


        if((1 << other.gameObject.layer) == LayerMask.GetMask("TriggerEnemy"))
        {
            var _gameObject = other.transform.parent.gameObject.GetComponent<MovingGameObject>();
            if(_gameObject != null && (1 << other.transform.parent.gameObject.layer) != LayerMask.GetMask("Danger"))
                _gameObject.DieByBulletPlayer();
                
            _gameObject = null;
        }
        
        // Destroy(this.gameObject);
        if(PoolManager.m_instance != null)
            PoolManager.PlayParticleFireShooting(this.transform);

        this.gameObject.SetActive(false);
        

    }


    private void OnTriggerEnter2D(Collider2D other) 
    {

        if((1 << other.gameObject.layer) == LayerMask.GetMask("Player"))
            return;


        if((1 << other.gameObject.layer) == LayerMask.GetMask("Enemy"))
        {
            var _gameObject = other.gameObject.GetComponent<MovingGameObject>();
            if(_gameObject != null)
                _gameObject.DieByBulletPlayer();

            _gameObject = null;
        }


        if((1 << other.gameObject.layer) == LayerMask.GetMask("TriggerEnemy"))
        {
            var _gameObject = other.transform.parent.gameObject.GetComponent<MovingGameObject>();
            if(_gameObject != null && (1 << other.transform.parent.gameObject.layer) != LayerMask.GetMask("Danger"))
                _gameObject.DieByBulletPlayer();

            _gameObject = null;
        }

        // // Skip range TurtleCannon
        // if((1 << other.gameObject.layer) == LayerMask.GetMask("TurtleCannon")
        // && other.GetType().ToString().Contains("CapsuleCollider2D"))
        // {
        //     return;
        // }



        // Destroy(this.gameObject);

        if(PoolManager.m_instance != null)
            PoolManager.PlayParticleFireShooting(this.transform);
            
        this.gameObject.SetActive(false);
        
    }

}
