using System.Data.Common;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

[Serializable]
public struct BulletInfo
{
    public GameObject bullet;
    public float damage;
}

public class D_man : MonoBehaviour
{
        [SerializeField] String targetTag;
        [SerializeField] GameObject ShockObject;
        [SerializeField] GameObject Missile;
        [SerializeField] float barellActiveTime = 0.5f;

        [SerializeField] float distance = 10f;


        [Header("HurtPart")]
            [SerializeField] AudioClip scream;
            [SerializeField] [Range(0f, 1f)] float screamVolume;

        [Header("BulletList")]
          [SerializeField] List<BulletInfo> bull;

        Transform playerTransform;
        GameObject Target;
        Rigidbody2D rb2d;
        CircleCollider2D handLCollider, handRCollider;

        GameObject Barell1, Barell2, legL, legR;
        LaserSpawner laser;

        bool CollisionAttack = false, isNotInAir = false;
          Transform  t;

          Coroutine att, move;
          bool isMoving= false, hasBeenDamaged = false;

        int i = 0;
        float hp;

        bool isDead = false; 
        Vector2 vec;
    HealthScript health;
    void Awake()
    {
        health = GetComponent<HealthScript>();
        hp = health.gethealth();
        rb2d = GetComponent<Rigidbody2D>();

        handLCollider= transform.GetChild(2).gameObject.GetComponent<CircleCollider2D>();
        handRCollider= transform.GetChild(3).gameObject.GetComponent<CircleCollider2D>();

        Barell1 = transform.GetChild(6).gameObject;
        Barell2= transform.GetChild(7).gameObject;
        legL = transform.GetChild(4).gameObject;
        legR = transform.GetChild(5).gameObject;

        Target = GameObject.FindGameObjectWithTag(targetTag);
        //playerTransform = Target.GetComponent<Transform>();
        laser = GetComponent<LaserSpawner>();

        vec = new Vector2(0,0);
        move = null;
    }

    void Start()
    {
        att = StartCoroutine(AttackRoutine());
        //move();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead) return;
        if(health.gethealth() <= 0){
            StopAllCoroutines();
            StartCoroutine(Finale());
            return;
        }
         playerTransform = Target.GetComponent<Transform>();
        // Debug.Log("Player Is at: " + playerTransform.position);
         transform.position = new Vector2(playerTransform.position.x + distance, transform.position.y);

         if(move != null && Target.GetComponent<Rigidbody2D>().linearVelocityX == 0)
        {
            StopCoroutine(move);
        }

        if(!isMoving && !CollisionAttack && Target.GetComponent<Rigidbody2D>().linearVelocityX != 0)
        {
            move = StartCoroutine(MoveRoutine());
            move = null;
        }
         //Debug.Log("DAS-Man Is at: " + transform.position);
       //laser.Spawn(Mathf.Abs((playerTransform.position.x - transform.position.x)* (playerTransform.position.x - transform.position.x)), Mathf.Sqrt((playerTransform.position.x - transform.position.x)* (playerTransform.position.x - transform.position.x) + (playerTransform.position.y - transform.position.y)*(playerTransform.position.y - transform.position.y)));
        if(health.gethealth() % 1000 ==  0 || health.gethealth() <= 0)
        {
            if(health.gethealth() > 999000)
                return;
            if(!hasBeenDamaged){
                AudioSource.PlayClipAtPoint(scream, Camera.main.transform.position, screamVolume);
                StartCoroutine(HurtRoutine());
            }
            if(health.gethealth() <= 0){
                StopAllCoroutines();
                StartCoroutine(Finale());
           }
        }
    }

//For Damaging the enemy use this function, has a list of bullets suposed to damage the enemy
    void OnTriggerEnter2D(Collider2D other)
    {
          foreach(BulletInfo x in bull)
            {
                if(other.tag == x.bullet.tag)
                {
                    health.DealDamage(other.GetComponent<Bullet>().GetDamage());
                    Destroy(other.gameObject);
                    break;
                }
            }
    }

    void SmashAttack()
    {
        GameObject handl, handr;
        handl = transform.GetChild(2).gameObject;
        handr = transform.GetChild(3).gameObject;
        CollisionAttack = true;
        StartCoroutine(SmashRoutine(handl, handr));
    }

    IEnumerator SmashRoutine(GameObject handl, GameObject handr)
    {

        handl.transform.rotation = Quaternion.Euler(0,0,-80);
        handr.transform.rotation = Quaternion.Euler(0,0,80);
        handl.transform.localScale = new Vector2(2f, 2f);
        handr.transform.localScale = new Vector2(2f, 2f);
        yield return new WaitForSecondsRealtime(1f);

        handl.transform.rotation = Quaternion.Euler(0,0,-5);
        handr.transform.rotation = Quaternion.Euler(0,0,5);
        yield return new WaitForSecondsRealtime(0.3f);
        //yield return new WaitForSecondsRealtime(Mathf.Abs(rb2d.linearVelocityY/(rb2d.gravityScale* Physics2D.gravity.y)));
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 90f);
        isNotInAir = true;
         yield return new WaitForSecondsRealtime(4.5f);

