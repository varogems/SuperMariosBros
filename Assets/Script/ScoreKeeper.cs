using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] int m_score;
    [SerializeField] int m_coin;
    [SerializeField] int m_lives;
    
    public static ScoreKeeper m_instance {get; private set;}



    private void Awake() 
    {
        if(FindObjectsOfType(this.GetType()).Length > 1)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            return;
        }
        else 
        {
            m_score     = 0;
            m_coin      = 0;
            m_lives     = Config.m_PlayerLives;
            m_instance  = this;
            
            Debug.Log("Init ScoreKeeper success!");

            DontDestroyOnLoad(this.gameObject);

        }

    }

    public void decreaseLives()
    {
        m_lives--;
    }

    public void resetLives()
    {
        m_lives = Config.m_PlayerLives;
    }

    public int getLives()
    {
        return m_lives;
    }




    public void collectScore(int score)
    {
        m_score += score;
        Mathf.Clamp(m_score, 0 , int.MaxValue);
    }
    public int getScore()
    {
        return  m_score;
    }

    public void resetScore()
    {
        m_score = 0;
    }

    


    public void collectCoin()
    {
        m_coin++;
        Mathf.Clamp(m_coin, 0 , int.MaxValue);
    }

    public int getCoin()
    {
        return  m_coin;
    }

    public void resetCoin()
    {
        m_coin = 0;
    }

}
