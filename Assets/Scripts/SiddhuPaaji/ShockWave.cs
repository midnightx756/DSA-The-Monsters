using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [SerializeField] float accel;
    [SerializeField] float damage;
    [SerializeField] float ScaleOverTime;
    [SerializeField] float life;

    Rigidbody2D rb2D;
    Vector2 uu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        uu = transform.localScale;
        Destroy(gameObject, life);
    }

    // Update is called once per frame
    void Update()
    {
        rb2D.linearVelocityX += Mathf.Sign(transform.localScale.x) * accel;
        uu.x +=  Mathf.Sign(transform.localScale.x) * ScaleOverTime;
        uu.y += ScaleOverTime;
        transform.localScale = uu;
    }

     void OnCollisionEnter2D(Collision2D collision)
     {
          if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthScript>().DealDamage(damage);
            //collision.gameObject.GetComponent<Rigidbody2D>
            Destroy(gameObject);
        }
     }
}
