using UnityEngine;

public class BossBullet : MonoBehaviour
{

    [SerializeField] float Speed, Damage;
    Rigidbody2D rb2d;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d =   GetComponent<Rigidbody2D>();
        rb2d.linearVelocityX = Mathf.Cos(transform.rotation.z)* Speed *( (transform.localScale.x > 0)? 1 : -1);
        rb2d.linearVelocityY = Mathf.Sin(transform.rotation.z)* Speed;
    }

     void OnCollisionEnter2D(Collision2D collision)
     {
          if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthScript>().DealDamage(Damage);
        }
        Destroy(gameObject);
     }
     // Update is called once per frame
     void Update()
    {

    }
}
