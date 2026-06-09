using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    GameObject Boss;
    HealthScript health;

    TextMeshProUGUI healthtrash, bName;
    Slider healthslider;

    float hp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        Boss = GameObject.FindGameObjectWithTag("Boss");
        if(Boss == null)
            return;
        health = Boss.GetComponent<HealthScript>();    
        TextMeshProUGUI[] arr =  GetComponentsInChildren<TextMeshProUGUI>(); 
        healthtrash = arr[0];
        bName = arr[1];
        healthslider  = GetComponentInChildren<Slider>();

        hp = health.gethealth();
        healthtrash.text = health.gethealth() +"/\n" + hp;
        healthslider.value = hp;
        healthslider.maxValue = hp;

        bName.text = Boss.name;
    }
    // Update is called once per frame
    void Update()
    {
        if(health == null) return;
         healthtrash.text = health.gethealth() +"/\n" + hp;
         healthslider.value = health.gethealth();
    }
}
