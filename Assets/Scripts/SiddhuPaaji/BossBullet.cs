using System.Collections;
//using Mono.Cecil;
//using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class BossBullet : MonoBehaviour
{

    [SerializeField] float Speed, Damage;
    [SerializeField] Sprite img;
    [SerializeField] float EffectLasting = 0.5f;
    Rigidbody2D rb2d;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.linearVelocityX = Mathf.Sin(transform.eulerAngles.z* Mathf.Deg2Rad) * Speed;
        rb2d.linearVelocityY = -Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad)* Speed;
    }

     void OnTriggerEnter2D(Collider2D collision)
     {
          if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthScript>().DealDamage(Damage);
        }

        if(collision.tag == "Player" || collision.CompareTag("Ground"))
            StartCoroutine(Cor());
     }

     IEnumerator Cor()
    {
        Debug.Log("Attacked a medium");
       GameObject p = new("Pluh");
       SpriteRenderer rr = p.AddComponent<SpriteRenderer>();
       rr.sortingLayerName = "Hider Ground";
       rr.sprite = img;
       rr.transform.position = transform.position;
       rr.transform.localScale = new Vector2(10,10);
       transform.position = new Vector2(transform.position.x, -500);
        yield return new WaitForSecondsRealtime(EffectLasting);
        Destroy(p);
        Destroy(gameObject);
    }
     // Update is called once per frame
     void Update()
    {

    }
}
