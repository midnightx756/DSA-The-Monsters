using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SiddhuPaaji : MonoBehaviour
{
    
    HealthScript hs;

    bool isDead = false;

    GameObject leg1, leg2, arm1;

    [Header("Manual Leg Animation")]
        [SerializeField] Sprite legUp;
        [SerializeField] Sprite legDown;
        [SerializeField] float Speed;
        [SerializeField] float distanceToMaintain;

    [Header("Attack 1 Machine Gun")]
        [SerializeField] float FireRate;
        [SerializeField] GameObject bullet;
        [SerializeField] int ammo;
        Vector2 pos;
        Transform Barell;

        bool isAttacking = false;
    GameObject player;

    Coroutine w;

    float crab;
    Vector2 util;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hs = GetComponent<HealthScript>();
        leg1 = transform.GetChild(3).gameObject;
        leg2 = transform.GetChild(4).gameObject;
        arm1 = transform.GetChild(1).gameObject;
        Barell = arm1.transform.GetChild(0);
        player = GameObject.FindWithTag("Player");
        util = new Vector2(0,0);
        pos = new Vector2(0,0);
        StartCoroutine(Fire());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
             hs.health = 0;
             return;
        }
        if (isAttacking){
            StopWalk();
            return;
        }
        if(hs.gethealth() <= 0)
        {
            hs.health = 0;
            StopAllCoroutines();
            StartCoroutine(Death());
        }
        if(player.GetComponent<Rigidbody2D>().linearVelocityX != 0)
        {
            if(w == null)
                w = StartCoroutine(walk());
        }
        else if (player.GetComponent<Rigidbody2D>().linearVelocityX == 0)
        {
            if(w != null){
                StopCoroutine(w);
                w = null;
                StopWalk();
            }
        }
        util.x = player.transform.position.x + distanceToMaintain;
        util.y = transform.position.y;
        transform.position = util;
   }

   IEnumerator Death()
    {
        isDead = true;
        ScoreKeeper sk = FindAnyObjectByType<ScoreKeeper>();
        if(sk != null)
            sk.UpdateScore(1000000);
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene("DarshanBattle");
    }

    IEnumerator walk()
    {
        if(isDead) yield return null;
        while (true)
        {
            leg1.GetComponent<SpriteRenderer>().sprite = legUp;
            yield return new WaitForSecondsRealtime(1/Speed);
            leg1.GetComponent<SpriteRenderer>().sprite = legDown;
           yield return new WaitForSecondsRealtime(1/Speed);
            leg2.GetComponent<SpriteRenderer>().sprite = legUp;
            yield return new WaitForSecondsRealtime(1/Speed);
            leg2.GetComponent<SpriteRenderer>().sprite = legDown;
            yield return new WaitForSecondsRealtime(1/Speed);
        }
    }


    IEnumerator Fire()
    {
        if(isDead) yield return null;
        isAttacking = true;
        for(int i = 1; i<= ammo; i++)
        {
            pos = player.transform.position;
            crab = Mathf.Atan2(Mathf.Abs(player.transform.position.x - arm1.transform.position.x), arm1.transform.position.y - player.transform.position.y);
            arm1.transform.rotation = Quaternion.Euler(0,0,crab / Mathf.PI *  -180); 
            if(Barell == null)
                Debug.Log("Barell missing");
            else if(bullet == null)
                Debug.Log("Add a bullet prefab");
            else
            {
                GameObject inst = Instantiate(bullet);
                inst.transform.SetPositionAndRotation(Barell.position,  arm1.transform.rotation);
                pos = inst.transform.localScale;
                pos.x *= (arm1.transform.localScale.x > 0)? -1 : 1;
                inst.transform.localScale = pos;
            }
            yield return new WaitForSecondsRealtime(1/FireRate);
        }
        arm1.transform.rotation = Quaternion.identity;
        isAttacking = false;
    }
    void StopWalk()
    {
        leg1.GetComponent<SpriteRenderer>().sprite = legDown;
        leg2.GetComponent<SpriteRenderer>().sprite = legDown;
    }
}