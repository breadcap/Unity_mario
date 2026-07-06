using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private Rigidbody2D rb;

    // 0 : 생성(올라오는 중)
    // 1 : 이동
    private int mushroom_state = 0;

    public float riseSpeed = 1f;      // 올라오는 속도
    public float moveSpeed = 3f;      // 이동 속도

    private Vector3 targetPos;
    private int dir = -1;              // 1 = 오른쪽, -1 = 왼쪽

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 박스 위로 0.5칸 올라올 때까지
        targetPos = transform.position + Vector3.up * 0.5f;

        // 올라오는 동안 중력 끄기
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        if (mushroom_state == 0)
        {
            Rise();
        }
        else if (mushroom_state == 1)
        {







            Move();







        }
    }

    void Rise()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            riseSpeed * Time.deltaTime);

        if (transform.position == targetPos)
        {
            mushroom_state = 1;
            rb.gravityScale = 1;
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("block") ||
            collision.gameObject.CompareTag("opened_box") ||
            collision.gameObject.CompareTag("mushroom_box") ||
            collision.gameObject.CompareTag("coin_box"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 좌우에서 부딪힌 경우
                if (Mathf.Abs(contact.normal.x) > 0.5f)
                {
                    dir *= -1;
                    break;
                }
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                player.Bigger();
            }

            Destroy(gameObject);
        }




    }
    
}