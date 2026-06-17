using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
   
    [SerializeField] float velocity_multiplier;
    [SerializeField] float jumpSpeed = 5f;
    //[SerializeField] float climbSpeed = 3f;
    //[SerializeField] float deathHeightSpeed = 5f;
    [SerializeField] float widenTime = 5f;
    [SerializeField] float widenHealth = 150f;

    [Header("Shooting")]
     [SerializeField] float firetime = 0.5f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun; 
    [SerializeField] AudioClip shootingclip;
    [SerializeField] [Range(0f, 1f)] float shootingVolume = 0.5f;
    [SerializeField]ParticleSystem FireParticles;

    Vector2 moveInput;
    Rigidbody2D myrigidbody;
    Animator legAnimator, gunAnimator;
    CapsuleCollider2D collider2d;
    BoxCollider2D legcollider2d;
    LayerMask layerMask;
    bool isAlive = true;

    public bool control = true, on_effector = false, backeffect = false;
    bool isWide = false, flip_sprite = true;
    HealthScript health;
    float curtime;

      Coroutine fire;
        int jumpcounter = 0;
        float legposn;
         float d;
    float startingGravityScale;

    [Header("CameraKeeping")]
        [SerializeField] double xDistance = 5f;
        [SerializeField] double yDistance = 2f;
        GameObject followPlayer;

    Vector2 util1;
    MusicPlayer musicplayer;

    void Start()
    {
        util1 = new Vector2(0,0);
        followPlayer = GameObject.FindWithTag("CameraFollow");
        myrigidbody = GetComponent<Rigidbody2D>();
        legAnimator = transform.Find("Legs").GetComponent<Animator>();
        gunAnimator = transform.Find("Rifle").GetComponent<Animator>();
        collider2d = GetComponent<CapsuleCollider2D>();
        legcollider2d = GetComponent<BoxCollider2D>();
        startingGravityScale = myrigidbody.gravityScale;
        health = GetComponent<HealthScript>();
        legposn = legcollider2d.offset.y;

        d = firetime;

        musicplayer = FindFirstObjectByType<MusicPlayer>();
    }

    void Update()
    {
        if(isAlive)
        {
            if(followPlayer != null){
                //This portion is so that the object that is supposed to hol cinemachine transform works
                util1.x = (float)(transform.position.x + xDistance);
                util1.y = (float)(transform.position.y + yDistance);
                followPlayer.transform.position = util1;
            }

             if(isWide && Time.time - curtime >= widenTime){
                isWide = false;
                transform.localScale = new Vector2(14, 14);
                legcollider2d.offset = new Vector2(legcollider2d.offset.x, legposn);
                musicplayer.StopAudio();
                musicplayer.PlayBGM();
                // health.PermaBoost(-widenHealth);
             }
            Run();
            flipSprite();
            if(health.gethealth() <= 0)
            {
                SceneManager.LoadScene("GameOverMenu");
                //health.Die();
            }
            //ClimbLadder();
           // Die(); 
        }
        else
        {
            return;
        }
    }

    void OnMove(InputValue value)
    {
        if(!control) return;
        if(!isAlive)
        { return;}
        moveInput = value.Get<Vector2>();
        if (backeffect && moveInput.x < 0)
        {
            moveInput.x = 0;
        }
        //Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if(!control) return;
        if(!isAlive)
        { return;}
        if(value.isPressed)
        {
            layerMask = LayerMask.GetMask("Ground");
            if(collider2d.IsTouchingLayers(layerMask) && legcollider2d.IsTouchingLayers(layerMask))
            {
                    myrigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
                    jumpcounter++;
             }
        }
    }

    void OnWiden(InputValue value)
    {
        if(!control) return;
        if(isWide)return;
        
        isWide = true;
        transform.localScale = new Vector2(70, 14);
        curtime = Time.time;
        legcollider2d.offset = new Vector2(legcollider2d.offset.x, legposn - 0.3f);
        //health.PermaBoost(widenHealth);
        health.TempBoost(health.gethealth() * widenHealth, widenTime);
        Debug.Log(health.gethealth());
        musicplayer.StopAudio();
        musicplayer.PlayWidenMusic();
    }
    void Run()
    {
        if(on_effector && moveInput.x == 0)
        {
            flip_sprite = false;
              legAnimator.SetBool("IsWalking", false);
            return;
        }
        flip_sprite = true;
        Vector2 playerVelocity = new Vector2(moveInput.x * velocity_multiplier, myrigidbody.linearVelocity.y);
      
        myrigidbody.linearVelocity = playerVelocity;

        legAnimator.SetBool("IsWalking", true);

        bool playerHasHorizontalSpeed = Mathf.Abs(myrigidbody.linearVelocity.x) > Mathf.Epsilon;
        if(!playerHasHorizontalSpeed)
        {
            legAnimator.SetBool("IsWalking", false);
        }
    }

    void flipSprite()
    {
        if(!flip_sprite)return;
        bool playerHasHorizontalSpeed = Mathf.Abs(myrigidbody.linearVelocity.x) > Mathf.Epsilon;
        float scale = 14f;
        if(playerHasHorizontalSpeed)
        {
         /* if(isWide)
            scale = 70f;
           else 
            scale = 14f;*/
          scale = Mathf.Abs(transform.localScale.x);
          transform.localScale = new Vector2(Mathf.Sign(myrigidbody.linearVelocity.x)* scale, 14f);
        }
    }

   /* void ClimbLadder()
    {
            layerMask = LayerMask.GetMask("Ladder");
            if(!collider2d.IsTouchingLayers(layerMask))
            {
              myrigidbody.gravityScale = startingGravityScale;
              return;
            }
           // Debug.Log(layerMask);
            Vector2 playerVelocity = new Vector2(myrigidbody.linearVelocity.x, moveInput.y * climbSpeed);
            myrigidbody.linearVelocity = playerVelocity;
            myrigidbody.gravityScale = 0;

             bool playerHasVerticalVelocity = Mathf.Abs(myrigidbody.linearVelocity.y) > Mathf.Epsilon;

        if(playerHasVerticalVelocity)
        {
            myAnimator.SetBool("isClimbing", true);
        }
        else
        {
            myAnimator.SetBool("isClimbing", false);
        }
    }

    void Die()
    {
        layerMask = LayerMask.GetMask("Enemy", "Traps");
        if(!collider2d.IsTouchingLayers(layerMask)){
           return;
        }
        myAnimator.SetBool("IsDead", true);
        myrigidbody.linearVelocity = new Vector2(myrigidbody.linearVelocity.x, deathHeightSpeed);
        isAlive = false;
        FindFirstObjectByType<GameSession>().ProcesslayerDeath();
    }
*/
    void OnAttack(InputValue value)
    {
        if(!control) return;
         if(!isAlive)
        { return;}
        
       if(value.isPressed)
        {
            gunAnimator.SetBool("isFiring", true);
            if(isWide)
                firetime = 0.1f;
            else
                firetime = d;
            fire = StartCoroutine(bulletSpawner(firetime));
        }
        else{
            if(fire != null)
                StopCoroutine(fire);
            gunAnimator.SetBool("isFiring", false);
        }
    }

    IEnumerator bulletSpawner(float delay)
    {
        while (true)
        {   
            GameObject bullet1 = Instantiate(bullet, gun.position, transform.rotation);
            bullet1.SetActive(true);
            AudioSource.PlayClipAtPoint(shootingclip, Camera.main.transform.position, shootingVolume);
            PlayEffectHit();
            if(isWide)
            {
                bullet1.GetComponent<Bullet>().SetDamage(Mathf.Abs(transform.localScale.x) * bullet1.GetComponent<Bullet>().GetDamage());
            }
            yield return new WaitForSecondsRealtime(delay);
        }
    }

    void PlayEffectHit()
    {
        if(FireParticles != null)
        {
            ParticleSystem effect = Instantiate(FireParticles, gun.position, Quaternion.Euler(0, 90, 0));
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
        }
    }
}
