using UnityEngine;

public class Ground2 : MonoBehaviour
{
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector2 tracker, tracker2;

    [SerializeField] float speed = 0.1f;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        tracker = new Vector2(player.transform.position.x, player.transform.position.y);
        tracker2 = new Vector2(player.transform.position.x, player.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        /*if(player.transform.position.x - tracker.x <= 0)
        {
            Debug.Log("Player was behind his initial positon" + tracker.x + " " + player.transform.position.x );
            tracker.x = player.transform.position.x;
            if(tracker.x > tracker2.x)
                tracker2.x = tracker.x;
            return;
        }
        if(player.transform.position.x < tracker2.x)
        {
            Debug.Log("Player returning");
            return;
        }
        tracker.x = player.transform.position.x;
        tracker.y = gameObject.transform.position.y;
        gameObject.transform.position = tracker;
        //gameObject.GetComponent<SpriteRenderer>().material.mainTextureOffset = new Vector2((tracker.x * speed) % gameObject.transform.localScale.x, 0);*/
    }
}
