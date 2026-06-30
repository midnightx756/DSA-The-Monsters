using UnityEngine;

public class menubg : MonoBehaviour
{

    [SerializeField] float height;
    
    [Header("Layers")]
    [SerializeField] float layer1Speed;
    [SerializeField] float layer2Speed;

    [SerializeField] float layer3Speed;

    Vector2 util, util2;
    GameObject layer1, layer2, layer3;
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        layer1 = transform.GetChild(0).gameObject;
        layer2 = transform.GetChild(1).gameObject;
        layer3 = transform.GetChild(2).gameObject;
        player = GameObject.FindWithTag("Player");
        util = new Vector2(0,player.transform.position.y + height);
        util2 = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        util.x = player.transform.position.x;
        util.y = height;
        transform.position = util;
        util2.x = player.transform.position.x * layer1Speed;
        util2.y = 0;
        layer1.GetComponent<SpriteRenderer>().material.mainTextureOffset = util2;
        util2.x = player.transform.position.x * layer2Speed;
        util2.y = 0;
        layer2.GetComponent<SpriteRenderer>().material.mainTextureOffset = util2;
        util2.x = player.transform.position.x * layer3Speed;
        util2.y = 0;
        layer3.GetComponent<SpriteRenderer>().material.mainTextureOffset = util2;
    }
}
