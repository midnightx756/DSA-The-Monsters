using UnityEngine;

public class SidBG : MonoBehaviour
{

    [SerializeField] float BGSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    GameObject player;
    Vector2 util;
    void Start()
    {
        player= GameObject.FindGameObjectWithTag("Player");
        util = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null) return;
        util.x = player.transform.position.x;
        util.y = transform.position.y;
        transform.position = util;
        util.x *= BGSpeed;
        util.y = 0;
        GetComponent<SpriteRenderer>().material.mainTextureOffset = util;
    }
}
