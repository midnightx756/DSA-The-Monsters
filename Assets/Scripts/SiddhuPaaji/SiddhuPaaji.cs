using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SiddhuPaaji : MonoBehaviour
{
    
    HealthScript hs;

    bool isDead = false;

    GameObject leg1, leg2, arm1, arm2;

    [SerializeField] float inattackpadding;

    [Header("Manual Leg Animation")]
        [SerializeField] Sprite legUp;
        [SerializeField] Sprite legDown;
        [SerializeField] float Speed;
        [SerializeField] float distanceToMaintain;

    [Header("Attack 1 Machine Gun")]
        [SerializeField] float FireRate;
        [SerializeField] GameObject bullet;
        [SerializeField] int ammo;
        [SerializeField]  AudioClip fireSound;
        [SerializeField] [Range(0f, 1f)] float firevolume;
        Vector2 pos;
        Transform Barell;

        bool isAttacking = false;

    [Header("Attack 2 Smash Attack")]
        [SerializeField] float verticalYVelocity = 10f;
        [SerializeField] GameObject Shockwave;
        [SerializeField] float ShakeDur= 1f, ShakeMag = 3f;
        [SerializeField] AudioClip Shock;
        [SerializeField] [Range(0f, 1f)] float shockvolume;

    [Header("Attack 3 Missile")]
        [SerializeField] GameObject Missile;
        [SerializeField] int number;
        [SerializeField] float cooldown;
        [SerializeField] Sprite normal, launch;
    GameObject player;

    Coroutine w;

    float crab, yr, roda;
    Vector2 util, util2;
    Rigidbody2D rb2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hs = GetComponent<HealthScript>();
        leg1 = transform.GetChild(3).gameObject;
        leg2 = transform.GetChild(4).gameObject;
        arm1 = transform.GetChild(1).gameObject;
        arm2 = transform.GetChild(2).gameObject;
        Barell = arm1.transform.GetChild(0);
        player = GameObject.FindWithTag("Player");
        rb2D = GetComponent<Rigidbody2D>();
        util = new Vector2(0,0);
        pos = new Vector2(0,0);
        util2 = new Vector2(0,0);
        yr = transform.position.y;
        StartCoroutine(Attaxk());
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
            roda = transform.position.x - player.transform.position.x;
            if(Mathf.Abs(roda) > inattackpadding)
            {
                util2.x = player.transform.position.x + Mathf.Sign(roda) * inattackpadding;
                util2.y = (transform.position.y > yr) ? transform.position.y : yr;
                transform.position = util2;
            }
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
        util.y = yr;
        transform.position = util;
   }

   IEnumerator Death()
    {
        isDead = true;
        player.GetComponent<PlayerMovement>().follow = true;
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
        StopWalk();
        for(int i = 1; i<= ammo; i++)
        {
            pos = player.transform.position;
            pos = transform.localScale;
            pos.x *= -1;
            if(player.transform.position.x > transform.position.x)
            {
                if(transform.localScale.x >= 0)
                {
                    transform.localScale = pos;
                }
            }
            else
            {
                if(pos.x < 0) pos.x = -pos.x;
                transform.localScale = pos;
            }
            crab = Mathf.Atan2(Mathf.Abs(player.transform.position.x - arm1.transform.position.x), arm1.transform.position.y - player.transform.position.y);
            arm1.transform.rotation = Quaternion.Euler(0,0,crab / Mathf.PI *  -180 * Mathf.Sign(transform.localScale.x)); 
            if(Barell == null)
                Debug.Log("Barell missing");
            else if(bullet == null)
                Debug.Log("Add a bullet prefab");
            else
            {
                GameObject inst = Instantiate(bullet);
                AudioSource.PlayClipAtPoint(fireSound, Camera.main.transform.position, firevolume);
                inst.transform.SetPositionAndRotation(Barell.position,  arm1.transform.rotation);
                pos = inst.transform.localScale;
                pos.x *= (transform.localScale.x > 0)? -1 : 1;
                inst.transform.localScale = pos;
            }
            yield return new WaitForSecondsRealtime(1/FireRate);
        }
        arm1.transform.rotation = Quaternion.identity;
        isAttacking = false;
    }

    IEnumerator GroundSmash()
    {
        isAttacking = true;
        StopWalk();
        yield return new WaitForSecondsRealtime(0.5f);
        pos.x = -0.05210066f;
        pos.y = -3.28f;
        leg1.GetComponent<SpriteRenderer>().sprite = legUp;
        leg2.GetComponent<SpriteRenderer>().sprite = legUp;
        leg1.GetComponent<BoxCollider2D>().offset =  pos;
        leg2.GetComponent<BoxCollider2D>().offset =  pos;
        rb2D.linearVelocityY = verticalYVelocity;
        crab = verticalYVelocity* verticalYVelocity/ (2* rb2D.gravityScale* Physics2D.gravity.y);
        Debug.Log("Distance: " + crab + " Time: "+ verticalYVelocity/ rb2D.gravityScale* Physics2D.gravity.y + Mathf.Sqrt(crab* 2 / ( rb2D.gravityScale* Physics2D.gravity.y)));
       //yield return new WaitForSecondsRealtime(verticalYVelocity/ rb2D.gravityScale* Physics2D.gravity.y + Mathf.Sqrt(crab* 2 / ( rb2D.gravityScale* Physics2D.gravity.y)));
       Debug.Log("Velocity: " + verticalYVelocity + "Gravity: " + rb2D.gravityScale* Physics2D.gravity.y + "Time: " +  2 * verticalYVelocity / (rb2D.gravityScale* Physics2D.gravity.y * -1) );
       yield return new WaitForSecondsRealtime(2 * verticalYVelocity / (rb2D.gravityScale* Physics2D.gravity.y* -1) );
       StartCoroutine(CameraShake());
       AudioSource.PlayClipAtPoint(Shock, Camera.main.transform.position, shockvolume);
        if(Shockwave != null)
        {
            GameObject gb = Instantiate(Shockwave);
            pos.y = gb.transform.position.y;
            pos.x = transform.position.x;
            gb.transform.position = pos;

            //Opposite one
            gb = Instantiate(Shockwave);
            pos.y = gb.transform.position.y;
            pos.x = transform.position.x;
            gb.transform.position = pos;
            pos = gb.transform.localScale;
            pos.x *= -1;
            gb.transform.localScale = pos;
        }

       yield return new WaitForSecondsRealtime(0.5f);
        pos.y = -4.496043f;
        leg1.GetComponent<BoxCollider2D>().offset =  pos;
        leg2.GetComponent<BoxCollider2D>().offset =  pos;
        leg1.GetComponent<SpriteRenderer>().sprite = legDown;
        leg2.GetComponent<SpriteRenderer>().sprite = legDown;
        yield return new WaitForSecondsRealtime(0.8f);
        isAttacking = false;

    }

    IEnumerator MissilerRoutine()
    {
        if(isDead) yield return null;
        isAttacking = true;
        yield return new WaitForSeconds(0.8f);
        GameObject tor,  inst, tr;
        tor = transform.GetChild(0).gameObject;
        tor.GetComponent<SpriteRenderer>().sprite = launch;
        tr = tor.transform.GetChild(0).gameObject;
        for(int i = 1; i<= number; i++)
        {
            if(isDead || Missile == null) yield return null;
            inst = Instantiate(Missile);
            inst.transform.position = tr.transform.position;
            inst = Instantiate(Missile);
            inst.transform.position = tr.transform.position;
            yield return new WaitForSecondsRealtime(cooldown);
        }
        tor.GetComponent<SpriteRenderer>().sprite = normal;
        isAttacking = false;
        Debug.Log("Missiler ends here" + " isAttacking: " + isAttacking);
    }
    
    IEnumerator yank()
    {
        isAttacking = true;
        if(isDead) yield return null;
        yield return new WaitForSecondsRealtime(4f);
        arm2.GetComponent<ChainArm>().FireChain();
        yield return new WaitForSecondsRealtime(4f);
        isAttacking = false;
    }
    void StopWalk()
    {
            if(w != null){
                StopCoroutine(w);
                w = null;
            }
        leg1.GetComponent<SpriteRenderer>().sprite = legDown;
        leg2.GetComponent<SpriteRenderer>().sprite = legDown;
    }

    IEnumerator CameraShake()
    {
        if(isDead) yield return null;
        GameObject PF  = GameObject.FindWithTag("CameraFollow");
        player.GetComponent<PlayerMovement>().follow = false;
        Vector3 initpos = PF.transform.position;
        float elapsed = 0;
        while(elapsed < ShakeDur)
        {
            if(isDead) yield return null;
            PF.transform.position = initpos + (Vector3)UnityEngine.Random.insideUnitCircle *  ShakeMag;
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        player.GetComponent<PlayerMovement>().follow = true;
        PF.transform.position = initpos;
    }

    IEnumerator Attaxk()
    {
        while(true){
            if(isDead) yield return null;
            if(player == null)
            {
                yield return null;
            }
            else
            {
                if(player.GetComponent<PlayerMovement>() == null || player.GetComponent<HealthScript>() ==null || player.GetComponent<Rigidbody2D>() == null)
                    yield return null;
            }
            if(!isAttacking){
                Debug.Log("Is;nt attacking");
                if (player.GetComponent<PlayerMovement>().isUsingGun)
                {
                    Debug.Log("Starting the ground Smash");
                    yield return StartCoroutine(GroundSmash());
                }
                else if(player.GetComponent<HealthScript>().gethealth() < 20)
                {
                    Debug.Log("Firing Bullets");
                    yield return StartCoroutine(Fire());
                }
                else if(player.GetComponent<Rigidbody2D>().linearVelocityX == 0)
                {
                    Debug.Log("Starting the missiles");
                    yield return StartCoroutine(MissilerRoutine());
                }
                yield return new WaitForSecondsRealtime(1.5f);
            }
            else
            {
                Debug.Log("LOl An attack is going on, pls wait");
                yield return new WaitForSecondsRealtime(1.5f);
            }
        }
    }

    IEnumerator lol()
    {
        yield return new WaitForSecondsRealtime(5f);
        if(!isAttacking)
          StartCoroutine(MissilerRoutine());
        yield return new WaitForSecondsRealtime(15f);
        if(!isAttacking)
         StartCoroutine(Fire());
        yield return new WaitForSecondsRealtime(10f);
        if(!isAttacking)
         StartCoroutine(GroundSmash());
        yield return new WaitForSecondsRealtime(10f);
    }
}

internal class SerilaizeFiledAttribute : Attribute
{
}