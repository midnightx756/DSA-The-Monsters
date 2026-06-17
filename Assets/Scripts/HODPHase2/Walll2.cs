using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Walll2 : MonoBehaviour
{
    [Header("LayeSpeeds")]
    [SerializeField] float layer1Speed;
    [SerializeField] float layer2Speed;
    [SerializeField] float layer3Speed;
    
    [Header("Light Controlling")]
    [SerializeField] Light2D envLight;
    [SerializeField] float maxIntensity;
    [SerializeField] float minIntensity;
    [SerializeField] float IntensityFactor;
    Vector2 tracker;
    float t;
    GameObject player, layer1, layer2, layer3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        layer1 = transform.GetChild(0).gameObject;
        layer2 = transform.GetChild(1).gameObject;
        layer3 = transform.GetChild(2).gameObject;
        tracker = new Vector2(player.transform.position.x, transform.position.y);
        //tracker2 = new Vector2(player.transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        tracker.x = player.transform.position.x;
        tracker.y = transform.position.y;
        if(envLight != null)
        {
            envLight.intensity = Time.time * IntensityFactor%maxIntensity + minIntensity;
            envLight.transform.position = tracker;
        }
        transform.position = tracker;
        t = player.transform.position.x;
        tracker.x = t* layer1Speed;
        tracker.y = 0;
       setOffset(layer1, tracker);
       tracker.x = t * layer2Speed;
       setOffset(layer2, tracker);
       tracker.x = t * layer3Speed;
       setOffset(layer3, tracker);
    }

    void setOffset(GameObject layer, Vector2 offset)
    {
        layer.GetComponent<SpriteRenderer>().material.mainTextureOffset = offset;
    }
}
