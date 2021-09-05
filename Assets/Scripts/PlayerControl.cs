using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //定义刚体
    private Rigidbody2D rb;

    //SerializeField 可将私有变量在 u3d 中看见对应组件
    [SerializeField]private Animator animator;

    public float speed;
    public float jumpForce;

    //获取绘制地面，需要新建 ground Layer 
    public LayerMask ground;

    //狐狸碰撞区域
    public Collider2D collider2D;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Movement();
        SwitchAnimation();
    }

    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal"); 
        float facedirection = Input.GetAxisRaw("Horizontal");

        Debug.Log($"horizontalMove: {horizontalMove} facedirection: {facedirection}");
        //角色移动
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime, rb.velocity.y); //Time.deltaTime 物理时间百分比
            //调用动画，奔跑
            animator.SetFloat("running", Mathf.Abs(facedirection));
        }

        //向前还是向后
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }

        //角色跳跃
        if (Input.GetButtonDown("Jump")) {
            //给 Y 轴设置跳跃力
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);

            //设置跳跃值
            animator.SetBool("jumping", true);
        }
    }

    //切换动画
    void SwitchAnimation()
    {
        //正在跳跃
        if (animator.GetBool("jumping")) {
            ////如果 Y 轴设置跳跃力消失
            if (rb.velocity.y < 0)
            {
                animator.SetBool("jumping", false);
                animator.SetBool("falling", true);
            }
        } else if (collider2D.IsTouchingLayers(ground)) { //当狐狸碰撞到地面
            animator.SetBool("falling", false); 
            animator.SetBool("idle", true);
        }
    }
}
