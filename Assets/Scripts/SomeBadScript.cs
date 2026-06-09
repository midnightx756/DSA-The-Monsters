using UnityEngine;
using UnityEngine.Animations;

public class SomeBadScript : MonoBehaviour
{
    GameObject parentO;
    void Awake()
    {
        parentO = transform.parent.gameObject;
    }
    void OnCollisionEnter2D(Collision2D other) {
       // Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Player")
            parentO.GetComponent<BounceBreaker>().throwBack(other.gameObject);
    }
}
