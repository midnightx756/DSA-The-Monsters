using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
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
    float orthosize, xx, yy, camHeight, camWidth;
    Vector3 camPos;

    GameObject player;
    Rigidbody2D playerRigidBody2D;
    bool isDead = false;

    HealthScript health;

    Animator an;

    Coroutine cr = null;

    bool isInitiating = false;
    float hp;
     void Awake()
     {
          player = GameObject.FindWithTag("Player");
          playerRigidBody2D = player.GetComponent<Rigidbody2D>();
          pC2 = FindAnyObjectByType<CinemachineCamera>();
          health = GetComponent<HealthScript>();
          an = GetComponent<Animator>();
     }
     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
        if(pC2 == null)
        {
            Debug.Log("Ni*ga");
        }
        if(player == null)
        {
            Debug.Log("HAHA");
        }
        //an.SetBool(IsSummoning, true);
        cr = StartCoroutine(Attack(2));
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead) return;
        if(health.gethealth() <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(BossDie());
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
        isDead = true;
        yield return new WaitForSeconds(20);
        health.Die();
     }

     IEnumerator Attack(int phase)
    {
        yield return null;
        if(phase == 1){
            while(true){
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
                yield return new WaitForSecondsRealtime(20f);
             }
        }
        if(phase == 2)
        {
            while(true){
                yield return new WaitForSecondsRealtime(5f);
                TrashSummon();
                yield return new WaitForSecondsRealtime(10f);
                SteamSummon();
                yield return new WaitForSecondsRealtime(20f);
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
