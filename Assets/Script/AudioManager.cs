using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum EffectAudio
    {
        BrickBroken = 0,
        KoopaShell,
        Coin,
        QuestionBlock,
        QuestionBlockNone,
        Cannon,
        FireBall,
        FlagDown,
        JumpLv1,
        JumpLv2,
        MarioDeath,
        UpLvlMushroom,
        LostALive,
        GameOver,
        
        Count
    }


    public enum BackgroundMusic
    {
        Menu = 0,
        Ingame
    }

    List<AudioClip> m_listAudioCLips;
    List<AudioClip> m_listBackgroundMusicCLips;
    float           volumeEffect;
    AudioSource     m_audioSource;
    public static AudioManager m_instance{get; private set;}




    void Awake()
    {
        if(FindObjectsOfType(this.GetType()).Count() > 1)
        {
            gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        else 
        {
            m_audioSource = GetComponent<AudioSource>();
            StartCoroutine(IEWaitResourceGame());
            m_instance = this;

            //! Old
            // initAudioEffect();
            // initBackgroundMusic();

            DontDestroyOnLoad(this.gameObject);
        }
    }

    //! Old
    void initAudioEffect()
    {
        m_listAudioCLips    = new List<AudioClip>();
        volumeEffect        = Config.m_volumeEffect;

        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/Brick_Block"));
        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/Koopa_Shell"));
        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/Mario_Coin"));
        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/Question_Block_(Long_Ver.)"));
        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/Question_Block_Hit"));
        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/SMB_Cannon"));
        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/SMB_Fireball_Sound"));
        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/SMB_Flagpole_Sound"));
        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/SMB_Jump_Sound_(Ver._1)"));
        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/SMB_Jump_Sound_(Ver._2)"));
        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/Super_Mario_Bros._Death_Sound"));
        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/Super_Mushroom"));
        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/Lose_a_life_SMB"));
        m_listAudioCLips.Add((AudioClip) Resources.Load("Sounds/Effects/Game_Over_SMB"));
    }

    //! Old
    void initBackgroundMusic()
    {
        if(m_audioSource != null)
            m_audioSource.volume = Config.m_volumeBackgroundMusic;
            
        m_listBackgroundMusicCLips = new List<AudioClip>();

        m_listBackgroundMusicCLips.Add((AudioClip) Resources.Load("Sounds/Musics/SMAS_-_Game_Select"));
        m_listBackgroundMusicCLips.Add((AudioClip) Resources.Load("Sounds/Musics/SMBDX_Overworld_Theme"));

    }

    IEnumerator IEWaitResourceGame()
    {
        yield return new WaitWhile(() => ResourceGame.m_instance == null);
        InitAudioEffect(ResourceGame.GetListPrefabByType(ResourceGame.ETypeResource.AudioEffect));
        InitBackgroundMusic(ResourceGame.GetListPrefabByType(ResourceGame.ETypeResource.BackgroundMusic));
        
        Debug.Log("Init AudioManager success!");

    }


    public void InitAudioEffect(List<KeyValuePair<string, UnityEngine.Object>> list)
    {
        
        m_listAudioCLips    = new List<AudioClip>();
        volumeEffect        = Config.m_volumeEffect;

        for(int index = 0; index < list.Count; index++)
            m_listAudioCLips.Add((AudioClip)list[index].Value);
            
    }

    public void InitBackgroundMusic(List<KeyValuePair<string, UnityEngine.Object>> list)
    {
        if(m_audioSource != null)
            m_audioSource.volume = Config.m_volumeBackgroundMusic;
            
        m_listBackgroundMusicCLips = new List<AudioClip>();

        for(int index = 0; index < list.Count; index++)
            {
                m_listBackgroundMusicCLips.Add((AudioClip)list[index].Value);
                Debug.Log("Index: " + index);
            }

    }




    public void playEffectAudio(EffectAudio effectAudio)
    {
        if(m_listAudioCLips[(int)effectAudio] != null)
            AudioSource.PlayClipAtPoint(m_listAudioCLips[(int)effectAudio],
                                        Camera.main.transform.position, 
                                        volumeEffect);

    }

    public void playBackGroundMusic(BackgroundMusic music)
    {
        if(m_listBackgroundMusicCLips[(int)music] != null)
        {
            m_audioSource.Stop();
            m_audioSource.clip = m_listBackgroundMusicCLips[(int)music];
            m_audioSource.loop = true;
            m_audioSource.Play();
        }

    }

    public void stopBackgroungMusic()
    {
        m_audioSource.Stop();
    }

    public void onVolumeEffectChanged(float value)
    {
        volumeEffect = value;
    }

    public void onVolumeBackgroundMusicChanged(float value)
    {
        m_audioSource.volume = value;
    }

    public float valueVolumeEffect()
    {
        return volumeEffect;
    }
    
    public float valueVolumeBackgroundMusic()
    {
        return m_audioSource.volume;
    }




}
