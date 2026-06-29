using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
     GameObject Player;
     ScoreKeeper scoreKeeper;
    HealthScript health;

    public string playername;
    //float score;
    TextMeshProUGUI healthtrash;
    Slider healthslider;
    Image img;
    float hp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        Player = GameObject.FindGameObjectWithTag("Player");
        health = Player.GetComponent<HealthScript>();    
        healthtrash = GetComponentInChildren<TextMeshProUGUI>();
        healthslider  = GetComponentInChildren<Slider>();
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        hp = health.gethealth();
        healthtrash.text = "Name: " + playername+ "\nScore: " + scoreKeeper.GetScore()+ "\nHealth: " + health.gethealth() +"/\n" + hp;
        img = transform.GetChild(3).GetComponent<Image>();
        healthslider.value = hp;
        healthslider.maxValue = hp;
    }
    // Update is called once per frame
    void Update()
    {
         healthtrash.text =  "Name: " + playername+ "\nScore: " + scoreKeeper.GetScore()+ "\nHealth: " + health.gethealth();
         healthslider.value = health.gethealth();
    }
    public IEnumerator moveDir(float Duration)
    {
        Debug.Log("Clock Started " + Duration + " "+1/Duration);
        if(img== null)
        {
            Debug.Log("4rth image is invalid");
            yield return null;
        }
        float itr = 1 / Duration;
        for(float i = itr; i<= 1 + itr; i+= itr)
        {
            //Debug.Log(i);
            img.fillAmount = i;
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
