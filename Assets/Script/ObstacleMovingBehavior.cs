using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleMovingBehavior : MonoBehaviour
{
    MovingGameObject m_gameObject;
    [SerializeField] bool m_turnArroundWithObstacle = true;
    int m_idPlatform = int.MinValue;


    void Awake()
    {
        m_gameObject = transform.parent.GetComponent<MovingGameObject>();
    }



    //! gameobject turn arround when touch everything except player.
    private void OnTriggerEnter2D(Collider2D other) 
    {
        //! Get first platform if first stand this platform.
        if(!m_turnArroundWithObstacle && (m_idPlatform == int.MinValue))
            m_idPlatform = (1 << other.gameObject.layer);

        if((m_turnArroundWithObstacle && (1 << other.gameObject.layer) != LayerMask.GetMask("Player")) &&
            (m_turnArroundWithObstacle && (1 << other.gameObject.layer) != LayerMask.GetMask("Bullet Player")))
            m_gameObject.TurnArround();
    }


    //! gameobject turn arround when collider without platform.
    private void OnTriggerExit2D(Collider2D other) 
    {
        // if(!m_turnArroundWithObstacle && 
        //     ((1 << other.gameObject.layer) == LayerMask.GetMask("Block")        || 
        //     (1 << other.gameObject.layer) == LayerMask.GetMask("StairBlock")    || 
        //     (1 << other.gameObject.layer) == LayerMask.GetMask("Bridge")        || 
        //     (1 << other.gameObject.layer) == LayerMask.GetMask("Island")        ||
        //     (1 << other.gameObject.layer) == LayerMask.GetMask("Ground")))
        //     m_gameObject.TurnArround();

        if(!m_turnArroundWithObstacle && ((1 << other.gameObject.layer) == m_idPlatform))
            m_gameObject.TurnArround();

    }
}
