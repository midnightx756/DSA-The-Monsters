using System.Collections;
using UnityEngine;

public class Missiler : MonoBehaviour
{
    [SerializeField] GameObject missile;
    [SerializeField] float animationPlaybackspeed;
    [SerializeField] float moveDis;

    [SerializeField] float moveSpeed;

    [SerializeField] Sprite standSprite;
    HealthScript health;

    ScoreKeeper scoreKeeper;
    GameObject dingdong, player;
    bool isDead = false, isAttacking = false;
    SpriteRenderer sp;
    Sprite spr;
    Coroutine cc;
    Animator anim;
    Vector2 util, util2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<HealthScript>();
        sp = GetComponent<SpriteRenderer>();
        spr = sp.sprite;
        sp.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        dingdong = transform.GetChild(0).gameObject;
        anim = GetComponent<Animator>();
        util = new Vector2(0, transform.position.y);
        util2 = new Vector2(transform.localScale.x, transform.localScale.y);
        player = GameObject.FindWithTag("Player");
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        cc = StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead) return;
        if (isAttacking)
        {
            //anim.speed =0;
            return;
        }
        if(health.gethealth() <= 0)
        {
            StartCoroutine(Death());
        }
        if(Mathf.Abs(transform.position.x - player.transform.position.x) < moveDis)
        {
            anim.speed = animationPlaybackspeed;
            util2.x *= (transform.position.x > player.transform.position.x)? 1 : -1;
            util.x = transform.position.x + moveSpeed* ((transform.position.x > player.transform.position.x)? 1 : -1);
            util.y = transform.position.y;
            transform.localScale = util2;
            transform.position = util;
        }
        else if(player.GetComponent<Rigidbody2D>().linearVelocityX == 0)
        {
            anim.speed = 0;
            sp.sprite = standSprite;
        }
    }

     IEnumerator Death()
        {
            isDead = true;
            StopCoroutine(cc);
            scoreKeeper.UpdateScore(100);
            yield return new WaitForSecondsRealtime(0.5f);
            health.Die();
        }

        IEnumerator Attack()
        {
            yield return new WaitForSecondsRealtime(5f);
            while(true){
                if(isDead || this == null) yield return null;
                isAttacking = true;
                //anim.controller.stop();
                anim.speed = 0;
                sp.sprite = standSprite;
                for(int i = 1; i<= 4; i++)
                {
                    if(missile == null){
                            Debug.Log("Prefab the misile, anyways go ahead");
                            continue;
                    }
                    GameObject inst = Instantiate(missile);
                    if(inst == null || inst.transform == null)
                    {
                              Debug.Log("Prefab the misile, anyways go ahead");
                            continue;
                    }
                    if(dingdong == null)
                    {
                         Debug.Log("Child is invalid");
                            continue;
                    }
                    inst.transform.position = dingdong.transform.position;
                    yield return new WaitForSecondsRealtime(0.5f);
                }
                isAttacking = false;
                yield return new WaitForSecondsRealtime(8f);
            }
        }
}
