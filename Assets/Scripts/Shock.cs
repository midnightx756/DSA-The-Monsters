using System;
using Unity.VisualScripting;
using UnityEngine;

public class Shock : MonoBehaviour
{
    [SerializeField] float destructiontime;
    [SerializeField] float damage;

    [SerializeField] AudioClip shocksound;
    [SerializeField] [Range(0f, 1f)] float shockVolume = 0.5f;
    void Start()
    {
            AudioSource.PlayClipAtPoint(shocksound, Camera.main.transform.position, shockVolume);
            Destroy(gameObject, destructiontime);   
    }

    void Update()
    {
        
    }

     void OnTriggerEnter2D(Collider2D collision)
     {
          if(collision.tag == "Player")
        {
            collision.GetComponent<HealthScript>().DealDamage(damage);
            collision.GetComponent<Rigidbody2D>().linearVelocityY = 100;
        }
     }
}
