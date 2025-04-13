using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    Vector2 m_vectorDir;
    Rigidbody2D m_rbd2;
    Transform m_originTransform;
    // Start is called before the first frame update
    void Awake()
    {
        m_originTransform = transform;
        m_vectorDir = Vector2.zero;
        m_rbd2      = GetComponent<Rigidbody2D>();
    }
    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(m_vectorDir * 40f * Time.deltaTime);
    }



    public void setDirectionVector(Vector2 vectorDir)
    {
        transform.localScale = new Vector3(Mathf.Sign(vectorDir.x), transform.localScale.y, transform.localScale.z);
        m_vectorDir = vectorDir;
        gameObject.SetActive(true);

        StartCoroutine(IEHideBullet());
    }

    IEnumerator IEHideBullet()
    {
        yield return new WaitForSeconds(1f);
        transform.rotation = m_originTransform.rotation;
        gameObject.SetActive(false);
    }
}
