using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public enum LevelName
    {
        Menu = 0,
        Settings,
        GameOver,
        Level1,
        Level2,
        Level3,
        Level4
    }

    List<KeyValuePair<string, float>> m_listLevel = new List<KeyValuePair<string, float>>();

    ScoreKeeper             m_scoreKeeper;
    static bool             m_isGameOver = false;
    AudioManager            m_audioManager;

    [SerializeField]Slider  m_sliderVolumeEffect;
    [SerializeField]Slider  m_sliderVolumeBackgroundMusic;
    private void Awake() 
    {

        m_listLevel.Add(new KeyValuePair<string, float>("Scenes/Menu",      1f));
        m_listLevel.Add(new KeyValuePair<string, float>("Scenes/Settings",  1f));
        m_listLevel.Add(new KeyValuePair<string, float>("Scenes/GameOver",  2f));
        m_listLevel.Add(new KeyValuePair<string, float>("Scenes/Level 1",   2f));
        m_listLevel.Add(new KeyValuePair<string, float>("Scenes/Level 2",   2f));
        m_listLevel.Add(new KeyValuePair<string, float>("Scenes/Level 3",   2f));
        m_listLevel.Add(new KeyValuePair<string, float>("Scenes/Level 4",   2f));



        StartCoroutine(IEWaitInitFromAnother());

    }

    IEnumerator IEWaitInitFromAnother()
    {

        yield return new WaitWhile(() => ScoreKeeper.m_instance == null);
        m_scoreKeeper = ScoreKeeper.m_instance;

        
        yield return new WaitWhile(() => AudioManager.m_instance == null);
        m_audioManager = AudioManager.m_instance;

        if(m_isGameOver && m_listLevel[(int)LevelName.GameOver].Key.Contains(SceneManager.GetActiveScene().name))
            m_audioManager.playEffectAudio(AudioManager.EffectAudio.GameOver);
        
        if(m_listLevel[(int)LevelName.Menu].Key.Contains(SceneManager.GetActiveScene().name))
            m_audioManager.playBackGroundMusic(AudioManager.BackgroundMusic.Menu);

        if(m_listLevel[(int)LevelName.Level1].Key.Contains(SceneManager.GetActiveScene().name) ||
            m_listLevel[(int)LevelName.Level2].Key.Contains(SceneManager.GetActiveScene().name) ||
            m_listLevel[(int)LevelName.Level3].Key.Contains(SceneManager.GetActiveScene().name))
                    m_audioManager.playBackGroundMusic(AudioManager.BackgroundMusic.Ingame);
        
        initVolume();
        
        Debug.Log("Init LevelManager success!");
    }

    void initVolume()
    {
        if(m_sliderVolumeEffect != null)
            m_sliderVolumeEffect.value = m_audioManager.valueVolumeEffect();

        if(m_sliderVolumeBackgroundMusic != null)
            m_sliderVolumeBackgroundMusic.value = m_audioManager.valueVolumeBackgroundMusic();
    }




    public void LoadScene(LevelName level)
    {
        StartCoroutine(IELoadScene(level));
    }

    public void LoadNameScene(String nameScene)
    {
        for(int i = 0; i < m_listLevel.Count; i++)
            if(m_listLevel[i].Key.Contains(nameScene))
                StartCoroutine(IELoadScene((LevelName) i));
    }


    IEnumerator IELoadScene(LevelName level)
    {
        yield return new WaitForSeconds(m_listLevel[(int)level].Value);
        SceneManager.LoadScene(m_listLevel[(int)level].Key);
    }


    public void LoadSceneMenu()
    {
        LoadScene(LevelName.Menu);
    }

    public void LoadSceneSettings()
    {
        LoadScene(LevelName.Settings);
    }


    public void reloadScene()
    {
        if(m_scoreKeeper.getLives() == 0)
        {
            m_isGameOver = true;
            LoadScene(LevelName.GameOver);
        }
        else
        {
            m_scoreKeeper.decreaseLives();
            LoadNameScene(SceneManager.GetActiveScene().name);
        }
    }


    public void PlayAgain()
    {
        m_scoreKeeper.resetLives();
        m_scoreKeeper.resetScore();
        m_scoreKeeper.resetCoin();

        LoadSceneLevel1();
    }


    public void Quit()
    {
        Application.Quit();
    }



    public void onVolumeEffectChanged(float value)
    {
        m_audioManager.onVolumeEffectChanged(value);
    }

    public void onVolumeBackgroundMusicChanged(float value)
    { 
        m_audioManager.onVolumeBackgroundMusicChanged(value);
    }

    public void LoadSceneLevel1()
    {
        m_isGameOver = false;
        SceneManager.LoadScene("Scenes/Level 1");
        // SceneManager.LoadScene("Scenes/Level 4");
    }


}
