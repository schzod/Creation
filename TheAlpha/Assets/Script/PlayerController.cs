using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Game Component
    private Rigidbody2D rb;
    private Animator anm;
    
    //FSM
    private enum State {idle, running, jumping, falling};
    private State state = State.idle;
    private Collider2D Col;
    
    //Serialize Fields
    [SerializeField] private LayerMask Gnd;
    [SerializeField] private float spd = 5f;
    [SerializeField] private float jmpf = 10f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text CherryText;



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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            CherryText.text = cherries.ToString();
        }
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
