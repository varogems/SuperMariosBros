using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    public enum PoolType
    {
        BulletPlayer = 0,   //FireBullet
        BulletBoss,         //TwinSlasher
        BulletBill,

        Count,
    };

    //!------------------------------------------------------------------------
    static List<KeyValuePair<int, GameObject>>        m_listObjectPool;
    static Transform m_transform;
    
    //!------------------------------------------------------------------------
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
            
            m_transform = transform;
            StartCoroutine(IEWaitAwakeResource());

            DontDestroyOnLoad(this.gameObject);
            
        }
    }

    IEnumerator IEWaitAwakeResource()
    {
        // while(ResourceGame.m_instance == null)
        //     yield return new WaitForSeconds(0.1f);

        yield return new WaitWhile(() => ResourceGame.m_instance == null);

        Init();
        
    }

    public static void Init()
    {
        m_listObjectPool = new List<KeyValuePair<int, GameObject>>();

        GameObject gameObject;
        for(int i = 0; i < (int)PoolType.Count; i++)
        {
            //! Create Gameobject with component ObjectPool
            gameObject              = new GameObject();
            gameObject.name         = ResourceGame.GetNamePrefabByIndex(i);
            gameObject.AddComponent<ObjectPool>();
            gameObject.GetComponent<ObjectPool>().setPrefab(ResourceGame.GetPrefabByIndex(i));

            //!  Add this gameobject to gameobject with name "PoolManager"
            m_listObjectPool.Add(new KeyValuePair<int, GameObject>(i, gameObject));
            gameObject.transform.SetParent(m_transform);
        }
        gameObject = null;
        
        Debug.Log("Init PoolManager success!");
    }


    //! Spawn bullet for boss
    public static void SpawnTwinSlasher(Transform _transformBoss)
    {
        ObjectPool pool = m_listObjectPool[(int)PoolType.BulletBoss].Value.GetComponent<ObjectPool>();
        int numberOfBulletBossSpawn = Random.Range(2, pool.numberOfGameObject());

        float angle = 90 / numberOfBulletBossSpawn;

        GameObject gameObject;
        BulletBoss bulletBoss;
        Vector2 vectorProjectile;

        for(int i = 0; i < numberOfBulletBossSpawn; i++)
        {
            vectorProjectile = Vector2.zero;

            vectorProjectile.x  =    (Mathf.Sign(_transformBoss.localScale.x) > 0) ? Mathf.Cos(angle * i * Mathf.Deg2Rad): 
                                                                                    Mathf.Cos((180f - angle * i)  * Mathf.Deg2Rad);

            vectorProjectile.y  =    Mathf.Sin((180f - angle * i)  * Mathf.Deg2Rad);

            gameObject                      = pool.GetPooledObject();
            gameObject.transform.position   = _transformBoss.position;
            gameObject.transform.rotation   = _transformBoss.rotation;
            
            bulletBoss = gameObject.GetComponent<BulletBoss>();
            bulletBoss.setDirectionVector(vectorProjectile);
        }

        gameObject = null;
        bulletBoss = null;

    }

    //! Spawn bullet for player
    public static void SpawnFireBullets(Transform _transformPlayer)
    {

        GameObject _fireBullet = m_listObjectPool[(int)PoolType.BulletPlayer].Value.GetComponent<ObjectPool>().GetPooledObject();

        Vector3 posAppear = new Vector3(_transformPlayer.position.x + _transformPlayer.localScale.x/2,
                                        _transformPlayer.position.y + 1 , 
                                        _transformPlayer.position.z);

        _fireBullet.transform.position      = posAppear;
        _fireBullet.transform.localScale    = _transformPlayer.localScale;
        _fireBullet = null;
        
    }


    public static void SpawnBulletBill(Transform _transformPlayer)
    {
        GameObject BulletBill           = m_listObjectPool[(int)PoolType.BulletBill].Value.GetComponent<ObjectPool>().GetPooledObject();
        BulletBill.GetComponent<MovingGameObject>().RefreshCollider();
        BulletBill.transform.position   = new Vector2(_transformPlayer.position.x + _transformPlayer.localScale.x * 1.2f, _transformPlayer.position.y + 1);
        BulletBill.transform.localScale = new Vector3(Mathf.Sign(_transformPlayer.transform.position.x - _transformPlayer.position.x), 1, 1);
    }





}
