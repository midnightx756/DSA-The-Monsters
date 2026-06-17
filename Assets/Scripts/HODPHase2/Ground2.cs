using UnityEngine;

public class Ground2 : MonoBehaviour
{
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector2 tracker, tracker2, thirrd, frth;

    [SerializeField] float speed = 0.1f;
    GameObject Conveyor;
    SpriteRenderer sprite;
    Material mat;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerMovement>().backeffect = true;
        tracker = new Vector2(player.transform.position.x, player.transform.position.y);
        tracker2 = new Vector2(player.transform.position.x, player.transform.position.y);
        Conveyor = transform.GetChild(0).gameObject;
        thirrd = new Vector2(0,0);
        sprite = GetComponent<SpriteRenderer>();
        mat = sprite.material;
    }

void OnCollisionStay2D(Collision2D other) {
        if (Conveyor.GetComponent<CapsuleCollider2D>().IsTouching(player.GetComponent<CapsuleCollider2D>()))
        {
            player.GetComponent<PlayerMovement>().on_effector = true;
            thirrd.x = -5;
            thirrd.y = player.GetComponent<Rigidbody2D>().linearVelocityY;
            player.GetComponent<Rigidbody2D>().linearVelocity = thirrd;
        }
        else
        {
             player.GetComponent<PlayerMovement>().on_effector = false;
        }
}

     // Update is called once per frame
     void Update()
    {
        if(player.transform.position.x - tracker.x <= 0)
        {
            //Debug.Log("Player was behind his initial positon" + tracker.x + " " + player.transform.position.x );
            tracker.x = player.transform.position.x;
            if(tracker.x > tracker2.x)
                tracker2.x = tracker.x;
            return;
        }
        if(player.transform.position.x < tracker2.x)
        {
           // Debug.Log("Player returning");
            return;
        }
        tracker.x = player.transform.position.x;
        tracker.y = gameObject.transform.position.y;
        gameObject.transform.position = tracker;
        //gameObject.GetComponent<SpriteRenderer>().material.mainTextureOffset = new Vector2((tracker.x * speed) % gameObject.transform.localScale.x, 0);
        frth.x = (player.transform.position.x * speed) % gameObject.transform.localScale.x;
        frth.y = 0f;
        mat.SetTextureOffset("_NormalMap", frth);
    }
}