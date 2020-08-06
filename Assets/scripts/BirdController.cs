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
    float flyDepletion = 0.4f;

    //hunger variables
    float hungerMeter = 6.0f;
    float hungerMeterTimer;
    float hungerRecoverRate = 2.0f;
    float hungerDepletion = 0.1f;

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
        hungerMeterTimer = hungerMeter;
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

        DepleteHungerMeter();
        animator.SetBool("isGrounded", isGrounded());
        animator.SetBool("isGliding", isFlyDepleted() || !Input.GetKey(KeyCode.Space));
        UIStaminaBar.instance.setValue(flyMeterTimer / flyMeter);
        UIHungerBar.instance.setValue(hungerMeterTimer / hungerMeter);
    }

    private void FixedUpdate()
    {
        if (flight)
        {
            Fly();
        }
        rigidbody2d.velocity = new Vector2(speed * horizontal, rigidbody2d.velocity.y);

        if (rigidbody2d.velocity.y < 0 && Input.GetKey(KeyCode.W))
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x * 1.5f, rigidbody2d.velocity.y * 0.8f);
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
        if(!flight && (rigidbody2d.velocity.y < 0 && Input.GetKey(KeyCode.W)))
        {
            if (isFlyDepleted())
            {
                flyMeterTimer = 0;
            }
            else
            {
                flyMeterTimer -= Time.deltaTime * flyDepletion;
            }
        }
        
    }

    void DepleteHungerMeter()
    {
        
        if (isHungerDepleted())
        {
            hungerMeterTimer = 0;
        }
        else
        {
            hungerMeterTimer -= Time.deltaTime * hungerDepletion;
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

    bool isHungerDepleted()
    {
        return hungerMeterTimer - Time.deltaTime <= 0;
    }

    bool isHungerMeterFull()
    {
        return hungerMeterTimer + hungerRecoverRate >= hungerMeter;
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

    public void hungerRecover()
    {
        if(isHungerMeterFull())
        {
            hungerMeterTimer = hungerMeter;
        }
        else
        {
            hungerMeterTimer += hungerRecoverRate;
        }
    }
}
