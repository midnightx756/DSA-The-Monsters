using UnityEngine;

public class Rhed : MonoBehaviour
{
        ParticleSystem s;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            s =GetComponent<ParticleSystem>();
            s.Play();
            Destroy(gameObject, 2f);   
    }
}
