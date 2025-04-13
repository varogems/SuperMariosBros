using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int m_point;
    ScoreKeeper m_scoreKeeper;
    AudioManager m_audioManager;

    private void Awake() 
    {
        m_scoreKeeper   = FindObjectOfType<ScoreKeeper>();
        m_audioManager  = FindObjectOfType<AudioManager>();
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        m_audioManager.playEffectAudio(AudioManager.EffectAudio.Coin);
        m_scoreKeeper.collectCoin();
        Destroy(this.gameObject);
    }
}
