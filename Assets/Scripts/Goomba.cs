using UnityEngine;

public class Goomba : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;

    public float moveSpeed = 1.5f;
    public float activateDistance = 6f; // X 거리 기준 활성화

    private int dir = -1;
    private int state = 0;
    // 0 : 비활성
    // 1 : 이동
    // 2 : 밟힘

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (state == 2) return;

        CheckActivate();

        if (state == 1)
        {
            Move();
        }
    }

    void CheckActivate()
    {
        if (state != 0) return;
        if (player == null) return;

        float xDist = Mathf.Abs(player.position.x - transform.position.x);

        if (xDist <= activateDistance)
        {
            state = 1;
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
    }

    void Squash()
    {
        state = 2;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        animator.SetTrigger("dead");

        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state != 1) return;

        // 벽 충돌
        if (collision.gameObject.CompareTag("block") ||
            collision.gameObject.CompareTag("opened_box")||
            collision.gameObject.CompareTag("goomba"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Mathf.Abs(contact.normal.x) > 0.7f)
                {
                    dir *= -1;
                    break;
                }
            }
        }

        // 플레이어 충돌
        if (collision.gameObject.CompareTag("Player"))
        {
            bool stomped = false;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    stomped = true;
                    break;
                }
            }

            if (stomped)
            {
                Squash();

                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 8f);
            }
            else
            {
                collision.gameObject.GetComponent<Player>().Hit();
            }
        }
    }
}