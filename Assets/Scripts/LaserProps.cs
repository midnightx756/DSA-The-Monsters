using System.Collections;
using System.Timers;
using UnityEngine;

public class LaserProps : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float Duration;

    [SerializeField] float damageInterval = 0.1f;
    [SerializeField] Vector2 moveSpeed;
    //bool isFiring = false;
    float timer = 0;
    GameObject Player;
    HealthScript hp;
    Material m;
    Vector2 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        if(Player != null)
          hp = Player.GetComponent<HealthScript>();
        m = GetComponent<SpriteRenderer>().material;
        offset = new Vector2();
    }

    void Start()
    {
        Destroy(gameObject, Duration);
    }
     void Update()
     {
            offset = moveSpeed * Time.time;
            m.mainTextureOffset = offset;
     }
     public void OnTriggerStay2D(Collider2D other)
     {
          if(other.tag != "Player")return;
          if(hp == null)return;

          if(Time.time>=timer)
        {
            hp.DealDamage(damage);
            timer += Time.time + damageInterval;
        }
     }
}