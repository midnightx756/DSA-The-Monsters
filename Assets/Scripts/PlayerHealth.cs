using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
     GameObject Player;
    HealthScript health;

    public string playername;

    float score;
    TextMeshProUGUI healthtrash;
    Slider healthslider;

    float hp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        Player = GameObject.FindGameObjectWithTag("Player");
        health = Player.GetComponent<HealthScript>();    
        healthtrash = GetComponentInChildren<TextMeshProUGUI>();
        healthslider  = GetComponentInChildren<Slider>();

        hp = health.gethealth();
        healthtrash.text = "Name: " + playername+ "\nScore: " + score+ "\nHealth: " + health.gethealth() +"/\n" + hp;
        healthslider.value = hp;
        healthslider.maxValue = hp;
    }
    // Update is called once per frame
    void Update()
    {
         healthtrash.text =  "Name: " + playername+ "\nScore: " + score+ "\nHealth: " + health.gethealth();
         healthslider.value = health.gethealth();
    }
}
