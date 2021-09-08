using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected AudioSource deathAudio;
    //青蛙刚体
    protected Collider2D collider2D;

    // Use this for initialization
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>(); 
        animator = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>();
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        deathAudio.Play();
        animator.SetTrigger("death");
    }

    private void OnDestroy()
    {
        //Death();
    }
}
