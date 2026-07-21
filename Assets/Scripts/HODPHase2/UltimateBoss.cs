using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class UltimateBoss : MonoBehaviour
{
     private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
     //private static readonly int IsSummoning = Animator.StringToHash("isSummoning");

      //private static readonly int IsSummoningWall = Animator.StringToHash("isSummoningWall");
     //CinemachineVirtualCamera playerCamera;
     CinemachineCamera pC2;
    [SerializeField] float within = 10f;
    [SerializeField] GameObject PistonPrefab;
    [SerializeField] GameObject WallPrefab;

    [SerializeField] GameObject TrashThrower, SteamVent;
    float orthosize, xx, yy, camHeight, camWidth, k;
    Vector3 camPos;


    [Header("Phases")]
    [SerializeField] float phase1Heath = 100000f;
    GameObject player;
    Rigidbody2D playerRigidBody2D;
    bool isDead = false;

    [Header("Phase 2")]
        [SerializeField] float attackpadding = 10f;
    GameObject Ground;

    [Header("Do not edit")]
    public static int phase;
    HealthScript health;

    Animator an;

    Coroutine cr = null;
    public static bool istransitioning = false;
    bool isInitiating = false;
    public static float hp;
    public static UltimateBoss Instance {get; private set;}

    //Flags for phase;
    static bool phase2 = false;
     void Awake()
     {
        if(Instance != null){Destroy(gameObject); return;}
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("Before transition: " + phase + " "+phase2 + " "+ istransitioning);
        istransitioning = false;
        phase2 = false;
        phase = 0;
        Debug.Log("After transition: " + phase + " "+phase2 + " "+ istransitioning);
        //Debug.Log(SceneManager.GetActiveScene().name);
       //  if (!SceneManager.GetActiveScene().name.Contains("HOD"))
         //{
            //Debug.Log("This is not a battle scene");
             //Destroy(gameObject);
           //  return;
         //}
        //GameObject[] r =  GameObject.FindGameObjectsWithTag("Boss");
        //int numBosses = r.Length;

     }
     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
        Debug.Log("FuckYou");
        hp = health.gethealth();
        /*int l = FindObjectsOfType<UltimateBoss>().Length;
        if(l > 1)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }*/
      
    }

     void OnEnable()
     {
          SceneManager.sceneLoaded += OnSceneLoaded;
          Debug.Log("Enabled");
     }

     void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       List<string> gamePlayScenes = new List<string>
       {
            "HODSir",
            "HODSirPhase2"
       };//{"HODSir", "HODSirPhase2"};
       if(!gamePlayScenes.Contains(scene.name)){
            if(health.gethealth() <= 0)
            {
                Debug.Log("Supposed to go in end game");
            }
            Debug.Log("This is not a battle scene rather " + scene.name);
            isDead = true;
            StopAllCoroutines();
            Destroy(gameObject);
            return;
        }
          player = null;
          isDead = false;
          istransitioning = false;
        player = GameObject.FindWithTag("Player");
          playerRigidBody2D = player.GetComponent<Rigidbody2D>();
          pC2 = FindAnyObjectByType<CinemachineCamera>();

          Ground =  GameObject.FindGameObjectWithTag("GroundChild");
          if(Ground == null)
        {
            Debug.Log("Ground Not accquired");
        }
          health = GetComponent<HealthScript>();
          k = (health.gethealth() > 0) ?  health.gethealth() : 100;
          an = GetComponent<Animator>();
          if(phase == 1){
            Debug.Log("Yeah Repositioning");
            transform.position = new Vector2(10f, 15f);
          }
        if(pC2 == null)
        {
            Debug.Log("Ni*ga");
        }
        if(player == null)
        {
            Debug.Log("HAHA");
        }
        //an.SetBool(IsSummoning, true);
        cr = StartCoroutine(Attack(++phase));
    }

     void OnDisable()
     {
          SceneManager.sceneLoaded -= OnSceneLoaded;
          StopAllCoroutines();
     }

     void OnDestroy(){
         StopAllCoroutines();
    }
     // Update is called once per frame
     void Update()
    {
        if(isDead) return;
        if(health.gethealth() <= 0)
        {
            Debug.Log("Bro Died");
            StopAllCoroutines();
            Debug.Log("Stopped everything");
            StartCoroutine(BossDie());
            return;
        }
        if(player == null)
        {
            Debug.Log("Player is not found lol");
            return;
        }
        if (!SceneManager.GetActiveScene().name.Contains("HOD"))
         {
            Debug.Log("This is not a battle scene");
             Destroy(gameObject);
             return;
         }
          if(istransitioning){
              //Debug.Log("Hehe");
              return;
        }
        if(!phase2 && health.gethealth() <= phase1Heath)
        {
            StopAllCoroutines();
            istransitioning = true;
            phase2 = true;
            Debug.Log("Loading Phase " + phase);
            SceneManager.LoadScene("HODSirPhase2");
            //Start();
            return;
        }
        if (isInitiating)
        {
            //Debug.Log("Summoning is going on");
            //Debug.Log(an.GetNextAnimatorClipInfo(0));
            an.SetBool(IsWalkingHash, false);
            return;
        }
        if(pC2 == null)
        {
            return;
        }
        if(playerRigidBody2D.linearVelocityX != 0)
            an.SetBool(IsWalkingHash, true);
        else 
            an.SetBool(IsWalkingHash, false);
        orthosize = pC2.Lens.OrthographicSize;
        camHeight = 2f* orthosize;
        camWidth = camHeight * Camera.main.aspect;

        camPos = pC2.transform.position;

        xx = camPos.x + (camWidth/ 2f) + within;
        yy = transform.position.y;

        camPos.x = xx;
        camPos.y = yy;
        camPos.z = 0;
        transform.position = camPos;
    }

     IEnumerator BossDie()
     {
        Debug.Log("Here Bruh");
        isDead = true;
        //StopAllCoroutines();
        FindAnyObjectByType<ScoreKeeper>().UpdateScore(2000000);
        yield return new WaitForSeconds(5);
        Debug.Log("Won");
        SceneManager.LoadScene("EndGameMenu");
        //health.Die();
     }

     IEnumerator Attack(int phase)
    {
        yield return null;
        if(phase == 1){
            while(true){
                Debug.Log("Phase 1 initiated");
                if(isDead || this == null) yield break;
                yield return new WaitForSecondsRealtime(3f);
                isInitiating = true;
                an.SetTrigger("SpawnPiston");
                //yield return new WaitForSecondsRealtime(15f);
                var clipInfo = an.GetCurrentAnimatorClipInfo(0);
                AnimationClip clip = clipInfo[0].clip;
                int f = Mathf.RoundToInt(clip.length * clip.frameRate);
                for(int i = 0; i< f; i++)
                {
                    yield return new WaitForEndOfFrame();
                }
                //yield return new WaitForSeconds(clip.length);
                //an.SetBool(IsSummoning, false);
                isInitiating = false;
                PistonAttack();

                yield return new WaitForSecondsRealtime(10f);

                isInitiating = true;
                //an.SetBool(IsSummoningWall, true);
                an.SetTrigger("SpawnWall");
                clipInfo = an.GetCurrentAnimatorClipInfo(0);
                clip = clipInfo[0].clip;
                f =  Mathf.RoundToInt(clip.length * clip.frameRate);
                for(int i = 0; i<= f; i++)
                {
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSecondsRealtime(clip.length);
                //an.SetBool(IsSummoningWall, false);
                WallSpawn();
                isInitiating = false;
                yield return new WaitForSecondsRealtime(10f);
             }
        }
        if(phase == 2)
        {
            Debug.Log("Phase 2 initiated");
            while(true){
                if(isDead || this == null|| player == null|| Ground == null) yield break;
                yield return new WaitForSecondsRealtime(5f);
                if(Mathf.Abs(player.transform.position.x - Ground.transform.position.x) > attackpadding)
                {
                    isInitiating = true;
                    an.SetTrigger("SpawnSteam");
                    //yield return new WaitForSecondsRealtime(15f);
                    var clipInfo = an.GetCurrentAnimatorClipInfo(0);
                    AnimationClip clip = clipInfo[0].clip;
                    int f = Mathf.RoundToInt(clip.length * clip.frameRate);
                    for(int i = 0; i< f; i++)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    isInitiating = false;
                    SteamSummon();
                     yield return new WaitForSecondsRealtime(health.gethealth()/ k *  10 + 1);
                }
                else if(Mathf.Abs(player.transform.position.x - Ground.transform.position.x) <= attackpadding)
                {
                      isInitiating = true;
                     an.SetTrigger("SpawnTrash");
                    //yield return new WaitForSecondsRealtime(15f);
                    var clipInfo = an.GetCurrentAnimatorClipInfo(0);
                    AnimationClip clip = clipInfo[0].clip;
                    int f = Mathf.RoundToInt(clip.length * clip.frameRate);
                    for(int i = 0; i< f; i++)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    isInitiating = false;
                    TrashSummon();
                    //yield return new WaitForSecondsRealtime(20f);
                    yield return new WaitForSecondsRealtime(health.gethealth()/ k *  10 + 1);
                }
                //isInitiating = true;
                /*an.SetTrigger("SpawnTrash");
                //yield return new WaitForSecondsRealtime(15f);
                var clipInfo = an.GetCurrentAnimatorClipInfo(0);
                AnimationClip clip = clipInfo[0].clip;
                int f = Mathf.RoundToInt(clip.length * clip.frameRate);
                for(int i = 0; i< f; i++)
                {
                    yield return new WaitForEndOfFrame();
                }
                isInitiating = false;
                TrashSummon();
                yield return new WaitForSecondsRealtime(10f);
                 isInitiating = true;
                an.SetTrigger("SpawnSteam");
                //yield return new WaitForSecondsRealtime(15f);
                clipInfo = an.GetCurrentAnimatorClipInfo(0);
                clip = clipInfo[0].clip;
                f = Mathf.RoundToInt(clip.length * clip.frameRate);
                for(int i = 0; i< f; i++)
                {
                    yield return new WaitForEndOfFrame();
                }
                isInitiating = false;
                SteamSummon();
                yield return new WaitForSecondsRealtime(20f);*/
            }
        }
    }

    void PistonAttack()
    {
        //if(an.GetBool(IsSummoning)) {Debug.Log("OOPs, I was summoning"); return;}
        GameObject temp = Instantiate(PistonPrefab);
        temp.transform.position = new Vector2(player.transform.position.x + 10f, -5);
    }

    void WallSpawn()
    {
        GameObject temp = Instantiate(WallPrefab);
        temp.transform.position = new Vector2(transform.position.x - 10f, 14.3f);
    }

    void TrashSummon()
    {
        GameObject temp = Instantiate(TrashThrower);
        temp.transform.position = new Vector2(player.transform.position.x, TrashThrower.transform.position.y);
    }

      void SteamSummon()
    {
        GameObject temp = Instantiate(SteamVent);
        temp.transform.position = new Vector2(transform.position.x - 10f, SteamVent.transform.position.y);
    }
}
