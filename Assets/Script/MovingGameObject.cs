using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MovingGameObject : MonoBehaviour
{
    Transform[] m_listTranform;
    void Awake()
    {
    }

    public virtual void  TurnArround()
    {
        transform.localScale = new Vector3(transform.localScale.x * (-1),
                                    transform.localScale.y,
                                    transform.localScale.z);
    }

    public virtual void BeStomped()
    {
        //Transform[] transforms = GetComponentsInChildren<Transform>();

        if(m_listTranform == null)
            m_listTranform = GetComponentsInChildren<Transform>();

        foreach (Transform _transforms in m_listTranform)
            if (_transforms.name.ToString().Contains("TriggerEnemy") || 
                _transforms.name.ToString().Contains("TopEnemy"))
                _transforms.gameObject.SetActive(false);
    }

    public void RefreshCollider()
    {
        
        if(m_listTranform == null)
            m_listTranform = GetComponentsInChildren<Transform>();
            
        foreach (Transform _transforms in m_listTranform)
            if (_transforms.name.ToString().Contains("TriggerEnemy") || 
                _transforms.name.ToString().Contains("TopEnemy"))
                _transforms.gameObject.SetActive(true);

    }
    
    public virtual void DieByBulletPlayer()
    {

    }

    public virtual void BeKicked(Collision2D collisionPlayer)
    {
        
    }

    
    public virtual void CrashIntoDie()
    {
    }
}
