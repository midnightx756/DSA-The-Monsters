using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Laser : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]float Damage;
    [SerializeField] bool isScaingAllowed = false;

    bool damagePLayer = false;
    Vector2 sss;
    SpriteRenderer rr;
    GameObject Player;
     void OnTriggerEnter2D(Collider2D collision)
     {
        if(!damagePLayer)return;
          if(collision.tag == "Player")
        {
            HealthScript hs = collision.GetComponent<HealthScript>();
            if(hs == null)  return;
            float dmg =  Mathf.Abs(collision.transform.position.x - transform.position.x)/100;
            hs.DealDamage((dmg > 1) ? (dmg * Damage) : Damage);
        }
     }
     void Start()
    {
        Player = GameObject.FindWithTag("Player");
        rr = GetComponent<SpriteRenderer>();
        sss = new Vector2(0, transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(rr.color.a >= 1)
        {
            damagePLayer = true;
        }
        else 
            damagePLayer = false;
        if(!isScaingAllowed) return;
        sss.x = Mathf.Abs(transform.position.x - Player.transform.position.x);
        if(sss.x > transform.localScale.x)
            transform.localScale  = sss;
    }
}
