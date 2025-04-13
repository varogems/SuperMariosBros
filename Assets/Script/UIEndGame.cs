using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class UIEndGame : MonoBehaviour
{

    [SerializeField]TextMeshProUGUI m_UIScore;
    ScoreKeeper                     m_scoreKeeper;

    private void Awake() 
    {
        m_scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    void Start() 
    {
        if(m_scoreKeeper != null)
            m_UIScore.text = "Your score:\n" + m_scoreKeeper.getScore().ToString("000000000");
    }
}
