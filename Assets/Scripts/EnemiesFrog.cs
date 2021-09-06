using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesFrog : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    //青蛙刚体
    private Collider2D collider2D;

    //地面
    public LayerMask ground;

    public Transform leftPoint, rightPoint;
    public float speed, jumpForce;

    public float leftX, rightX;
    private bool faceLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();

        //处理 left objcet 和 rightobject 跟随青蛙移动的问题

        //1：
        //断绝父子关系，
        transform.DetachChildren();

        //方法二
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        //清理内容
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //播放完 idle 动画后，再调用 Movement 
        //Movement();

        SwitchAnimation();
    }

    void Movement()
    {
        if (faceLeft)
        {
            if (collider2D.IsTouchingLayers(ground))
            {
                rb.velocity = new Vector2(-speed, jumpForce);
                animator.SetBool("jumping", true);
            }
         

            //到达最左掉头
            if (transform.position.x < leftX)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceLeft = false;
            }
        }
        else
        {
            if (collider2D.IsTouchingLayers(ground))
            {
                rb.velocity = new Vector2(+speed, jumpForce);
                animator.SetBool("jumping", true);
            }

          

            //到达最优掉头
            if (transform.position.x > rightX)
            {
                transform.localScale = new Vector3(1, 1, 1);
                faceLeft = true;
            }
        }
    }

    void SwitchAnimation()
    {
        if (animator.GetBool("jumping"))
        {
            if (rb.velocity.y < 0.1)
            {
                animator.SetBool("jumping", false);
                animator.SetBool("falling", true);
            }
        }

        //当下楼碰到地面时，切换回 idle 动画
        if (rb.IsTouchingLayers(ground))
        {
            animator.SetBool("falling", false);
        }
    }
}
