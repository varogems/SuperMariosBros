using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGame : MonoBehaviour
{
    public enum ETypeResource
    {
        Prefab = 0,
        AudioEffect,
        BackgroundMusic
    }

    static List<KeyValuePair<int, List<KeyValuePair<string, UnityEngine.Object>>>> m_listResrc;
    public static ResourceGame m_instance {get; private set;}

    void Awake()
    {
        if(FindObjectsOfType(this.GetType()).Length > 1)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            return;
        }
        else 
        {
            Init();
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
    }

    static void Init()
    {
        
        LoadResource();
        
        // //! Create PoolManager gameobject.
        // GameObject poolManager = new GameObject("PoolManager");
        // poolManager.AddComponent<PoolManager>();
        // poolManager.transform.position = new Vector3(0, 0, 0);
        // poolManager.transform.rotation = Quaternion.identity;
        // poolManager.transform.localScale = Vector3.one;
        // PoolManager.Init();



        // //! Create AudioManager gameobject
        // GameObject audioManagerGO = new GameObject("AudioManager");
        // audioManagerGO.AddComponent<AudioSource>();
        // AudioSource audioSource = audioManagerGO.GetComponent<AudioSource>();
        // audioSource.playOnAwake = true;
        // audioSource.volume = 0.8f;
        
        // audioManagerGO.AddComponent<AudioManager>();
        // audioManagerGO.transform.position = new Vector3(0, 0, 0);
        // audioManagerGO.transform.rotation = Quaternion.identity;
        // audioManagerGO.transform.localScale = Vector3.one;
        
        // AudioManager audioManagerObjecScript = audioManagerGO.GetComponent<AudioManager>();
        // audioManagerObjecScript.InitAudioEffect(m_listResrc[(int)ETypeResource.AudioEffect].Value);
        // audioManagerObjecScript.InitBackgroundMusic(m_listResrc[(int)ETypeResource.BackgroundMusic].Value);
        
        Debug.Log("Init ResourceGame success!");
        
        
    }

    static void LoadResource()
    {
        //! Init m_listResrc
        m_listResrc = new List<KeyValuePair<int, List<KeyValuePair<string, UnityEngine.Object>>>>();

        //! Prefab for pool manager.
        List<KeyValuePair<string, UnityEngine.Object>> listPoolPrefab = new List<KeyValuePair<string, UnityEngine.Object>>();
        listPoolPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("FireBullet",     Resources.Load("Prefab/FireBullet")));
        listPoolPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("TwinSlasher",    Resources.Load("Prefab/TwinSlasher")));
        listPoolPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("BulletBill",     Resources.Load("Prefab/BulletBill")));

        m_listResrc.Add(new KeyValuePair<int, List<KeyValuePair<string, UnityEngine.Object>>>((int)ETypeResource.Prefab, listPoolPrefab));


        
        //! Prefab for audio effect.
        List<KeyValuePair<string, UnityEngine.Object>> listAudioPrefab = new List<KeyValuePair<string, UnityEngine.Object>>();
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("Brick_Block",                         Resources.Load("Sounds/Effects/Brick_Block")));
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("Koopa_Shell",                         Resources.Load("Sounds/Effects/Koopa_Shell")));
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("Mario_Coin",                          Resources.Load("Sounds/Effects/Mario_Coin")));
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("Question_Block",                      Resources.Load("Sounds/Effects/Question_Block_(Long_Ver.)")));
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("Question_Block_Hit",                  Resources.Load("Sounds/Effects/Question_Block_Hit")));
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("SMB_Cannon",                          Resources.Load("Sounds/Effects/SMB_Cannon")));
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("SMB_Fireball_Sound",                  Resources.Load("Sounds/Effects/SMB_Fireball_Sound")));
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("SMB_Flagpole_Sound",                  Resources.Load("Sounds/Effects/SMB_Flagpole_Sound")));
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("SMB_Jump_Sound_(Ver._1)",             Resources.Load("Sounds/Effects/SMB_Jump_Sound_(Ver._1)")));
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("SMB_Jump_Sound_(Ver._2)",             Resources.Load("Sounds/Effects/SMB_Jump_Sound_(Ver._2)")));
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("Super_Mario_Bros._Death_Sound",       Resources.Load("Sounds/Effects/Super_Mario_Bros._Death_Sound")));
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("Sounds/Effects/Super_Mushroom",       Resources.Load("Sounds/Effects/Super_Mushroom")));
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("Lose_a_life_SMB",                     Resources.Load("Sounds/Effects/Lose_a_life_SMB")));
        listAudioPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("Game_Over_SMB",                       Resources.Load("Sounds/Effects/Game_Over_SMB")));

        m_listResrc.Add(new KeyValuePair<int, List<KeyValuePair<string, UnityEngine.Object>>>((int)ETypeResource.AudioEffect, listAudioPrefab));




        
        //! Prefab for background music.
        List<KeyValuePair<string, UnityEngine.Object>> listBgMusicPrefab = new List<KeyValuePair<string, UnityEngine.Object>>();
        listBgMusicPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("SMAS_-_Game_Select",    Resources.Load("Sounds/Musics/SMAS_-_Game_Select")));
        listBgMusicPrefab.Add(new KeyValuePair<string, UnityEngine.Object>("SMBDX_Overworld_Theme", Resources.Load("Sounds/Musics/SMBDX_Overworld_Theme")));
        m_listResrc.Add(new KeyValuePair<int, List<KeyValuePair<string, UnityEngine.Object>>>((int)ETypeResource.BackgroundMusic, listBgMusicPrefab));

    }

    public static List<KeyValuePair<string, UnityEngine.Object>> GetListPrefabByType(ETypeResource type)
    {
        return m_listResrc[(int)type].Value;
    }

    public static UnityEngine.Object GetPrefab(string namePrefab)
    {
        foreach(KeyValuePair<string, UnityEngine.Object> _obj  in m_listResrc[(int)ETypeResource.Prefab].Value)
            if(_obj.Key.ToString() == namePrefab)
                return _obj.Value;
        return null;
    }

    public static int numberOfPool()
    {
        return m_listResrc.Count;
    }

    public static UnityEngine.Object GetPrefabByIndex(int index)
    {
        return m_listResrc[(int)ETypeResource.Prefab].Value[index].Value;
    }

    public static string GetNamePrefabByIndex(int index)
    {
        return m_listResrc[(int)ETypeResource.Prefab].Value[index].Key;
    }
}
