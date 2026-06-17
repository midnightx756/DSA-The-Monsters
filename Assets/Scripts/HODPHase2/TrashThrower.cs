using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class TrashThrower : MonoBehaviour
{
    [SerializeField]Sprite[] objList;
    [SerializeField] GameObject TrashPrefab;

    [SerializeField] float StartPos, EndPos;
    [SerializeField] float ExistenceTime;
    float startTime;
    bool end = false;
    Vector2 pos, pos2;

    GameObject player, t;

    Coroutine attack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        startTime = Time.time;
        pos = new Vector2(player.transform.position.x, StartPos);
        pos2 = new Vector2(0f,0f);
        StartCoroutine(Down());
    }

    // Update is called once per frame
    void Update()
    {
        if(end) return;
        if(Time.time - startTime >= ExistenceTime)
        {
            StopCoroutine(attack);
            StartCoroutine(Back());
            return;
        }
    }

    IEnumerator Back()
    {
        end = true;
        yield return new WaitForSecondsRealtime(0.5f);
        for(float i = EndPos; i<=StartPos; i++)
        {
            pos.y = i;
            transform.position = pos;
                yield return new WaitForSecondsRealtime(0.2f);
        }
        yield return new WaitForSecondsRealtime(3f);
        Destroy(gameObject);
    }

        IEnumerator Down()
    {
        end = true;
        yield return new WaitForSecondsRealtime(0.5f);
        for(float i = StartPos; i>=EndPos; i--)
        {
            pos.y = i;
            transform.position = pos;
            yield return new WaitForSecondsRealtime(0.2f);
        }
        yield return new WaitForSecondsRealtime(3f);
        end = false;
        attack = StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while(true){
        for(int i = 1; i<=15; i++){
            t = Instantiate(TrashPrefab);
            t.GetComponent<SpriteRenderer>().sprite = objList[Random.Range(0, objList.Length)];
            pos2.x = transform.position.x + Random.Range(-0.8f, 0.8f) * transform.localScale.x/2;
            pos2.y = transform.position.y - transform.localScale.y/2; 
            t.GetComponent<Rigidbody2D>().linearVelocityY = Random.Range(1f, 20f);
            t.transform.position = pos2;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSecondsRealtime(0.5f);
    }
    }
}
