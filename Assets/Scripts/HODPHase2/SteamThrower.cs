using System;
using System.Collections;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class SteamThrower : MonoBehaviour
{

    [SerializeField] float damage;

    [SerializeField] float life = 100f;
    GameObject Head;
    ParticleSystem[] vents;

    AudioSource Music;
    Coroutine dmg;
    Vector2 vv;
    bool isDown = true, isDead = false;

    float stime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stime = Time.time;
        Head = transform.GetChild(0).gameObject;
        Debug.Log(Head.name);
        vents = GetComponentsInChildren<ParticleSystem>();
        vv = new Vector2(0,0);
        Music = GetComponent<AudioSource>();
        StartCoroutine(Function());
    }

     void OnTriggerEnter2D(Collider2D collision)
     {
        if(isDown) return;
        Debug.Log("It's time to start");
        if(collision.tag == "Player")
          dmg = StartCoroutine(Damage(collision));
     }

     void OnTriggerExit2D(Collider2D collision)
     {
        if(dmg != null)
            StopCoroutine(dmg);
     }

     IEnumerator Damage(Collider2D player)
     {
        while (!isDown)
        {
            player.GetComponent<HealthScript>().DealDamage(damage);
            yield return new WaitForSeconds(1f);
        }
     }


    IEnumerator Function()
    {
        while(true){
            vv.x = 0;
            vv.y = 0.39f;
            Head.transform.localPosition = vv;
            yield return new WaitForSecondsRealtime(0.8f);
            isDown = false;
            Music.Play();
            vents[0].Play();
            vents[1].Play();
            yield return new WaitForSecondsRealtime(4f);
            vents[0].Stop();
            vents[1].Stop();
            Music.Stop();
            isDown = true;
            yield return new WaitForSecondsRealtime(0.8f);
            vv.x = 0;
            vv.y = 0.19f;
            Head.transform.localPosition = vv;
            yield return new WaitForSecondsRealtime(1f);
        }
    }
     // Update is called once per frame
     void Update()
    {
        if(isDead) return;
        if(Time.time - stime > life)
        {
            StopAllCoroutines();
            isDown = true;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        isDead = true;
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
