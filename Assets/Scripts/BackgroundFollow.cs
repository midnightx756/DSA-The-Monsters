using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    GameObject Player;
    [SerializeField] float y_upper;
    [SerializeField] float padding;
    //[SerializeField] float speed = 1f;

    Vector2 position; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        position = Player.transform.position;
        if(Mathf.Abs(position.x - transform.position.x) > padding)
            gameObject.transform.position = new Vector2(position.x,y_upper);
    }
}
