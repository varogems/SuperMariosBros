using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{

    Player m_player;
    void Awake()
    {
        StartCoroutine(IEWaitGetPlayer());
    }

    IEnumerator IEWaitGetPlayer()
    {
        yield return new WaitWhile(() => 
        {
            m_player = FindObjectOfType<Player>(); 
            return m_player == null;
        });
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if((1 << other.gameObject.layer) == LayerMask.GetMask("Player") && (m_player != null))
            m_player.ImmediatelyDie();
    }
}
