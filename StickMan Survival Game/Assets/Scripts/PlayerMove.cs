using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
 
    public LayerMask ground;
    public GameObject checkGround;

    [SerializeField]
    private float groundSpeed;  //Player speed on the ground
    [SerializeField]
    private float airSpeed;  //player speed in the air.
    [SerializeField]
    private float jumpforceY; 
    [SerializeField]
    private float jumpforceX;
    private bool isGrounded;
    private Rigidbody2D rb;

    private bool isRight = true;
    private bool isJumping = false;
    private Animator anim;

    void Start ()
    { //intialising variable.
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isGrounded = false;
    }

    float x;
    float jump;

    void Update () {                     //update function for nmormal emthod calls.
        x = Input.GetAxis("Horizontal");            //taking input.
        if (isJumping)
        {
            anim.SetBool("jump", true);
            layerHandler(isGrounded);
        }
        if(isGrounded)
        {
            anim.SetBool("jump", false);
            anim.SetBool("land", false);
            layerHandler(isGrounded);
        }
        if (rb.velocity.y < 0)
        {
            anim.SetBool("land", true);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetTrigger("punch");
        }
	}

    void FixedUpdate()   //fixedUpdate function for physics realted function calls.   
    {
        if (Physics2D.Raycast(checkGround.transform.position, Vector2.down, 0.1f, ground))
        {
            isGrounded = true;
        }
        Move(x);
        Flip(x);
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        if (Input.GetKeyDown(KeyCode.W))             //jump input.
        {
            isJumping = Jump();
        }
    }

    private void layerHandler(bool isgrounded)         //handling animation layer.
    {
        if (isgrounded)
        {
            anim.SetLayerWeight(1, 0);
        }
        else
        {
            anim.SetLayerWeight(1, 1);
        }
    }

    private void Move(float x) //player move by this functiomn.
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(x * groundSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(x*airSpeed, rb.velocity.y);
        }
    }

    private bool Jump()                 //jumping 
    {
        if (isGrounded)
        {
            Vector2 jumpf = new Vector2(0f, jumpforceY);
            isGrounded = false;
            // Debug.Log("jumf : " + jumpf);
            rb.AddForce(jumpf);
            return true;
        }
        else
            return false;
    }

    private void Flip(float x) //to flip sprite in the direction of walking.
    {
        if (isRight && x < 0 || !isRight && x>0)
        {
            isRight = !isRight;
            Vector2 f = transform.localScale;
            f.x *= -1;
            transform.localScale = f;
        }
    }
}
