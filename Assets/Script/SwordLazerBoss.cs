using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordLazerBoss : MonoBehaviour
{
    SpriteRenderer m_spR;
    // Start is called before the first frame update
    void Awake()
    {
        m_spR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        m_spR.color = new Color(m_spR.color.r,
                                m_spR.color.g, 
                                m_spR.color.b, 
                                (m_spR.color.a + 0.33f) % 1f);
    }
}
