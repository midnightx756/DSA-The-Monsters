using UnityEngine;

public class AXE_BG: MonoBehaviour
{

    [SerializeField] float BGSpeed;
    [SerializeField] float ChildSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    GameObject player;
    SpriteRenderer ss;
    Vector2 util;
    void Start()
    {
        player= GameObject.FindGameObjectWithTag("Player");
        util = new Vector2(0,0);

        ss = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
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
        util.x = ss.material.mainTextureOffset.x + (ChildSpeed + player.GetComponent<Rigidbody2D>().linearVelocityX %  ChildSpeed) * Time.deltaTime;
        ss.material.mainTextureOffset = util;
    }
}
