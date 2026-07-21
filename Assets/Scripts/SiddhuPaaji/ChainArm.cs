using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.WindowsMR.Input;

public class ChainArm : MonoBehaviour
{
    [UnitHeaderInspectable("Refs")]
    public LineRenderer line;
    public Transform shootPoint;
    public GameObject HandPrefab;

    [Header("Settings")]
    public int segments = 6;   
    public float shootSpeed = 30f;
    public float yankSpeed = 50f;
    public float holdTime = 1f;

    private GameObject hand;
    private HandGrab handGrab;
    private bool isGrappling = false;

    private Rigidbody2D grabbedRb;

    private SiddhuPaaji bossAI; 
     // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        line.positionCount = segments;
        bossAI = GetComponentInParent<SiddhuPaaji>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateChainVisual();
    }

    public void FireChain()
    {
        if(hand != null) return;

        Vector2 dir = (FindFirstObjectByType<PlayerMovement>().transform.position - shootPoint.position).normalized;

        hand = Instantiate(HandPrefab, shootPoint.position, Quaternion.identity);
        handGrab = hand.GetComponent<HandGrab>();
        handGrab.Init(this);

        Rigidbody2D handRb = hand.GetComponent<Rigidbody2D>();
        handRb.linearVelocity = dir * shootSpeed;
    }

    void UpdateChainVisual()
    {
        Vector3 endPos = hand ? hand.transform.position : shootPoint.position;

        for(int i = 0; i< segments; i++)
        {
            float t = i/(float)(segments - 1);
            Vector3 pos = Vector3.Lerp(shootPoint.position, endPos, t);
            pos.y -= Mathf.Sin(t * Mathf.PI) * 0.4f;
            line.SetPosition(i, pos);
        }
    }

    public void OnGrab(Transform player)
    {
        if(isGrappling) return;

        isGrappling = true;

        grabbedRb = player.GetComponent<Rigidbody2D>();
        
        StartCoroutine(YankPlayer(player));
    }

    IEnumerator YankPlayer(Transform player)
    {
        while(Vector2.Distance(player.position, hand.transform.position) > 0.5f)
        {
            Vector2 dir = (hand.transform.position - player.transform.position).normalized;
            grabbedRb.linearVelocity = dir * yankSpeed;
            yield return null;
        }

        grabbedRb.linearVelocity = Vector2.zero;
        grabbedRb.bodyType = RigidbodyType2D.Kinematic;
        player.position = hand.transform.position;

        Vector3 ceiling = new Vector3(player.position.x, 8f, 0);
        Vector3 floor = new Vector3(player.position.x, -4f, 0);

        for(int i = 0; i< 3; i++)
        {
            while(Vector2.Distance(player.position, ceiling) > 0.1f)
            {
                player.position = Vector3.MoveTowards(player.position, ceiling, yankSpeed * Time.deltaTime);
                hand.transform.position = player.transform.position;
                yield return null;
            }
            while(Vector2.Distance(player.position, floor) > 0.1f)
            {
                player.position = Vector3.MoveTowards(player.position, floor, yankSpeed * Time.deltaTime);
                hand.transform.position = player.transform.position;
                yield return null;
            }
        }


        Release();
    }

    public void Release()
    {
        if (grabbedRb)
        {
            grabbedRb.bodyType = RigidbodyType2D.Dynamic;
            grabbedRb.GetComponent<PlayerMovement>().enabled = true;
        }

        if(hand) Destroy(hand);
        isGrappling = false;
    }
}
