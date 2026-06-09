using UnityEngine;

public class FactoryWall : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float Damage;
    Vector2 transforms;
    GameObject tire1, tire2, saw;
    HealthScript hs;
    CapsuleCollider2D sawCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hs = GetComponent<HealthScript>();
        saw = transform.GetChild(0).gameObject;
        sawCollider = saw.GetComponent<CapsuleCollider2D>();
        tire1 = transform.GetChild(1).gameObject;
        tire2 = transform.GetChild(2).gameObject;
        transforms = new Vector2(transform.position.x,transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(hs.gethealth() <= 0)
        {
            hs.Die();
        }
        transforms.x += moveSpeed * Time.time;
        transform.position = transforms;
        //tire2.transform.rotation = Quaternion.Euler(0,0, transforms.x % 360);
    }

     void OnCollisionEnter2D(Collision2D collision)
     {
          if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player found");
            GameObject t = collision.gameObject;
            if(sawCollider.IsTouching(t.GetComponent<CapsuleCollider2D>()))
            {
                Debug.Log("Player Damager By saw");
                t.GetComponent<HealthScript>().DealDamage(Damage);
            }
        }
     }
}
