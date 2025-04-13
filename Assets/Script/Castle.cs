using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{

    [SerializeField] String m_nextSceneName;
    Player m_player;
    LevelManager m_levelManager;

    void Awake() 
    {
        m_player = FindObjectOfType<Player>();
        m_levelManager = FindObjectOfType<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if((1 << other.gameObject.layer) == LayerMask.GetMask("Player"))
            StartCoroutine(IENextLevel());
    }

    IEnumerator IENextLevel()
    {

        m_player.Pose();
        yield return new WaitForSeconds(1f);

        m_levelManager.LoadNameScene(m_nextSceneName);
        
    }


}
