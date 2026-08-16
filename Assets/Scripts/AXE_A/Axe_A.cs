using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines.ExtrusionShapes;
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthScript))]
[RequireComponent(typeof(BoxCollider2D))]

public class Axe_A : MonoBehaviour
{

    [SerializeField] float Distance;
    [SerializeField] int Score;

    [Header("Walk")]
        [SerializeField] List<Sprite> Walk;
        [SerializeField] float WalkSpeed;
        Sprite stand;
    [Header("Attack1")]
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] int numBulU, numBulL;
    [SerializeField] float Firerate;
    [SerializeField] float Duration;
    [SerializeField] float Arc = 60f;
    [SerializeField] GameObject Barrel;

    [Header("Attack2")]
    [SerializeField] float angularVelocity;
    [SerializeField] int numRays;
    [SerializeField] GameObject Rays, main;
    [SerializeField] float Durain;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Attack3")]
    [SerializeField] float DashSpeed;
    [SerializeField]float maxHeight ;
    [SerializeField] float minHeight;
    [SerializeField] int numSmashes;
    [SerializeField] float FlightSpeed = 1f;
    [SerializeField] List<Sprite> Flight;
    GameObject player, ground, Shield;

    Rigidbody2D rb2D;
    SpriteRenderer sr;
    Vector2 rr, poss;

    [Header("Brain")]
    [SerializeField] float BrainSize = 10f;
    int numAttacks = 3;
    bool processing, isDead = false;
    float nonWideInitHealth, nonWideFinHealth, WideInitHealth, WideFinHealth;

    BoxCollider2D hitb;
    bool walkr, isAttack = false, flipSprite = true, noReposition = false;
    Coroutine ww, wk;

    ParticleSystem ps;
    HealthScript hs;
    float t, lr;
    void Start()
    {
        //StartCoroutine(SwarmFire());
        player = GameObject.FindWithTag("Player");
        ground = GameObject.FindWithTag("Ground");
        
        sr = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        hitb = GetComponent<BoxCollider2D>();
        hs = GetComponent<HealthScript>();

        ps = GetComponentInChildren<ParticleSystem>();
        Shield = transform.GetChild(1).gameObject;

        if(Shield == null)
        {
                Debug.Log("NoShieldFoundException");    
        }

         lr = transform.position.y;

        stand = sr.sprite;
        rr = new();
        poss = new();

        StartCoroutine(Brain());
        //StartCoroutine(Dash());
        //StartCoroutine(SwarmFire());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if(hs.gethealth() <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(nameof(Death));
            return;
        }
        t = transform.position.x - player.transform.position.x;
        if(flipSprite && Mathf.Sign(t) != Mathf.Sign(transform.localScale.x))
        {
            poss.x = -transform.localScale.x;
            poss.y = transform.localScale.y;
            transform.localScale = poss;
        }
        if (processing)
        {
            if(player.GetComponent<PlayerMovement>().IsPlayerWide()){
                WideFinHealth = player.GetComponent<HealthScript>().gethealth();
                if(WideFinHealth > WideInitHealth)
                    WideInitHealth = player.GetComponent<HealthScript>().gethealth();
            }
            else
            {
                nonWideFinHealth = player.GetComponent<HealthScript>().gethealth();
            }
        }
        if (isAttack)
        {
            if(!Shield.activeInHierarchy)
                Shield.SetActive(true);
            return;
        }
        else if (!isAttack)
        {
             if(Shield.activeInHierarchy)
                Shield.SetActive(false);
        }
        if(noReposition) return;
        if(Mathf.Abs(t) > Distance){
            if(!walkr)
                ww = StartCoroutine(Manimator());
            poss.y = lr;
            poss.x = ground.transform.position.x + Mathf.Sign(transform.localScale.x) * Distance;
            transform.position = poss;
        }
        else if(Mathf.Abs(t) <= Distance && player.GetComponent<Rigidbody2D>().linearVelocityX == 0 )
        {
            if(ww != null)
                StopCoroutine(ww);
            walkr = false;
            sr.sprite = stand;
        }
        //transform.position = poss;
    }

    IEnumerator Manimator()
    {
        walkr = true;
        foreach(var x in Walk)
        {
            sr.sprite = x;
            yield return new WaitForSecondsRealtime(1/WalkSpeed);
        }
        walkr = false;
    }

    IEnumerator Death()
    {
        isDead = true;
        ScoreKeeper ss =FindAnyObjectByType<ScoreKeeper>();
        if(ss != null) ss.UpdateScore(Score);
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene("EndGameMenu");
        //Destroy(gameObject);
    }

    //Attacks
    IEnumerator SwarmFire()
    {
        isAttack = true;
        float cur = Time.time;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        while(Time.time - cur < Duration){
            float numBul = Random.Range(numBulL, numBulU);
            float seg = Arc/numBul;
            for(int i = 0; i<numBul; i++)
            {
                    GameObject inst = Instantiate(BulletPrefab);
                    inst.transform.position = Barrel.transform.position;
                    inst.transform.rotation = Quaternion.Euler(0,0, (i - numBul/2)* seg);
                    rr =   inst.transform.localScale;
                    rr.x *= Mathf.Sign(transform.localScale.x);
                    inst.transform.localScale = rr;
                    StartCoroutine(inst.GetComponent<BulletAkss>().StayStill());
            }
            yield return new WaitForSecondsRealtime(1/Firerate);
        }
        yield return new WaitForSecondsRealtime(20f);
        isAttack = false;
        GetComponent<Rigidbody2D>().gravityScale = 1f;
    }

    IEnumerator Sun()
    {
        //Give up;
        isAttack = true;

        rb2D.gravityScale = 0;

        GameObject ss = Instantiate(main);
        ss.transform.position = transform.position;
        ss.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.4f);
        List<GameObject> inst = new();
        float radi = 360/numRays;
        rr.x = Mathf.Abs(transform.position.x - player.transform.position.x);
        for(int i = 1; i <= numRays; i++)
        {
            inst.Add(Instantiate(Rays));
            inst[i-1].transform.rotation = Quaternion.Euler(0,0,i*radi);
            inst[i-1].transform.position = transform.position;
            rr.y = inst[i-1].transform.localScale.y * 3;
            inst[i-1].transform.localScale = rr;
            inst[i-1].GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.09f);
        }
        yield return new WaitForSecondsRealtime(10f);
          ss.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1f);
        for(int i = 0; i <numRays; i++)
        {
            inst[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1f);
        }
        if(inst == null){
            Debug.Log("Nothing instantiated");
            yield return null;
        }
        float T = Time.time;
        while(Time.time - T < Durain){
            for(int i = 0; i< inst.Count; i++)
            {
                inst[i].transform.Rotate(0,0,angularVelocity * Time.deltaTime);
            }
            yield return new WaitForEndOfFrame();
        }
        for(int i = 0; i <numRays; i++)
        {
            inst[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.09f);
        }
        ss.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.09f);
        yield return new WaitForSecondsRealtime(2f);
        for(int i = inst.Count - 1; i>=0; i--)
            Destroy(inst[i]);
        Destroy(ss);
         isAttack = false;
          rb2D.gravityScale = 0;
    }

    IEnumerator Dash()
    {
        noReposition = true;
        rb2D.gravityScale = 0f;
        //rb2D.linearVelocityY = DashSpeed;
        hitb.isTrigger = true;

        hs.BoostDefence(100);

        for(int i = 1; i<= numSmashes; i++)
        {
            ps.Play();
             wk = StartCoroutine(Animation(Walk, WalkSpeed));
             rb2D.linearVelocityY = DashSpeed;
             while(transform.position.y < maxHeight)yield return new WaitForEndOfFrame();
              rb2D.linearVelocityY = 0;
             yield return new WaitForSecondsRealtime(0.05f);
             poss.x =  Mathf.Abs(transform.position.x - player.transform.position.x);
             poss.y =  Mathf.Abs(transform.position.y - player.transform.position.y);
             rr = transform.position;
             StopCoroutine(wk);

             wk = StartCoroutine(Animation(Flight, FlightSpeed));
             transform.rotation = Quaternion.Euler(0,0, 180  -  (Mathf.Sign(transform.localScale.x) * Mathf.Atan2(poss.x, poss.y) *  Mathf.Rad2Deg));
              rr.x = transform.position.x -2 * Mathf.Sign(transform.localScale.x) * poss.x;
              rr.y = transform.position.y - 2* poss.y;
             yield return new WaitForSecondsRealtime(0.05f);
             flipSprite = false;
             rb2D.linearVelocityX = -DashSpeed * Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad);
             rb2D.linearVelocityY = DashSpeed  * Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad);
             int it = 0;
            while(it< 100 && transform.position.y > rr.y)
            {
                it++;
                yield return new WaitForEndOfFrame();
            }
             flipSprite = true;
            rb2D.linearVelocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
            yield return new WaitForSecondsRealtime(0.01f);
            StopCoroutine(wk);
            ps.Stop();
        }
        hs.BoostDefence(-100);
        hitb.isTrigger = false;
        noReposition = false;
        rb2D.gravityScale = 1f;
    }

     void OnTriggerEnter2D(Collider2D collision)
     {
        if(!noReposition) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthScript>().DealDamage(30);
        }
     }

     /*IEnumerator ConstMove()
    {
        while(true){
            foreach(var x in ll)
            {
                sr.sprite = x;
                yield return new WaitForSecondsRealtime(1/Speed);
            }
        }
    }*/

    IEnumerator Animation( List<Sprite> ll, float Speed)
    {
        while(true){
            foreach(var x in ll)
            {
                sr.sprite = x;
                yield return new WaitForSecondsRealtime(1/Speed);
            }
        }
    }

    IEnumerator Brain()
    {
        int[] res = new int[numAttacks];
        res[0] = 0;
        res[1] = 1;
        res[2] = 2;
        //StartCoroutine("");
        float[] rowData = new float[numAttacks];

        List<float[]> damageTable = new List<float[]>();
        string[] attackArr = {"SwarmFire", "Sun", "Dash"};

        int countItems = 0;

        System.Random rng = new System.Random();
        int n = numAttacks;
        while(true){
            //Random trials
            while(damageTable.Count < BrainSize - countItems){
                    while(n > 1)
                {
                    n--;
                    int k = rng.Next(n+1);

                    int te = res[k];
                    res[k] = res[n];
                    res[n] = te;
                }
                    yield return StartCoroutine(InitializeAttack(damageTable, attackArr, rowData, res));
            }

            //If List is full then we need to calculate the main rms
            if(countItems >= BrainSize)
            {
                countItems = 0;
            }

            yield return new WaitForEndOfFrame();

            //RMS Value Summation
            foreach(float[] x in damageTable)
            {
                rowData[0] = x[0]* x[0];
                rowData[1] = x[1]* x[1];
                rowData[2] = x[2]* x[2];
            }

            //Calculate RMS
            rowData[0] = Mathf.Sqrt(rowData[0]/BrainSize);
            rowData[1] = Mathf.Sqrt(rowData[1]/BrainSize);
            rowData[2] = Mathf.Sqrt(rowData[2]/BrainSize);

            res[0] = 0;
            res[1] = 1;
            res[2] = 2;
            bool r = false;
            float t; int it;
            for(int i = 0; i< numAttacks-1; i++)
            {
                r = false;
                for(int j = 0; j< numAttacks - 1- i; j++)
                {
                    if(rowData[j] < rowData[j + 1])
                    {
                        t = rowData[j];
                        rowData[j] = rowData[j+1];
                        rowData[j+1] = t;
                        it = res[j];
                        res[j] = res[j+1];
                        res[j+1] = it;
                        r = true;
                    }
                }
                if(!r) break;
            }

            yield return new WaitForSecondsRealtime(25f);
            if(damageTable.Count > countItems)
            {
                damageTable.RemoveRange(countItems, damageTable.Count - countItems);
            }
            yield return StartCoroutine(InitializeAttack(damageTable, attackArr, rowData, res));
            countItems++;
        }
    }

    IEnumerator InitializeAttack(List<float[]> damageTable, string[] attackArr, float[] rowData, int[] res)
    {     
        for(int i = 0; i< numAttacks; i++){
                 WideFinHealth = 0;
                WideInitHealth = 0;
                nonWideFinHealth = 0;
                nonWideInitHealth = 0;
                processing = true;
                if(player.GetComponent<PlayerMovement>().IsPlayerWide())
                    WideInitHealth = player.GetComponent<HealthScript>().gethealth();
                else
                    nonWideInitHealth = player.GetComponent<HealthScript>().gethealth();
            
                yield return StartCoroutine(attackArr[res[i]]);

                processing = false;

                rowData[res[i]] = Mathf.Abs(WideInitHealth - WideFinHealth) + Mathf.Abs(nonWideInitHealth - nonWideFinHealth);

                yield return StartCoroutine(ActivateShield(10f,5f));
        }
        damageTable.Add(rowData);
        yield return StartCoroutine(ActivateShield(10f, 10f));

    }

    IEnumerator ActivateShield(float WithShield, float WithoutShield)
    {
         if(!Shield.activeInHierarchy)
                    Shield.SetActive(true);
                yield return new WaitForSecondsRealtime(WithShield);
                if(Shield.activeInHierarchy)
                    Shield.SetActive(false);
                yield return new WaitForSecondsRealtime(WithoutShield);
    }
}
