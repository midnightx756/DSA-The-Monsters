using UnityEngine;
using UnityEngine.SceneManagement;

public class TraceDetective : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.LogError($"[Dr Jackie Chan] Scene changed to: {scene.name} | Time : {Time.time}");
        Debug.LogError($"[Dr Jackie Chan] Stack Trace: \n{StackTraceUtility.ExtractStackTrace()}");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
