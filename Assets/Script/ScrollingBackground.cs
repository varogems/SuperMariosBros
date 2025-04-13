using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

    [SerializeField] Vector2 speedScroll;

    Material                m_material;


    void Awake() 
    {
        m_material = this.GetComponent<SpriteRenderer>().material;
    }




    void Update()
    {
        m_material.mainTextureOffset += speedScroll* Time.deltaTime;
    }
}
