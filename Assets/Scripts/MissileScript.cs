using System;
using Unity.VisualScripting;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    [SerializeField]GameObject explosionfx;
    [SerializeField] AudioClip explosionsfx;
    [SerializeField] [Range(0f, 1f)] float volume = 0.5f;
    [SerializeField] float MaxHeight;
    [SerializeField] string TargetTag;

    bool attackMode = false;
    GameObject Target;
    Rigidbody2D myRigidbody;
    Transform targetTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag(TargetTag);
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.linearVelocityY = speed;
        targetTransform = Target.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject == null)
            return;
        if(gameObject.transform.position.y >= MaxHeight)
        {
             myRigidbody.linearVelocityY = 0f;
             float res = Mathf.Atan2(Mathf.Abs(gameObject.transform.position.y - Target.transform.position.y), Mathf.Abs(gameObject.transform.position.x - Target.transform.position.x)) * Mathf.Rad2Deg;
           // res = Math.Abs(res);
            gameObject.transform.rotation = Quaternion.Euler(0,0,res + ( (transform.position.x > Target.transform.position.x)? 0 : 90f)) ;
            myRigidbody.linearVelocityY = -speed * Mathf.Sin(res* Mathf.Deg2Rad); 
            myRigidbody.linearVelocityX = speed * Mathf.Cos(res* Mathf.Deg2Rad)*( (transform.position.x < Target.transform.position.x)? 1 : -1);
            attackMode = true;
        }
        if (attackMode)
        {
            myRigidbody.linearVelocityX += acceleration * ((transform.position.x < Target.transform.position.x) ? 1 : -1) * Time.deltaTime;
        }
    }

     void OnTriggerEnter2D(Collider2D collision)
     {
          if(collision.tag == "Player")
        {
            collision.GetComponent<HealthScript>().DealDamage(damage);
        }
        if(collision.tag.Equals("Ground") || collision.tag.Equals("Player"))
        {
            Instantiate(explosionfx, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(explosionsfx, Camera.main.transform.position, volume);
            Destroy(gameObject);
        }
     }
}
