using System.Collections;
using UnityEngine;

public class Trash : MonoBehaviour
{

    [SerializeField] float damage;

    Rigidbody2D rb2D;
    BoxCollider2D box;
    //Flag to tell tht trash has already damaged the player
    bool firedamage = false;
    Vector2 po;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        po = new Vector2(0,0);
        box = GetComponent<BoxCollider2D>();
    }


     void OnCollisionEnter2D(Collision2D collision)
     {
      
        if(firedamage) return;
          if(collision.gameObject.tag == "Player")
        {
            float v = rb2D.linearVelocityY;
            collision.gameObject.GetComponent<HealthScript>().DealDamage(((v < 0)?  v : 0) * rb2D.mass * -1);
            firedamage = true;
        }
     }

     IEnumerator Pee()
    {
        Debug.Log("Touched");
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(gameObject);
    }
     // Update is called once per frame
     void Update()
    {
         if (box.IsTouchingLayers(6))
        {
            Debug.Log("Trash is on conveyor");
            rb2D.linearVelocityX = -10;
        }
    }
}