        handl.transform.rotation = Quaternion.Euler(0,0,0);
        handr.transform.rotation = Quaternion.Euler(0,0,0);
         handl.transform.localScale = new Vector2(1f, 1f);
        handr.transform.localScale = new Vector2(1f, 1f);
        CollisionAttack = false;
    }

     void OnCollisionEnter2D(Collision2D collision)
     {
        if(!CollisionAttack)return;
        GameObject temp = null;
          if(collision.gameObject.tag == "Ground")
                temp = collision.gameObject;
            else
                return;
            t = temp.transform;
          if(handLCollider.IsTouching(temp.GetComponent<BoxCollider2D>()) || handRCollider.IsTouching(temp.GetComponent<BoxCollider2D>())){
                //if(i  >= 2)
                if(isNotInAir)
                {
                    Debug.Log("Yup done right " + i);
                    temp = Instantiate(ShockObject);
                    temp.transform.position =new Vector3(gameObject.transform.position.x, t.position.y + 27.4f, 0f);
                    isNotInAir = false;
                    i = -1;
                    return;
                    //Debug.Log(i);
                }
                else
                     ++i;
                Debug.Log("Collided: " + i + "times");
          }
     }

     IEnumerator SpawnMissile()
    {
        Barell1.SetActive(true);
        Barell2.SetActive(true);
        Instantiate(Missile, Barell1.transform.position, Quaternion.Euler(0,0, Barell1.transform.rotation.z));
        Instantiate(Missile, Barell2.transform.position, Quaternion.Euler(0,0, Barell2.transform.rotation.z));
        yield return new WaitForSecondsRealtime(barellActiveTime);
        Barell1.SetActive(false);
        Barell2.SetActive(false);
    }

    IEnumerator MoveRoutine(){
        isMoving = true;
          for(int i = 0; i != 40 ; i+= 1){
            legL.transform.rotation = Quaternion.Euler(0,0,i);
            legR.transform.rotation = Quaternion.Euler(0,0,i);
            yield return new WaitForSecondsRealtime(0.003f);
      }
        //yield return new WaitForSecondsRealtime(1f);
        for(int i = 40; i != 0 ; i+= -1){
            legL.transform.rotation = Quaternion.Euler(0,0,i);
            legR.transform.rotation = Quaternion.Euler(0,0,i);
            yield return new WaitForSecondsRealtime(0.003f);
      }
        //yield return new WaitForSecondsRealtime(0.8f);
        for(int i = 0; i != -40 ; i+= -1){
            legL.transform.rotation = Quaternion.Euler(0,0,i);
            legR.transform.rotation = Quaternion.Euler(0,0,i);
            yield return new WaitForSecondsRealtime(0.003f);
      }
        //yield return new WaitForSecondsRealtime(0.8f);
        for(int i = -40; i != 0 ; i+= 1){
            legL.transform.rotation = Quaternion.Euler(0,0,i);
            legR.transform.rotation = Quaternion.Euler(0,0,i);
            yield return new WaitForSecondsRealtime(0.003f);
      }
        isMoving = false;
        //yield return new WaitForSecondsRealtime(0.8f);
    }

    IEnumerator HurtRoutine()
    {
        StopCoroutine(att);
        hasBeenDamaged = true;
        GameObject handL = transform.GetChild(2).gameObject;
        GameObject handR = transform.GetChild(3).gameObject;
        for(int i = 0; i<= 40; i++)
        {
            handL.transform.rotation = Quaternion.Euler(0,0, -i);
            handR.transform.rotation = Quaternion.Euler(0,0, i);
            yield return new WaitForSecondsRealtime(0.002f);
        }

        for(int i = 0; i< 10; i++){
            vec.y = -1 * transform.localScale.x;
            vec.x = transform.localScale.y;
            transform.localScale = vec;
            yield return new WaitForEndOfFrame();
            vec.y = -1 * transform.localScale.y;
            transform.localScale = vec;
        }
        att = StartCoroutine(AttackRoutine());
        hasBeenDamaged = false;
    }


    IEnumerator Finale()
    {
        isDead = true;
        FindFirstObjectByType<ScoreKeeper>().UpdateScore(1000000);
        vec.x = transform.position.x;
        vec.y = 100f;
         transform.position = vec;
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene("tuduk");
        //health.Die();
    }
     //This is the main attack routing, add any new attacks here, all the atacks are methods so just call the method over here
    IEnumerator AttackRoutine()
    {
        Debug.Log("AHA");
        while(true){
           yield return new WaitForSecondsRealtime(4f);
            laser.Spawn("Player");
            yield return new WaitForSecondsRealtime(5f);
            SmashAttack();
            yield return new WaitForSecondsRealtime(10f);
            StartCoroutine(SpawnMissile());
            yield return new WaitForSecondsRealtime(20f);
        }
    }
}
