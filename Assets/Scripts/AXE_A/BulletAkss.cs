using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BulletAkss : MonoBehaviour
{
    Rigidbody2D rb2D;
    [SerializeField] Sprite CollisionSprite, FlcikerSprite, Normal;
    [SerializeField] float flashRate;
    [SerializeField ] float speed;

    [SerializeField] float damage;

    SpriteRenderer spr;
    //Sprite Normal;

    bool ok = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();

        Destroy(gameObject, 40f);
        //Normal = spr.sprite;
    }

    public IEnumerator StayStill()
    {
        ok = false;
        yield return new WaitForSecondsRealtime(0.4f);
        ok = true;
        StartCoroutine(Flash());
        rb2D.linearVelocityX = Mathf.Cos(Mathf.Abs(transform.eulerAngles.z * Mathf.Deg2Rad)) * -Mathf.Sign(transform.localScale.x) * speed;
        rb2D.linearVelocityY = -Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad) * speed;
    }

     void OnCollisionEnter2D(Collision2D collision)
     {
          if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
        {
            if(collision.gameObject.TryGetComponent<HealthScript>(out var hs))
            {
                hs.DealDamage(damage);
                Debug.Log("Damage Dealt");
            }
            StopAllCoroutines();
            StartCoroutine(Death());
        }
     }

     IEnumerator Death()
    {
        rb2D.linearVelocity = Vector2.zero;
        spr.sprite = CollisionSprite;
        yield return new WaitForSecondsRealtime(0.2f);
        Destroy(gameObject);
    }
     // Update is called once per frame
     void Update()
    {
        if(!ok) return;
    }

    IEnumerator Flash()
    {
        while(true){
            //Debug.Log("ssssss");
            spr.sprite = FlcikerSprite;
            yield return new WaitForSecondsRealtime(1/flashRate);
            spr.sprite = Normal;
            yield return new WaitForSecondsRealtime(1/flashRate);
        }
    }
}
