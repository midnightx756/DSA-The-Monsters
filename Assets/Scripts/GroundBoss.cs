using UnityEngine;

public class GroundBoss : MonoBehaviour
{
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector2 tracker;

    [SerializeField] float speed = 0.1f;

    [SerializeField] bool UpdateHeight = false;
    [SerializeField] float nHeight = 0f;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        tracker = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null) return;
        tracker.x = player.transform.position.x;
        tracker.y = (!UpdateHeight)? transform.position.y: nHeight;
        gameObject.transform.position = tracker;
        gameObject.GetComponent<SpriteRenderer>().material.mainTextureOffset = new Vector2((tracker.x * speed) % gameObject.transform.localScale.x, 0);
    }
}
