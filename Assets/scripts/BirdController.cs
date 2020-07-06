using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    float horizontal;
    bool flight;
    public float speed;

    public float flyForce;

    //flight cooldown variables
    float flyCooldown = 0.3f;
    float flyCooldownTimer;

    //flight meter variables
    float flyMeter = 6.0f;
    float flyMeterTimer;
    float flyRecover = 2.0f;

    Rigidbody2D rigidbody2d;

    Animator animator;

    public CircleCollider2D body;
    public BoxCollider2D feet;

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
        UIStaminaBar.instance.setValue(flyMeterTimer / flyMeter);
    }

    private void FixedUpdate()
    {
        if (flight)
        {
            Fly();
        }
        rigidbody2d.velocity = new Vector2(speed * horizontal, rigidbody2d.velocity.y);

        if (rigidbody2d.velocity.y < 0 && Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, rigidbody2d.velocity.y * 0.8f);
        }        
        
    }

    void Fly()
    {
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, flyForce);
        flight = false;
        flyCooldownTimer = flyCooldown;

        if (flyMeterTimer - 0.3f <= 0)
        {
            flyMeterTimer = 0;
        }
        else
        {
            flyMeterTimer -= 0.3f;
        }
    }

    void DepleteFlyMeter()
    {
        if(!flight && (rigidbody2d.velocity.y < 0 && Input.GetKey(KeyCode.UpArrow)))
        {
            if (isFlyDepleted())
            {
                flyMeterTimer = 0;
            }
            else
            {
                flyMeterTimer -= Time.deltaTime * 0.5f;
            }
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
        return feet.IsTouchingLayers(LayerMask.GetMask("Tile"));
    }
}
