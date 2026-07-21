using UnityEngine;

public class HandGrab : MonoBehaviour
{
    private ChainArm owner;
    
    public void Init(ChainArm arm)
    {
        owner = arm;
    }

     void OnTriggerEnter2D(Collider2D collision)
     {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;

            owner.OnGrab(collision.transform);
        }
        if(collision.CompareTag("Wall")){
            owner.Release(); 
        }
     }
}
