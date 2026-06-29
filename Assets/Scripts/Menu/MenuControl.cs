using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restartGame()
    {
        ScoreKeeper sk = FindAnyObjectByType<ScoreKeeper>();
        sk.reset();
        SceneManager.LoadScene("tuduk");
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
