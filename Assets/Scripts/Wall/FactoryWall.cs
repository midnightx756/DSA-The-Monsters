using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthScript))]
public class FactoryWall : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float Damage;

    [SerializeField] float MaxSpeed = 1000f;
    Vector2 transforms;

    Rigidbody2D rb2d;
    GameObject tire1, tire2, saw;
    HealthScript hs;
    CapsuleCollider2D sawCollider;

    bool isRunning;
    Coroutine c;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hs = GetComponent<HealthScript>();
        rb2d = GetComponent<Rigidbody2D>();
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
        //transforms.x += moveSpeed * Time.time % MaxSpeed;
        //transform.position = transforms;
        rb2d.linearVelocityX = moveSpeed * Time.time % MaxSpeed;
        //tire2.transform.rotation = Quaternion.Euler(0,0, transforms.x % 360);
    }

     void OnCollisionEnter2D(Collision2D collision)
     {
          if(collision.gameObject.tag == "Player" && !isRunning)
        {
            Debug.Log("Player found");
            c = StartCoroutine(SawAttack(collision));
            isRunning = true;
        }
     }

    void OnCollisionExit2D(Collision2D collision)
     {
        Debug.Log("Player Exited");
        if(c != null)
        {
            StopCoroutine(c);
            c = null;
        }
        isRunning = false;
     }

     IEnumerator SawAttack(Collision2D collision)
    {
        while (true)
        {
            GameObject t = collision.gameObject;
            if(t != null || t.GetComponent<CapsuleCollider2D>() != null){
                if(sawCollider.IsTouching(t.GetComponent<CapsuleCollider2D>()))
                {
                    Debug.Log("Player Damager By saw");
                    t.GetComponent<HealthScript>().DealDamage(Damage);
                }
            }
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
