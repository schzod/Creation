using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anm;
    private enum State {idle, running, jumping, falling};
    private State state = State.idle;
    private Collider2D Col;
    [SerializeField] private LayerMask Gnd;
    [SerializeField] private float spd = 5f;
    [SerializeField] private float jmpf = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anm = GetComponent<Animator>();
        Col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        anm.SetFloat("speed", spd);
        Movement();
        AnimationState();
        anm.SetInteger("state", (int)state);
    }

    private void Movement()
    {
        float hdirection = Input.GetAxis("Horizontal");
        bool vdirection = Input.GetButtonDown("Jump") && Col.IsTouchingLayers(Gnd);

        //going left
        if (hdirection < 0)
        {
            rb.velocity = new Vector2(-spd, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        //going right
        else if (hdirection > 0)
        {
            rb.velocity = new Vector2(spd, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        //Jumping
        if (vdirection)
        {
            rb.velocity = new Vector2(rb.velocity.x, jmpf);
            state = State.jumping;
        }
    }

    private void AnimationState()
    {
         if (state == State.jumping)
        {
            if(rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        }
        else if(state == State.falling)
        {
            if(Col.IsTouchingLayers(Gnd))
            {
                state = State.idle;
            }
        }

        else if(Mathf.Abs(rb.velocity.x) > 2f)
        {
            //Moving
            state = State.running;
        }
        
        else
        {
            state = State.idle;
        }
    }
}
