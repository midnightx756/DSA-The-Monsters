using System.Collections;
using UnityEngine;

public class Rammer : MonoBehaviour
{
    [SerializeField] float accel;
    [SerializeField] Vector2 force;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D  rb2D;
    GameObject player;
    Vector2 util, util2;

    BoxCollider2D boxy;
    HealthScript health;
    ScoreKeeper scoreKeeper;
    bool isDead = false;

    Animator anim;
    PlayerMovement pm;

    void Start()
    {
        StartCoroutine(Inactive());
        rb2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        util = new Vector2(transform.localScale.x, transform.localScale.y);
        boxy = GetComponent<BoxCollider2D>();
        health = GetComponent<HealthScript>();
        anim = GetComponent<Animator>();
        util2 = new Vector2(0,0);
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        rend.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead) return;
        if(health.gethealth() <= 0)
        {
            isDead = true;
            StartCoroutine(Death());
            return;
        }
        if(player != null)
        {
            if(player.transform.position.x > transform.position.x)
            {
                  rb2D.linearVelocityX = accel * Time.time;
                  if(transform.localScale.x > 0 )
                    util.x *= -1;
                  transform.localScale = util;
            }
            else{
                if(transform.localScale.x < 0 )
                    util.x *= -1;
                  transform.localScale = util;
                rb2D.linearVelocityX = -accel * Time.time;
            }
        }
    }

     void OnCollisionEnter2D(Collision2D collision)
     {
        if(isDead) return;
          if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(Throw(player));
        }
     }                                       

     IEnumerator Death()
    {
        isDead = true;
        anim.StopPlayback();
        util.y *= -1;
        transform.localScale = util;
        scoreKeeper.UpdateScore(50);
        yield return new WaitForSecondsRealtime(5f);
        health.Die();
    }

    IEnumerator Inactive()
    {
        isDead = true;
        Debug.Log("Waiting to arrive");
        yield return new WaitForSecondsRealtime(5f);
        Debug.Log("Wait Ends");
        isDead = false;
    }
    IEnumerator Throw(GameObject player)
    {
        player.GetComponent<HealthScript>().DealDamage(Mathf.Abs(rb2D.linearVelocityX) * rb2D.mass/100);
        pm= player.GetComponent<PlayerMovement>();
        pm.control = false;
        util2.x = (rb2D.linearVelocityX > 0)? force.x : -force.x;
        util2.y = force.y;
        player.GetComponent<Rigidbody2D>().AddForce(util2, ForceMode2D.Force);
        yield return new WaitForSecondsRealtime(0.4f);
        pm.control = true;
    }
}
