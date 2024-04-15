using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayer : Character
{
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 200;

    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;

    [SerializeField] private float jumpForce = 350;
    [SerializeField] private GameObject attackArea;

    private bool isGrounded= true;
    private bool isJumping = false;
    private bool isAttack;
    private bool isDead =false;
    private float horizontal;
    

    private int coin = 0;
    Vector3 savePoint;
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C");
        }
        //SavePoint();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        isGrounded = CheckGrouded();
        horizontal = Input.GetAxisRaw("Horizontal");

        if (isAttack)
        {
            //rb.velocity = Vector2.zero;
            return;
        }

        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
            
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.V) && isGrounded){
                Throw();
            }
        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        else if(isGrounded)
        {
            
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }

    }
    
    public override void  OnInit()
    {
        base.OnInit();
        isDead = false;
        isAttack = false;
        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();
        SavePoint();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        
    }
    public override void OnDespawn()
    {

        base.OnDespawn();
        OnInit();
    }

    private bool CheckGrouded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }

    private void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
        
    } 
    
    private void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.2f);
        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack = false;
        Debug.Log("reset");
        
    }

    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    } 
    
    private void Run()
    {

    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin")
        {
            coin++;
            Destroy(collision.gameObject);
        }
        if(collision.tag == "DeathZone")
        {
            isDead = true;
            ChangeAnim("die");
            Invoke(nameof(OnInit), 1f);
        }
    }
    internal void SavePoint()
    {
        savePoint = transform.position;
    }
    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }
    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }
}
