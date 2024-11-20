using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float Speed;

    public GameManager manager;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    GameObject scanObject;
    Animator anim;


    Vector2 inputVec;
    bool isHorizonMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Move Value
        inputVec.x = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        inputVec.y = manager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        // Check Button Down & Up
        bool hDown = manager.isAction ? false : Input.GetButtonDown("Horizontal");
        bool vDown = manager.isAction ? false : Input.GetButtonDown("Vertical");
        bool hUp = manager.isAction ? false : Input.GetButtonUp("Horizontal");
        bool vUp = manager.isAction ? false :Input.GetButtonUp("Vertical");

        // Check Horizontal Move
        if (hDown)
         isHorizonMove = true;
        else if (vDown)
         isHorizonMove = false;
        else if (hUp || vUp)
            isHorizonMove = inputVec.x != 0;

        // Animation
        anim.SetFloat("Speed", inputVec.sqrMagnitude);
        //if (anim.GetInteger("hAxisRaw") != h)
        //{
        //    anim.SetBool("isChange", true);
        //    anim.SetInteger("hAxisRaw", (int)h);
        //}
        //else if (anim.GetInteger("vAxisRaw") != v)
        //{
        //    anim.SetBool("isChange", true);
        //    anim.SetInteger("vAxisRaw", (int)v);
        //}
        //else
        //{
        //    anim.SetBool("isChange", false);
        //}

        // Direction
        //if (vDown && v == 1) dirVec = Vector3.up;
        //else if (vDown && v == -1) dirVec = Vector3.down;
        //else if (hDown && h == -1) dirVec = Vector3.left;
        //else if (hDown && h == 1) dirVec = Vector3.right;

        // Scan Object (for interaction)
        if (Input.GetKeyDown(KeyCode.Space) && scanObject != null)
        {
            Debug.Log("eyCode.Spac: " + scanObject.name);
            manager.Action(scanObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // NPC 태그를 가진 오브젝트와 충돌할 때만 대화 가능한 상태로 설정
        if (collision.CompareTag("NPC"))
        {
            scanObject = collision.gameObject; // NPC를 scanObject에 저장
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            scanObject = null; // 범위를 벗어나면 대화 불가능 상태로 설정
        }
    }

    void FixedUpdate()
    {
        Vector2 moveVec = isHorizonMove ? new Vector2(inputVec.x, 0) : new Vector2(0, inputVec.y);
        rigid.linearVelocity = moveVec * Speed;
    }

    void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.sqrMagnitude);

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
}
