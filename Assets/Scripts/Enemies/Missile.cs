using System;
using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float startSpeed;
    [SerializeField] float normalSpeed;

    [SerializeField] float Damage;


    [SerializeField] ParticleSystem explosionfx;
    [SerializeField] AudioClip explosionsfx;
    [SerializeField] [Range(0f, 1f)] float Volume;
    Rigidbody2D rb2D;
    GameObject player;
    Transform pT;
    bool wait = false;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        pT = player.transform;
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        wait = true;
        rb2D.linearVelocityX = startSpeed;
        yield return new WaitForSecondsRealtime(3);
        rb2D.linearVelocityX = 0f;

        while(pT.position.y <= transform.position.y)
        {
            transform.rotation = Quaternion.Euler(0,0,30);
            rb2D.linearVelocityX = normalSpeed* Mathf.Cos(MathF.PI/6);
            rb2D.linearVelocityY = normalSpeed* Mathf.Sin(MathF.PI/6);
            yield return new WaitForEndOfFrame();
        }
        rb2D.linearVelocityX = 0;
        rb2D.linearVelocityY = 0;
        transform.rotation = Quaternion.Euler(0,0,0);
        yield return new WaitForEndOfFrame();
        wait = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(wait)return;
        rb2D.linearVelocityX = normalSpeed - Time.time;
    }

     void OnCollisionEnter2D(Collision2D collision)
     {
          if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthScript>(). DealDamage(Damage);
        }
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ground"){
            Instantiate(explosionfx, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(explosionsfx, Camera.main.transform.position, Volume);
            Destroy(gameObject);
        }
     }
}
