using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;


using UnityEngine.SceneManagement;

public class UIStatus : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI m_UILevel;
    [SerializeField] TextMeshProUGUI m_UIScore;
    [SerializeField] TextMeshProUGUI m_UICoin;
    [SerializeField] TextMeshProUGUI m_UILives;
    



    [SerializeField] ScoreKeeper m_scoreKeeper;

    // Start is called before the first frame update
    void Awake()
    {
        m_scoreKeeper   = FindObjectOfType<ScoreKeeper>();
    }

    // Update is called once per frame
    void Update()
    {

        if(m_scoreKeeper != null)
        {
            m_UIScore.text = "Score:\n" + m_scoreKeeper.getScore().ToString("000000000");
            m_UICoin.text = "Coin:\n" + m_scoreKeeper.getCoin().ToString("000");
            m_UILives.text = "Live:\n" + m_scoreKeeper.getLives().ToString("000");
        }

        m_UILevel.text = SceneManager.GetActiveScene().name;

    }
}
