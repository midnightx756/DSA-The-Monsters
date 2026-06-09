using System;
using System.Collections;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float health = 50f;
    float curhealth;

    void Awake()
    {
        curhealth = health;
    }
    public void DealDamage(float amt)
    {
        health -=amt;
    }

     public void Die()
     {
        Destroy(gameObject);
     }

     public void PermaBoost(float amt)
    {
        health += amt;
    }

    public float gethealth()
    {
        return health;
    }

    public void TempBoost(float boostAmt, float duration)
    {
        StartCoroutine(changeHealth(boostAmt, duration));
    }

    IEnumerator changeHealth(float boostAmt, float duration)
    {
        health +=boostAmt;
        //Debug.Log(health);
        yield return new WaitForSecondsRealtime(duration);
         health = curhealth;
    }
}
