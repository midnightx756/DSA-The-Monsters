using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] float bulletLifetime = 9f;
    [SerializeField] float speed = 5f;
    [SerializeField] float Damage = 10f;

    HealthScript health;
    Rigidbody2D myrigidbody2d;
    PlayerMovement player;
    float xspeed;

    float elapsed = 0f;
    public void SetDamage(float t)
    {
        Damage = t;
    }
    public float GetDamage()
    {
        return Damage;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myrigidbody2d = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>();
        xspeed = player.transform.localScale.x * speed; 
        health = FindFirstObjectByType<HealthScript>();
    }
    // Update is called once per frame
    void Update()
    {
       myrigidbody2d.linearVelocity= new Vector2(xspeed, 0f);
       transform.localScale = new Vector2(Mathf.Sign(myrigidbody2d.linearVelocity.x)*5f, 3f);
       elapsed += Time.deltaTime;
       if(elapsed >= bulletLifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            //Debug.Log(other.name + " "+  other.tag);
            //health.DealDamage(Damage);
            other.GetComponent<HealthScript>().DealDamage(Damage);
            //Debug.Log(health.gethealth());
        }
        if(other.tag == "Boss")
        {
            health = other.GetComponent<HealthScript>();
            if(health != null)
                health.DealDamage(Damage);
        }
        if(other.tag == "FactoryWall")
        {
            HealthScript s = other.GetComponent<HealthScript>();
            if(s != null)
            {
                s.DealDamage(Damage);
            }
            else 
            {
                GameObject parentO = other.transform.parent.gameObject; 
                if(parentO != null)
                {
                    s = parentO.GetComponent<HealthScript>();
                    if(s != null)
                    {
                        s.DealDamage(Damage);
                    }
                }
            }
        }
        Destroy(gameObject);
    }
}
