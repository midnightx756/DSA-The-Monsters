using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

public class BounceBreaker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]  public float rebound = 10f;
    [SerializeField] public float lifetime = 100f;
    GameObject arm, padbear, pad, temp;
    CapsuleCollider2D padCollider;

    Rigidbody2D rb2d;
    Vector2 ob, posArm, posPad, posPadBear;
    PlayerMovement pm;
    float mag = 0;
    float start;
    bool isSummoningOrDying = false;
     void Awake()
     {
            ob = new Vector2(0,0);
            StartCoroutine(goUP());
            arm = transform.GetChild(1).gameObject;
            padbear = transform.GetChild(2).gameObject;
            pad = transform.GetChild(3).gameObject;
            temp = GameObject.FindWithTag("Player");

            rb2d = temp.GetComponent<Rigidbody2D>();
            pm = temp.GetComponent<PlayerMovement>();
            padCollider = pad.GetComponent<CapsuleCollider2D>();

     }

    //Coroutine to show coming up
    IEnumerator goUP()
    {
        isSummoningOrDying = true;
        ob.x = transform.position.x;
        ob.y = -5;
        transform.position = ob;
        for(int i = -5; i<=0;i++)
        {
            ob.y = i;
            ob.x = transform.position.x;
            transform.position = ob;
            yield return new WaitForSecondsRealtime(0.4f);
        }
         ob.x = transform.position.x;
        ob.y = 0.5f;
        transform.position = ob;
        start = Time.time;
        posPad = pad.transform.position;
        posArm = arm.transform.position;
        posPadBear = padbear.transform.position;
        isSummoningOrDying = false;
    }

    public void throwBack(GameObject gameObject)
    {
        Debug.Log("UNity Physics shat on me");
        return;
        //StartCoroutine(throwr(gameObject));
        //Debug.Log(temp.GetComponent<Rigidbody2D>().linearVelocityX);
    }

   /* IEnumerator throwr(GameObject gameObject)
    {
      PlayerMovement mv = gameObject.GetComponent<PlayerMovement>();
      Rigidbody2D rb2d = gameObject.GetComponent<Rigidbody2D>();

      mv.control = false;

      yield return new WaitForFixedUpdate();

      rb2d.linearVelocity = Vector2.zero;

        Vector2 dir = (gameObject.transform.position - transform.position).normalized;
        dir.y = 0.3f;
        dir.Normalize();

        rb2d.AddForce(dir * rebound, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);
        mv.control = true;

    }*/


    IEnumerator goDown()
    {
        isSummoningOrDying = true;
        ob.x = transform.position.x;
        ob.y = 0.5f;
        transform.position = ob;
        for(int i = 0; i>=-5;i--)
        {
            ob.y = i;
            ob.x = transform.position.x;
            transform.position = ob;
            yield return new WaitForSecondsRealtime(0.4f);
        }
        yield return new WaitForSecondsRealtime(10f);
        Destroy(gameObject);
       // isSummoningOrDying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSummoningOrDying) return;
        if(Time.time - start >= lifetime)
        {
            StartCoroutine(goDown());
        }
        if (padCollider.IsTouching(temp.GetComponent<CapsuleCollider2D>()))
        {
            pm.control = false;
            //rb2d.linearVelocityX = rebound* -10;
            ob.x = temp.transform.position.x - rebound;
            ob.y = temp.transform.position.y + 0.5f;
            temp.transform.position = ob;
            ScoreKeeper sc = FindAnyObjectByType<ScoreKeeper>();
            if(sc != null)
            {
                sc.UpdateScore(100);
                Debug.Log("Score: " + sc.GetScore());
            }
            Debug.Log("KnockBacked");
            pm.control = true;
            
        }

        mag = Mathf.Sin((Mathf.PI * Time.time) % (2 * Mathf.PI));
        ob.x = posArm.x  + mag* (arm.transform.localScale.x/2);
        ob.y = posArm.y;
        arm.transform.position = ob;
        ob.x -= 2;
        padbear.transform.position = ob;
        ob.x -= 0.5f;
        pad.transform.position = ob;
    }
}
