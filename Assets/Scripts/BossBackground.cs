using UnityEngine;
using UnityEngine.Rendering.Universal;
public class BossBackground : MonoBehaviour
{
    [Header("Layer Velocities")]
        [SerializeField] float firstLayerSpeed;
        [SerializeField] float secondLayerSpeed;
        [SerializeField] float thirdLayerSpeed;

        
    GameObject layer1, layer2, layer3, player;
    
    [Header("Positioning")]
        [SerializeField] float playerXDistance;

    Vector2 ob;
    [SerializeField]Light2D spriteLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        layer1 = transform.GetChild(0).gameObject;
        layer2 = transform.GetChild(1).gameObject;
        layer3 = transform.GetChild(2).gameObject;
        player = GameObject.FindWithTag("Player");
        //spriteLight = FindAnyObjectByType<Light2D>();
        ob = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if(spriteLight != null)
        {
            ob.x = player.transform.position.x + playerXDistance;
            ob.y = spriteLight.transform.position.y;
            spriteLight.transform.position = ob;
            spriteLight.intensity = Time.time % 2;
            //Debug.Log(spriteLight.transform.position);
        }
        ob.x = player.transform.position.x + playerXDistance;
        ob.y = transform.position.y;
        transform.position = ob;
        ob.x = ob.x * firstLayerSpeed;
        ob.y = 0;
        layer1.GetComponent<SpriteRenderer>().material.mainTextureOffset = ob;
        ob.x = player.transform.position.x + playerXDistance;
        ob.x = ob.x * secondLayerSpeed;
        ob.y = 0;
        layer2.GetComponent<SpriteRenderer>().material.mainTextureOffset = ob;
        ob.x = player.transform.position.x + playerXDistance;
        ob.x = ob.x * thirdLayerSpeed;
        ob.y = 0;
        layer3.GetComponent<SpriteRenderer>().material.mainTextureOffset = ob;
    }
}
