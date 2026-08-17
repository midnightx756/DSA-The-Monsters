using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class spriteCont : MonoBehaviour
{
        [SerializeField] Sprite Shot;

        bool Works = false;
        Sprite StartSprite;

        SpriteRenderer rendererN;
     void Start()
     {
          rendererN = GetComponent<SpriteRenderer>();
          StartSprite = rendererN.sprite;
          Debug.Log(StartSprite.name + " "+ Shot.name);
     }
     void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Called + tag is " + other.gameObject.tag);
        if (other.gameObject.CompareTag("Bullet"))
        {
            if(Works) return;
            StartCoroutine(Shock());
        }
    }

     void OnTriggerEnter2D(Collider2D other)
     {
          Debug.Log("Called + tag is " + other.gameObject.tag);
        if (other.gameObject.CompareTag("Bullet"))
        {
            if(Works) return;
            StartCoroutine(Shock());
        }
     }
     void OnDisable()
     {
          StopAllCoroutines();
     }
     IEnumerator Shock()
    {
        Works = true;
        rendererN.sprite = Shot;
        Debug.Log("Called");
        yield return new WaitForSecondsRealtime(0.2f);
        rendererN.sprite = StartSprite;
        Works = false;
    }
}
