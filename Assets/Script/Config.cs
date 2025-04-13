using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Config : MonoBehaviour
{
    public static Config m_instance{get; private set;}
    [SerializeField] public static float minAngleCrashInto;
    [SerializeField] public static float maxAngleCrashInto;



    [Header("Player")]
    [SerializeField] public static float m_PlayerSpeedRun;
    [SerializeField] public static float m_PlayerSpeedLinear;
    [SerializeField] public static float m_PlayerMaxSpeedLinear;
    [SerializeField] public static float m_PlayerForceJump;
    [SerializeField] public static float m_PlayerMaxHeightJump;
    [SerializeField] public static float m_PlayerTimeFiring;
    [SerializeField] public static float m_PlayerTimePosing;
    [SerializeField] public static int   m_PlayerLives;
    [SerializeField] public static float m_PlayerTimeHurt;
    [SerializeField] public static float m_PlayerTimeInvicibleAfterBeHurted;
    [SerializeField] public static float m_PlayerTimeInvicibleItem;
    [SerializeField] public static float m_PlayerTimeUpform;
    [SerializeField] public static int m_PlayerDamageStomp;
    [SerializeField] public static int m_PlayerDamageFireBullet;
    


    [Header("TurtleCannon/BulletBill")]
    [SerializeField] public static float m_TCDelayTimeShoot;
    [SerializeField] public static float m_BulletBillSpeedMove;
    [SerializeField] public static int m_BulletBillScore;


    [Header("Flag")]
    [SerializeField] public static float m_FlagSpeedDown;
    [SerializeField] public static float m_FlagDistanceMove;
    


    [Header("Goomba")]
    [SerializeField] public static float m_GoombaSpeedMove;
    [SerializeField] public static float m_GoombaTimeDie;
    [SerializeField] public static int m_GoombaScore;


    [Header("Koopa Troopa")]
    [SerializeField] public static float m_KTSpeedMove;
    [SerializeField] public static float m_KTSpeedFly;
    [SerializeField] public static float m_KTSpeedBeKicked;
    [SerializeField] public static float m_KTMaxHeightCanFly;
    [SerializeField] public static float m_KTTimeActive;
    [SerializeField] public static float m_KTTimeTurnArround;
    [SerializeField] public static int m_KTScore;
    

    [Header("Mushroom")]
    [SerializeField] public static float m_MSpeedUp;
    [SerializeField] public static float m_MSpeedMove;


    [Header("Spiny")]
    [SerializeField] public static float m_SpinySpeedMove;
    [SerializeField] public static float m_SpinyTimeTurnArround;
    [SerializeField] public static int m_SpinyScore;


    [Header("PiranhaPlant")]
    [SerializeField] public static int m_PiranhaPlantScore;


    [Header("Volume Audio")]
    [SerializeField] public static float m_volumeEffect;
    [SerializeField] public static float m_volumeBackgroundMusic;

    private void Awake() 
    {
        if(FindObjectsOfType(this.GetType()).Length > 1)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        else 
        {
            Init();
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

    }



    void Init()
    {
        //! Angle crash into
        minAngleCrashInto = 60f;
        maxAngleCrashInto = 89f;


        //! Player
        m_PlayerSpeedRun        = 5f;
        m_PlayerSpeedLinear     = 1f;
        m_PlayerMaxSpeedLinear  = 8f;
        m_PlayerForceJump       = 222f;
        
        m_PlayerMaxHeightJump   = 5f;
        // m_PlayerMaxHeightJump   = 4f;
        
        m_PlayerTimeFiring      = 0.15f;
        m_PlayerTimePosing      = 0.33f;
        m_PlayerLives           = 1;
        m_PlayerTimeHurt        = 0.6f;
        m_PlayerTimeUpform      = 1f;
        m_PlayerTimeInvicibleAfterBeHurted  = 1f;
        m_PlayerTimeInvicibleItem       = 8f;
        m_PlayerDamageStomp      = 2;
        m_PlayerDamageFireBullet = 1;


        //! Turtle Cannon/Bullet Bill
        m_TCDelayTimeShoot      = 4f;
        m_BulletBillSpeedMove   = 3f;
        m_BulletBillScore       = 100;

        //! Flag
        m_FlagSpeedDown         = .01f;
        m_FlagDistanceMove      = 7f;

        //! Goomba
        m_GoombaSpeedMove       = -2.2f;
        m_GoombaTimeDie         = .7f;
        m_GoombaScore           = 100;


        //! Koopa Troopa
        m_KTSpeedMove           = -2.2f;
        m_KTSpeedFly            = 2.2f;
        m_KTMaxHeightCanFly     = 2f;
        m_KTTimeActive          = 5f;
        m_KTSpeedBeKicked       = 15f;
        m_KTTimeTurnArround     = 0.4f;
        m_KTScore               = 100;

        //! Mushroom
        m_MSpeedMove    = 2.2f;
        m_MSpeedUp      = 2f;

        //! Spiny
        m_SpinySpeedMove        = -2.2f;
        m_SpinyTimeTurnArround  = .5f;
        m_SpinyScore            = 100;

        //! PiranhaPlant
        m_PiranhaPlantScore = 50;

        //!Volume Audio
        m_volumeEffect          = 0.5f;
        m_volumeBackgroundMusic = 0.5f;
    }


    public static float GetAngleCrashInto()
    {
        return UnityEngine.Random.Range(minAngleCrashInto, maxAngleCrashInto);
    }





}

