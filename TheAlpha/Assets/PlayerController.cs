using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anm;
    private enum State {idle, running, jumping};
    private State state = State.idle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anm = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        float hdirection = Input.GetAxis("Horizontal");
        float vdirection = Input.GetAxis("Vertical");
        
        if(hdirection < 0)
        {
            rb.velocity = new Vector2(-5, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        else if (hdirection > 0)
        {
            rb.velocity = new Vector2(5, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        else
        {
            
        }

        if (vdirection > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 10);
            state = State.jumping;
        }

        velocityState();
        anm.SetInteger("state", (int)state);
    }

    private void velocityState()
    {
         if (state == State.jumping)
        {

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
