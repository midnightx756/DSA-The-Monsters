using System;
using System.Collections;
using UnityEngine;

public class Effects : MonoBehaviour
{

    [SerializeField] float DPS;
    [SerializeField] float lifetime;
    [SerializeField] AudioClip Zap;
    [SerializeField] [Range(0f, 1f)] float volume;

    ParticleSystem ps;

    CircleCollider2D cc;
    Coroutine dmg;
    bool isRunning = false, isEffect = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ps = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        cc = GetComponent<CircleCollider2D>();
        Destroy(gameObject, lifetime);
    }


     void OnDestroy()
     {
        StopAllCoroutines();
        Debug.Log("Lala");
     }
     // Update is called once per frame
     void Update()
    {
        if (!isEffect)
        {
            StartCoroutine(Effect());
        }
        /*if (!cc.IsTouchingLayers(LayerMask.NameToLayer("Player")))
        {
            Debug.Log("Player exited");
            if(dmg != null)
                StopCoroutine(dmg);
            dmg = null;
            isRunning = false;
        }*/
    }

     void OnTriggerEnter2D(Collider2D collision)
     {
        Debug.Log("enter Player");
        if(collision.gameObject.tag == "Player")
        {
            if (!isRunning)
            {
                Debug.Log("Starting the damage routine");
                dmg = StartCoroutine(Damage(collision));
            }
            else
            {
                Debug.Log("COroutine already running");
            }
        }
        /*else
        {
            Debug.Log(collision.tag);
            if(dmg != null)
            {
                StopCoroutine(dmg);
                dmg = null;
                isRunning = false;
            }
        }*/
     }

     void OnTriggerExit2D(Collider2D collision)
     {
        Debug.Log("PlayerExited");
         isRunning = false;
          if(dmg == null)
        {
            Debug.Log("Either the player is running or the coroutine is null");
            return;
        }
        StopCoroutine(dmg);
        dmg = null;
     }

     IEnumerator Damage(Collider2D player)
    {
        isRunning = true;
        if(player == null)
        {
            Debug.Log("Player is null returning");
            yield return null;
        }
        HealthScript hs = player.GetComponent<HealthScript>();
        if(hs == null) yield return null;
        while (true)
        {
            hs.DealDamage(DPS);
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    IEnumerator Effect()
    {
        isEffect = true;
        AudioSource.PlayClipAtPoint(Zap, Camera.main.transform.position, volume);
        ps.Play();
        yield return new WaitForSecondsRealtime(2f);
        ps.Stop();
        yield return new WaitForSecondsRealtime(2f);
        isEffect = false;
    }
}
