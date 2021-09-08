using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEagle : Enemy
{
    //private Rigidbody2D rb;
    //private Collider2D collider2D;
    public Transform top, bottom;
    public float speed;
    private float topY, bottomY;

    public AudioSource explosionAudio;

    private bool isUp = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        //rb = GetComponent<Rigidbody2D>();


        topY = top.position.y;
        bottomY = bottom.position.y;

        Destroy(top.gameObject);
        Destroy(bottom.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //SwitchAnimation();
        Movement();
    }

    void Movement()
    {
        if (isUp)
        {

            //x 轴不变， y 轴上下
            rb.velocity = new Vector2(rb.velocity.x, speed);
            
            //到达最上
            if (transform.position.y > topY)
            {
                isUp = false;
            }
        }
        else
        {
            
            rb.velocity = new Vector2(rb.velocity.x, -speed);
         

            //到达最小向上
            if (transform.position.y < bottomY)
            {
                //transform.localScale = new Vector3(1, 1, 1);
                isUp = true;
            }
        }
    }
}
