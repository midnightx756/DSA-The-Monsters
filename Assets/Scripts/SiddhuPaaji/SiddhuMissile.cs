using System.Collections;
using UnityEngine;

public class SiddhuMissile : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float accel;
    [SerializeField] float damage;
    [SerializeField] float initHeight;

    [SerializeField] GameObject AfterFX;

    ParticleSystem ps;
    bool isInSummon = false;

    Rigidbody2D rb2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        ps = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        StartCoroutine(UpMove());
    }

    // Update is called once per frame
    void Update()
    {
        if(isInSummon) return;
        //GameObject player = GameObject.FindWithTag("Player");
        //if(player != null)
        //{
            //transform.rotation = Quaternion.Euler(0,0, Mathf.Atan2(transform.position.y - player.transform.position.y, Mathf.Abs(transform.position.x - player.transform.position.x))* Mathf.Rad2Deg);
        //}
    }

     void OnCollisionEnter2D(Collision2D collision)
     {
           Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player" )
        {
            if(collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<HealthScript>().DealDamage(damage);
            }
            GameObject t = Instantiate(AfterFX);
            t.transform.position = transform.position;
            Destroy(gameObject);
        }
     }
     /*void OnCollisionEnter2D(Collider2D collision)
     {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player" )
        {
            if(collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<HealthScript>().DealDamage(damage);
            }
            GameObject t = Instantiate(AfterFX);
            t.transform.position = transform.position;
            Destroy(gameObject);
        }
     }*/
     
     IEnumerator UpMove()
    {
        isInSummon = true;
        Vector2 ob = transform.localScale;
        transform.rotation = Quaternion.Euler(0,0, Mathf.Sign(ob.x) * -90);

        //Debug.Log("Particle System: " + ps.name + " Main: " + ps.main);
        var main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.startRotation3D = true;
        //main.startRotationZ = -transform.rotation.z;
        
        GameObject player = GameObject.FindWithTag("Player");
        if(player == null)
        {
            yield return null;
        }
        rb2D.linearVelocityY = 4;
        while(transform.position.y < initHeight)
        {
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.identity;
        rb2D.linearVelocityY = 0;
        rb2D.linearVelocityX = 0;
        if(player != null)
        {
            transform.rotation = Quaternion.Euler(0,0, Mathf.Atan2(transform.position.y - player.transform.position.y, Mathf.Abs(transform.position.x - player.transform.position.x))* Mathf.Rad2Deg);
             //Debug.Log(transform.position + " "+ player.transform.position + " "+ transform.rotation.z);
        }
         //main.startRotationZ = -transform.rotation.z;
        rb2D.linearVelocityY= Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad) * speed;
        rb2D.linearVelocityX = Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad)* speed;
        isInSummon = false;
        //rb2D.linearVelocityX = speed;
    }
}
