using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
public class LaserSpawner : MonoBehaviour
{
    [SerializeField] GameObject laser;
    [SerializeField] List<Transform> shooter;
    //[SerializeField] float duration = 2f;
   // [SerializeField] float damage = 0.5f;

       // List<GameObject> l;
        public Transform Target;
        GameObject dummy;
       // HealthScript hp;
        void Awake()
       {
         // dummy = new GameObject("Dummy");
          //hp = dummy.GetComponent<HealthScript>();
          //vec = dummy.transform;
          //l = new List<GameObject>(shooter.Count+1);
          //Debug.Log("Initially: " + l.Count);
      }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Spawn(string Tag)
    {
        //StartCoroutine(LaserRoutine(Target));
         for(int i =0; i< shooter.Count; i+=1)
        {
            dummy = GameObject.FindWithTag(Tag);
            Target = dummy.GetComponent<Transform>();
            //vec.position = shooter[i].position;
           //Debug.Log(shooter[i]);
           //vec.rotation = Quaternion.Euler(0, 0, Mathf.Atan2((Target.position.y - shooter[i].position.y), (Target.position.x - shooter[i].position.x)) *  Mathf.Rad2Deg);
           float distance = Mathf.Sqrt((Target.position.x - shooter[i].position.x) * (Target.position.x - shooter[i].position.x) + (Target.position.y - shooter[i].position.y)* (Target.position.y - shooter[i].position.y));
          GameObject temp = Instantiate(laser, shooter[i]);
          temp.transform.rotation = Quaternion.Euler(0, 0, -270 -  Mathf.Abs(Mathf.Atan2((Target.position.y - shooter[i].position.y), (Target.position.x - shooter[i].position.x)) *  Mathf.Rad2Deg));
          temp.transform.localScale =  new Vector2(laser.transform.localScale.x, 1.5f * distance);
            //vec.localScale = new Vector2(laser.transform.localScale.x, 4*distance);
           // l.Add(Instantiate(laser, shooter[i]));
           // l[i].transform.rotation = Quaternion.Euler(0, 0, -270 -  Mathf.Abs(Mathf.Atan2((Target.position.y - shooter[i].position.y), (Target.position.x - shooter[i].position.x)) *  Mathf.Rad2Deg));
           // l[i].transform.localScale =  new Vector2(laser.transform.localScale.x, 4*distance);
                  //Debug.Log(l.Count);
        }
    }
/*
    IEnumerator LaserRoutine(Transform Target)
  {
        Debug.Log(l.Count);
        yield return new WaitForSecondsRealtime(duration);
          for(int i =0; i< l.Count; i++)
        {
          if(l[i] != null)
              Destroy(l[i]);
        }
        l.Clear();
  }*/
}
