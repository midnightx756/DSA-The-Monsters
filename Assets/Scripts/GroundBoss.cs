using UnityEngine;

public class GroundBoss : MonoBehaviour
{
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector2 tracker;

    [SerializeField] float speed = 0.1f;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        tracker = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        tracker.x = player.transform.position.x;
        tracker.y = gameObject.transform.position.y;
        gameObject.transform.position = tracker;
        gameObject.GetComponent<SpriteRenderer>().material.mainTextureOffset = new Vector2((tracker.x * speed) % gameObject.transform.localScale.x, 0);
    }
}
