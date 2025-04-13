using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMovingBehavior : MonoBehaviour
{
    MovingGameObject m_gameObject;


    void Awake()
    {
        m_gameObject = transform.parent.GetComponent<MovingGameObject>();
    }


    //! gameobject turn arround when touch everything except player.
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if((1 << other.gameObject.layer) != LayerMask.GetMask("Player") && 
            (1 << other.gameObject.layer) != LayerMask.GetMask("Enemy") && 
            (1 << other.gameObject.layer) != LayerMask.GetMask("TriggerEnemy"))
                m_gameObject.TurnArround();
    }


}
