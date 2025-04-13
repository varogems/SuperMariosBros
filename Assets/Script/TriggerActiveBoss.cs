using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActiveBoss : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<SlashBeast>().ActiveBoss();
        Debug.Log("ActiveBoss SlashBeast");
        this.gameObject.SetActive(false);
    }
}
