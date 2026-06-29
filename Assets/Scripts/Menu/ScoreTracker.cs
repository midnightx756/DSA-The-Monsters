using TMPro;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    ScoreKeeper scoreKeeper;
    [SerializeField] TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        Debug.Log(scoreText.GetType());
        if(scoreText != null)
            scoreText.text = "" + scoreKeeper.GetScore();
    }

    // Update is called once per frame
    /*void Update()
    {
        //LOLLLLLLL
    }*/
}
