using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [Header("Enemies")]
    [SerializeField] List<GameObject> EnemyPrefabs;

    [Header("Bruh")]
    [SerializeField] float distance;
    [SerializeField] float Height;

    GameObject Player, temp;
    Vector2 ob;

    int n, t;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        ob  = new Vector2(0,0);
        n = EnemyPrefabs.Count;
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        ob.x = Player.transform.position.x + distance;
        ob.y = Height;
        transform.position = ob;
    }

    IEnumerator Spawn()
    {
        while(true){
            yield return new WaitForEndOfFrame();
            t = UnityEngine.Random.Range(0, n);
            temp = Instantiate(EnemyPrefabs[t]);
            temp.transform.position = transform.position;
            yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(1f, 7f));
        }
    }
}
