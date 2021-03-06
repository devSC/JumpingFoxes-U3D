using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    //定义刚体
    private Rigidbody2D rb;

    //SerializeField 可将私有变量在 u3d 中看见对应组件
    [SerializeField]private Animator animator;

    public AudioSource jumpAudioSource, hurtAudioSource, cherryAudio;

    public float speed;
    public float jumpForce;

    //获取绘制地面，需要新建 ground Layer 
    public LayerMask ground;

    //狐狸碰撞区域
    public Collider2D collider2D;
    public Collider2D headerCollider;

    public int cherry; //count

    public Text cherryNumberText;

    //用来检查头部是否碰到墙体
    public Transform cellingTransform;

    //受伤中，不需要执行滑动
    private bool isHurt;


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
        if (!isHurt)
        {
            Movement();
        }
        SwitchAnimation();
    }

    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal"); 
        float facedirection = Input.GetAxisRaw("Horizontal");

        //Debug.Log($"horizontalMove: {horizontalMove} facedirection: {facedirection}");
        //角色移动
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y); //Time.deltaTime 物理时间百分比
            //调用动画，奔跑
            animator.SetFloat("running", Mathf.Abs(facedirection));
        }

        //向前还是向后
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }

        //角色跳跃
        if (Input.GetButtonDown("Jump") && collider2D.IsTouchingLayers(ground)) {
            //给 Y 轴设置跳跃力

            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);

            //设置跳跃值
            animator.SetBool("jumping", true);

            //播放跳跃动画
            jumpAudioSource.Play();
        }

        Crouch();
    }

    //切换动画
    void SwitchAnimation()
    {

        //fix 从高处掉落时，敌人无法消失的问题
        if (rb.velocity.y < 0.1f && !collider2D.IsTouchingLayers(ground))
        {
            animator.SetBool("falling", true);
        }

        //正在跳跃
        if (animator.GetBool("jumping"))
        {
            ////如果 Y 轴设置跳跃力消失
            if (rb.velocity.y < 0)
            {
                animator.SetBool("jumping", false);
                animator.SetBool("falling", true);
            }
        }
        else if (isHurt) {

            animator.SetFloat("running", 0);

            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                animator.SetBool("hurting", false);
                animator.SetBool("idle", true); 
                isHurt = false;
            } else
            {
                
            }
        }
        else if (collider2D.IsTouchingLayers(ground))
        { //当狐狸碰撞到地面
            animator.SetBool("falling", false);
            animator.SetBool("idle", true);
        }
        //
    }

    //当狐狸碰到收集类的tag 如：樱桃等
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            cherryAudio.Play();

            Destroy(collision.gameObject);
            cherry += 1;
            cherryNumberText.text = cherry.ToString();
        }
        else if (collision.tag == "Deadline")
        {
            //将所有的背景音乐停止
            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);
        }
    }


    private void Restart()
    {
        //重置当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemies") {

            //get enemy game object
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (animator.GetBool("falling")) {

                enemy.JumpOn();
                Debug.Log($"enemy: {enemy}");
                //
                //float scale = 0.4;
                //踏上之后加个小跳
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime * 0.4f);

                //设置跳跃值
                animator.SetBool("jumping", true);
            }
            else
            {
                isHurt = true;

                hurtAudioSource.Play();

                animator.SetBool("hurting", true);
                if (transform.position.x < collision.gameObject.transform.position.x)
                {
                    //碰到敌人向左移10
                    rb.velocity = new Vector2(-5, rb.velocity.y);
                    
                }
                else if (transform.position.x > collision.gameObject.transform.position.x)
                {
                    //碰到敌人向右移10
                    rb.velocity = new Vector2(5, rb.velocity.y);
                }
            }


        }
    }

    private void Crouch()
    {
        //判断顶部是否有墙
        if (!Physics2D.OverlapCircle(cellingTransform.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch"))
            {
                animator.SetBool("crouching", true);
                headerCollider.enabled = false;
            }
            else 
            {
                animator.SetBool("crouching", false);
                headerCollider.enabled = true;
            }
        }


    }


}
