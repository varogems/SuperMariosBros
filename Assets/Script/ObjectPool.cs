using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool : MonoBehaviour
{
    UnityEngine.Object  m_prefab;
    Transform           m_transform;
    int                 m_initialPoolSize = 6;

    private List<GameObject>    m_pool;

    void Awake()
    {
        m_transform = GetComponent<Transform>();
    }



    public void setPrefab( UnityEngine.Object prefab)
    {
        m_prefab = prefab;
        init();
    }
    

    void init()
    {
        m_pool = new List<GameObject>();

        for (int i = 0; i < m_initialPoolSize; i++)
        {
            GameObject _obj = (GameObject) Instantiate(m_prefab, m_transform);
            
            if(_obj != null)
            {
                _obj.SetActive(false);
                m_pool.Add(_obj);
            }
        }

    }
    public int numberOfGameObject()
    {
        return m_initialPoolSize;
    }



    public GameObject GetPooledObject()
    {
        foreach (GameObject _obj in m_pool)
        {
            if (!_obj.activeInHierarchy)
            {
                _obj.SetActive(true);
                return _obj;
            }
        }
        
        // GameObject obj = (GameObject)Instantiate(m_prefab,
        //                                         _transform.position, 
        //                                         //Quaternion.identity
        //                                         _transform.rotation);

        GameObject obj_ = (GameObject) Instantiate(m_prefab, m_transform);
        if (obj_ != null) m_pool.Add(obj_);
        return obj_;
    }

    //! Not yet use
    public void ReturnPooledObject(GameObject _obj)
    {
        _obj.SetActive(false);
    }


}