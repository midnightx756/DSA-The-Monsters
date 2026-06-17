using UnityEngine;

public class Saws : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damagePerFrame = 0.1f;
    GameObject Saw1, Saw2, ground;

    //[SerializeField]AudioClip sawClip;
    AudioSource audioSource;
    //[SerializeField][Range(0f, 1f)] float sawVolume = 0.5f;
    CircleCollider2D s1, s2;
    CapsuleCollider2D playerCollider;
    float t;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Saw1 = transform.GetChild(0).gameObject;
        Saw2 = transform.GetChild(1).gameObject;
        ground = GameObject.FindWithTag("Ground");
        Debug.Log(ground.name);
        s1 = Saw1.GetComponent<CircleCollider2D>();
        s2 = Saw2.GetComponent<CircleCollider2D>();
        playerCollider = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

     void OnCollisionEnter2D(Collision2D collision)
     {
                 audioSource.Play();
     }

     void OnCollisionStay2D(Collision2D collision)
     {
            if(collision.gameObject.tag == "Player")
        {
            //AudioSource.PlayClipAtPoint(sawClip, Camera.main.transform.position, sawVolume);
            if(s1.IsTouching(playerCollider) && s2.IsTouching(playerCollider))
            {
                collision.gameObject.GetComponent<HealthScript>().DealDamage(damagePerFrame * 2);
            }
            else if(s1.IsTouching(playerCollider) || s2.IsTouching(playerCollider))
            {
                collision.gameObject.GetComponent<HealthScript>().DealDamage(damagePerFrame);
            }
        }
        else if(collision.gameObject.tag == "Trash")
        {
            Destroy(collision.gameObject);
        }     
     }

     void OnCollisionExit2D(Collision2D collision)
     {
          audioSource.Stop();
     }
     // Update is called once per frame
     void Update()
    {
        transform.position = ground.transform.position;
        t = Time.time%180 * speed %180;
        Saw1.transform.rotation = Quaternion.Euler(0,0, t);
        Saw2.transform.rotation = Quaternion.Euler(0,0, t);
    }
}
