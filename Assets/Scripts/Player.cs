using System.Collections; // 👈 코루틴(IEnumerator)을 쓰기 위해 필수 추가!
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 9f;


    public TMP_Text Gameover_text;
    public GameObject uiCanvas;

    private string gameover_intext = "";

    private Rigidbody2D rb;
    private bool isGrounded;

    public bool is_big = false;
    private bool isInvincible = false; // 👈 1. 무적 상태 체크용 변수 추가

    private BoxCollider2D box;
    private Animator animator;

    private bool is_gameover = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
    }

    void Update()
    {

        if (is_gameover)
        {
            return;
        }



        float h = Input.GetAxisRaw("Horizontal");

        // 좌우 이동
        rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);

        // 좌우 반전
        if (h > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (h < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        animator.SetBool("is_running", h != 0);
        animator.SetBool("is_jumping", !isGrounded);


        // 만약 y가 -6 아래면 즉시 게임 오버

        if(transform.position.y < -6.0)
        {
            Gameover(0);
            Debug.Log("게임 오버!");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 땅 체크
        if (collision.gameObject.CompareTag("block") || collision.gameObject.CompareTag("mushroom_box") ||
            collision.gameObject.CompareTag("coin_box") || collision.gameObject.CompareTag("opened_box"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                    break;
                }
            }
        }

        // 박스 체크
        if (collision.gameObject.CompareTag("mushroom_box") ||
            collision.gameObject.CompareTag("coin_box"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 플레이어가 아래에서 박스를 침
                if (contact.normal.y < -0.5f)
                {
                    MysteryBox box = collision.gameObject.GetComponent<MysteryBox>();

                    if (box != null)
                    {
                        box.Hit();
                        
                    }

                    break;
                }
            }
        }


        if (collision.gameObject.CompareTag("flag"))           
        {
            // 클리어

            Gameover(1);
        }




    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("block"))
        {
            isGrounded = false;
        }
    }

    public void Bigger()
    {
        if (is_big) return;

        is_big = true;
        animator.SetBool("is_big", true);

        // 변경 전 발 위치 저장
        float footY = transform.position.y + box.offset.y - box.size.y / 2f;

        // 큰 마리오 콜라이더
        box.size = new Vector2(0.8f, 1.8f);
        box.offset = new Vector2(0f, 0.06f);

        // 발 위치 유지
        float newPosY = footY - box.offset.y + box.size.y / 2f;
        transform.position = new Vector3(
            transform.position.x,
            newPosY,
            transform.position.z
        );
    }

    public void Smaller()
    {
        if (!is_big) return;

        is_big = false;
        animator.SetBool("is_big", false);

        // 변경 전 발 위치 저장
        float footY = transform.position.y + box.offset.y - box.size.y / 2f;

        // 작은 마리오 콜라이더
        box.size = new Vector2(0.8f, 0.8f);
        box.offset = new Vector2(0f, 0f);

        // 발 위치 유지
        float newPosY = footY - box.offset.y + box.size.y / 2f;
        transform.position = new Vector3(
            transform.position.x,
            newPosY,
            transform.position.z
        );

        // 👈 2. 작아진 직후 무적 코루틴 시작
        StartCoroutine(InvincibleRoutine());
    }

    

    public void Hit()
    {
        //  무적 상태일 때는 몬스터에게 부딪혀도 아래 대미지 로직을 무시하고 리턴
        if (isInvincible) return;

        if (is_big)
        {
            Smaller();
        }
        else
        {
            Debug.Log("게임 오버");
            Gameover(0);
        }
    }

    
    public void Gameover(int state)
    {


        uiCanvas.SetActive(true);

        if(state == 0)
        {
            gameover_intext = "GAME OVER!!!";

        }
        if(state == 1)
        {
            gameover_intext = "GAME CLEAR!";

        }



        Gameover_text.text = gameover_intext;
    }
    
    
    // 👈 4. 1초 동안 무적을 유지하는 코루틴 함수 추가
    private IEnumerator InvincibleRoutine()
    {
        isInvincible = true;
        
        // (선택사항) 무적 상태일 때 캐릭터를 반투명하게 하거나 깜빡거리게 만들면 좋습니다.
        // GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);

        yield return new WaitForSeconds(1f); // 1초 대기

        // (선택사항) 무적이 끝나면 원래 색상으로 복구
        // GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        isInvincible = false;
    }
}