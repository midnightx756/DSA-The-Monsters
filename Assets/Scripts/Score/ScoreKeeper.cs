using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{

    public static int score= 0;

    [SerializeField] int l1Score;
    [SerializeField] int l2Score;
    public static bool phaseDas = false, phaseHOD = false;
     void Awake()
     {
          int l = GameObject.FindObjectsByType<ScoreKeeper>(0).Length;
          if(l > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
     }
     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
        
    }

    public void UpdateScore(int Score)
    {
        score += Score;
    }

    public int GetScore()
    {
        return score;
    } 

    public void reset()
    {
        score = 0;
        phaseDas = false;
        phaseHOD = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(!phaseDas && score >= l1Score && score < l2Score)
        {
            phaseDas = true;
            Debug.Log("Loading Darshan Battle");
            SceneManager.LoadScene("DarshanBattle");
        }
        if(!phaseHOD && score >= l2Score)
        {
            phaseHOD = true;
            Debug.Log("Loading HOD Battle");
            SceneManager.LoadScene("HODSir");
        }
    }
}
