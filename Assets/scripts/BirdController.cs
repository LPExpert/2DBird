using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    float horizontal;
    bool flight;
    public float speed = 3.0f;

    public float flyForce = 6.0f;

    //flight cooldown variables
    float flyCooldown = 0.3f;
    float flyCooldownTimer;

    //flight meter variables
    float flyMeter = 3.0f;
    float flyMeterTimer;
    float flyRecover = 1.0f;

    Rigidbody2D rigidbody2d;

    Animator animator;

    Vector2 lookDirection = new Vector2(1, 0);

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        flyCooldownTimer = flyCooldown;
        flyMeterTimer = flyMeter;
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        

        horizontal = Input.GetAxis("Horizontal");

        Vector2 move = new Vector2(horizontal, 0.0f);

        if (!Mathf.Approximately(move.x, 0.0f))
        {
            lookDirection.Set(move.x, 0.0f);
            lookDirection.Normalize();
        }

        animator.SetFloat("LookX", lookDirection.x);
        animator.SetFloat("Speed", move.magnitude);

        if (!isFlyDepleted())
        {


            //Fly in use

            if (Input.GetKey(KeyCode.Space) && !isFlyCooldown())
            {
                flight = true;
                animator.SetTrigger("TakeOff");
                
            }

            if (isFlyCooldown())
            {
                flyCooldownTimer -= Time.deltaTime;
            }
                
            
            if (isGrounded())
            {
                GroundRest();
            } 
            else
            {
                DepleteFlyMeter();
            }
            
        } 
        else if(isGrounded())
        {
            GroundRest();
        }

        Debug.Log(flyMeterTimer);
        animator.SetBool("isGrounded", isGrounded());
        animator.SetBool("isGliding", isFlyDepleted() || !Input.GetKey(KeyCode.Space));
    }

    private void FixedUpdate()
    {
        if (flight)
        {
            Fly();
        }
        rigidbody2d.velocity = new Vector2(speed * horizontal, rigidbody2d.velocity.y);
        
        
    }

    void Fly()
    {
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, flyForce);
        flight = false;
        flyCooldownTimer = flyCooldown;
    }

    void DepleteFlyMeter()
    {
        if (isFlyDepleted())
        {
            flyMeterTimer = 0;
        }
        else
        {
            flyMeterTimer -= Time.deltaTime;
        }
    }

    bool isFlyCooldown()
    {
        return flyCooldownTimer - Time.deltaTime > 0;
    }


    bool isFlyDepleted()
    {
        return flyMeterTimer - Time.deltaTime <= 0;
    }

    bool isFlyMeterFull()
    {
        return flyMeterTimer >= flyMeter;
    }

    // Recovers fly meter when bird is grounded
    void GroundRest()
    {
        if (isFlyMeterFull())
        {
            flyMeterTimer = flyMeter;
        }
        else
        {
            flyMeterTimer += Time.deltaTime * flyRecover;
        }
    }

    bool isGrounded()
    {
        RaycastHit2D ground = Physics2D.Raycast(rigidbody2d.position, Vector2.down, 0.1f, LayerMask.GetMask("Tile"));
        return ground.collider != null;
    }
}
